using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportBook.ViewModels;
using SportBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace SportBook.Controllers
{
    //[Authorize(Roles ="user, admin")]
    public class HomeController : Controller
    {
        private readonly SportbookDatabaseContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, SportbookDatabaseContext context)
        {
            _logger = logger;
            _context = context;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Schedule()
        {
            var eventParticipations = _context.Participant.Where(e => e.FkUser == GetCurrentUser().UserId);
            var _events = _context.Event;
            var eventData = from first in _events
                                       join second in eventParticipations
                                       on first.EventId equals second.FkEvent
                                       select first;
            var joinedEvents = eventData.Include(e => e.FkGameTypeNavigation);
            var createdEvents = _events.Include(e => e.FkGameTypeNavigation).Where(e => e.FkOwner == GetCurrentUser().UserId);
            var returnEvents = new List<ScheduleData>();
            foreach (var item in joinedEvents)
            {
                if (item.FkGameTypeNavigation.IsOnline)
                {
                    returnEvents.Add(new ScheduleData(item.Title, item.StartTime,
                    item.EndTime, "../Esports/ViewEvent?id=" + item.EventId.ToString(), "info"));
                }
                else
                {
                    returnEvents.Add(new ScheduleData(item.Title, item.StartTime,
                    item.EndTime, "../Sports/ViewEvent?id=" + item.EventId.ToString(), "info"));
                }
            }
            foreach (var item in createdEvents)
            {
                if (item.FkGameTypeNavigation.IsOnline)
                {
                    returnEvents.Add(new ScheduleData(item.Title, item.StartTime,
                    item.EndTime, "../Esports/ViewEvent?id=" + item.EventId.ToString(), "important"));
                }
                else 
                {
                    returnEvents.Add(new ScheduleData(item.Title, item.StartTime,
                    item.EndTime, "../Sports/ViewEvent?id=" + item.EventId.ToString(), "important"));
                }
                
            }
            ViewData["events"] = returnEvents;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
