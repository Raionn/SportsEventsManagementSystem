using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;
using SportBook.ViewModels;

namespace SportBook.ViewComponents
{
    public class EventMemberList : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public EventMemberList(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int eventId, int userId)
        {
            ViewData["CurrentUser"] = GetCurrentUser();
            var eventItem = _context.Event.Find(eventId);
            ViewData["TeamOwner"] = _context.User.Where(x => x.UserId == eventItem.FkOwner).FirstOrDefault();
            var eventMemberList = _context.Participant.Where(p => p.FkEvent == eventId).Include(x => x.FkUserNavigation);
            if (eventItem.MaxParticipantAmt > eventMemberList.Count())
            {
                if (userId > 0)
                {
                    var user = _context.Participant.FirstOrDefault(x => x.FkUser == userId && x.FkEvent == eventId);

                    if (user == null)
                    {
                        var invites = _context.EventInvitation.Where(x => x.FkUser == userId);
                        _context.RemoveRange(invites);
                        _context.Participant.Add(new Participant { FkEvent = eventId, FkUser = userId });
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Participant.Remove(user);
                        await _context.SaveChangesAsync();
                    }

                }
            }


            return View(eventMemberList);
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