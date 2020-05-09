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
    public class TeamMemberListViewComponent : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public TeamMemberListViewComponent(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int teamId, int userId)
        {
            ViewData["CurrentUser"] = GetCurrentUser();
            var team = _context.Team.Find(teamId);
            ViewData["TeamOwner"] = _context.User.Where(x => x.UserId == team.FkOwner).FirstOrDefault();
            if (userId > 0)
            {
                var user = _context.TeamMember.FirstOrDefault(x => x.FkUser == userId);

                if (user == null)
                {
                    var invites = _context.TeamInvitation.Where(x => x.FkUser == userId);
                    _context.RemoveRange(invites);
                    _context.TeamMember.Add(new TeamMember { FkTeam = teamId, FkUser = userId });
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.TeamMember.Remove(user);
                    await _context.SaveChangesAsync();
                }

            }
            var teamMemberList = _context.TeamMember.Where(p => p.FkTeam == teamId).Include(x => x.FkUserNavigation);
            return View(teamMemberList);
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
