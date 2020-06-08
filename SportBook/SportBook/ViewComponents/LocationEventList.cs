using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.ViewComponents
{
    public class LocationEventList : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public LocationEventList(SportbookDatabaseContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int selectedLocationId)
        {
            var events = _context.Event.Include(e => e.FkGameTypeNavigation)
                           .Include(e => e.FkLocationNavigation)
                           .Include(e => e.FkOwnerNavigation)
                           .Where(e => e.FkGameTypeNavigation.IsOnline == false)
                           .Where(e => e.EndTime > DateTime.Now)
                           .Where(e => e.FkLocation == selectedLocationId);

            var participants = new Dictionary<int, int>();
            var eventList = events.ToList();
            foreach (var item in eventList)
            {
                participants.Add(item.EventId, _context.Participant.Where(x => x.FkEvent == item.EventId).Count());
            }
            ViewData["SelectedLocation"] = selectedLocationId;
            ViewData["Participants"] = participants;

            return View(events);
        }
    }
}
