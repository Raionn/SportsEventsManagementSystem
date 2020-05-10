using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportBook.ViewComponents
{
    public class InviteTeamMemberListViewComponent : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public InviteTeamMemberListViewComponent(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int teamId, int userId)
        {
            ViewData["CurrentUser"] = GetCurrentUser();
            var team = _context.Team.Find(teamId);
            ViewData["TeamOwner"] = _context.User.Where(x => x.UserId == team.FkOwner).FirstOrDefault();
            if (userId > 0)     // if Invite is clicked
            {
                var user = _context.TeamMember.FirstOrDefault(x => x.FkUser == userId && x.FkTeam == teamId);

                if (user == null)
                {   // user is not a team member
                    var userAlreadyInvited = _context.TeamInvitation.Any(x => x.FkUser == userId && x.IsAccepted);     // FALSE if user hasn't accepted an invitation
                    if (!userAlreadyInvited)
                    {
                        _context.TeamInvitation.Add(new TeamInvitation() { FkTeam = teamId, FkUser = userId });
                        await _context.SaveChangesAsync();
                    }
                }
            }
            //var participantList = _context.User.Where(u => u.UserId == eventId).Include(x => x.FkUserNavigation);
            var alreadyParticipants = from users in _context.User
                                      join teamMembers in _context.TeamMember.Where(x => x.FkTeam == teamId)
                                      on users.UserId equals teamMembers.FkUser
                                      select users;
            var notAcceptedInvite = from users in _context.User
                                    join invitations in _context.TeamInvitation.Where(x => x.IsAccepted == false)
                                    on users.UserId equals invitations.FkUser
                                    select users;

            var userList = _context.User.Except(alreadyParticipants).Except(notAcceptedInvite);
            return View(userList);
        }

        private User GetCurrentUser()
        {
            var externalId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = _context.User
                           .Where(s => s.ExternalId == externalId)
                           .FirstOrDefaultAsync();
            user.Wait();
            return user.Result;
        }
    }
}
