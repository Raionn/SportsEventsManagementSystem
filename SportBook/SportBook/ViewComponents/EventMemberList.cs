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

        public IViewComponentResult Invoke(int eventId, int userId)
        {
            List<User> users = GetItemsAsync(eventId, userId);
            return View(users);
        }
        private List<User> GetItemsAsync(int eventId, int userId)
        {
            if (userId > 0)
            {
                var user = _context.Participant.FirstOrDefault(x => x.FkUser == userId);

                if (user == null)
                {
                    _context.Participant.Add(new Participant { FkEvent = eventId, FkUser = userId });
                    _context.SaveChanges();
                }
                else
                {
                    _context.Participant.Remove(user);
                    _context.SaveChanges();
                }

            }
            var participantsList = _context.Participant.Where(p => p.FkEvent == eventId);
            var usersInEvent = (from users in _context.User
                               join participants in participantsList on users.UserId equals participants.FkUser
                               select users).ToList();
            return usersInEvent;
        }
    }
}