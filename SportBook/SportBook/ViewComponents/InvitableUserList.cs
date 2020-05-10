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
    public class InvitableUserListViewComponent : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public InvitableUserListViewComponent(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int eventId, int userId)
        {
            ViewData["CurrentUser"] = GetCurrentUser();
            var @event = _context.Event.Find(eventId);
            ViewData["EventOwner"] = _context.User.Where(x => x.UserId == @event.FkOwner).FirstOrDefault();
            if (userId > 0)     // if Invite is clicked
            {
                var user = _context.Participant.FirstOrDefault(x => x.FkUser == userId && x.FkEvent == eventId);

                if (user == null)
                {   // user is not a participant
                    var userAlreadyInvited = _context.EventInvitation.Any(x => x.FkUser == userId && x.IsAccepted);     // FALSE if user hasn't accepted an invitation
                    if (!userAlreadyInvited)
                    {
                        _context.EventInvitation.Add(new EventInvitation() { FkEvent = eventId, FkUser = userId });
                        await _context.SaveChangesAsync();
                    }
                }
            }
            //var participantList = _context.User.Where(u => u.UserId == eventId).Include(x => x.FkUserNavigation);
            var alreadyParticipants = from users in _context.User
                              join participants in _context.Participant.Where(x => x.FkEvent == eventId)
                              on users.UserId equals participants.FkUser
                              select users;
            var notAcceptedInvite = from users in _context.User
                          join invitations in _context.EventInvitation.Where(x => x.IsAccepted == false)
                          on users.UserId equals invitations.FkUser
                          select users;

            var participantList = _context.User.Except(alreadyParticipants).Except(notAcceptedInvite);
            return View(participantList);
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
