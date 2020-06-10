using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportBook.Helpers;
using SportBook.Models;
using SportBook.ViewModels;

namespace SportBook.Controllers
{
    [Authorize(Roles ="user,admin")]
    public class SportsController : Controller
    {
        private readonly SportbookDatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly string google_key;

        public SportsController(SportbookDatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            google_key = _configuration.GetValue<string>("ConnectionStrings:GOOGLE_API");
        }
        [Route("[action]")]           // MUST FIX THIS LATER
        public IActionResult Sports()
        {
            var userId = GetCurrentUser().UserId;

            var events = _context.Event.Include(e => e.FkGameTypeNavigation)
                                       .Include(e => e.FkLocationNavigation)
                                       .Include(e => e.FkOwnerNavigation)
                                       .Where(e => e.FkGameTypeNavigation.IsOnline == false)
                                       .Where(x => x.EndTime > DateTime.Now);

            var participants = new Dictionary<int, int>();
            var eventList = events.ToList();
            foreach (var item in eventList)
            {
                participants.Add(item.EventId, _context.Participant.Where(x => x.FkEvent == item.EventId).Count());
            }
            var eventParticipations = _context.Participant.Where(e => e.FkUser == userId);

            var myEvents = events.Where(e => e.FkOwner == userId);
            var joinedEvents = from first in events
                               join second in eventParticipations
                               on first.EventId equals second.FkEvent
                               select first; ;
            ViewData["myEvents"] = myEvents.OrderBy(x => x.StartTime);
            ViewData["joinedEvents"] = joinedEvents.OrderBy(x => x.StartTime).Except(myEvents);
            ViewData["Participants"] = participants;
            ViewData["GoogleApi"] = google_key;

            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            ViewData["CurrentUser"] = currentUser;
            //events = events.Except(myEvents).Except(joinedEvents).OrderBy(x => x.StartTime);
            events = events.OrderBy(x => x.StartTime);

            var locationsData = _context.Location.Include(t => t.FkGameTypeNavigation)
                                                 .Where(loc => loc.FkGameTypeNavigation.IsOnline == false)
                                                 .Where(loc => _context.Event.First(ev => ((ev.FkLocation == loc.LocationId) && (ev.EndTime > DateTime.Now))) != null)
                                                 .ToList();
            List<LocationData> locations = new List<LocationData>();
            foreach (var item in locationsData)
            {
                locations.Add(new LocationData(item.Longitude, item.Latitude, item.Address, item.FkGameTypeNavigation.Name,
                                               item.LocationId, item.FkGameType, events.Count(evnt => evnt.FkLocation == item.LocationId)));
            }
            ViewData["Locations"] = locations;

            return View(events);
        }
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            var sportbookDatabaseContext = _context.Location.Include(t => t.FkGameTypeNavigation).Where(e => e.FkGameTypeNavigation.IsOnline == false);
            List<LocationData> locations = new List<LocationData>();
            foreach (var item in sportbookDatabaseContext)
            {
                locations.Add(new LocationData(item.Longitude, item.Latitude, item.Address, item.FkGameTypeNavigation.Name, item.LocationId, item.FkGameType));
            }
            ViewData["Locations"] = locations;
            ViewData["GoogleApi"] = google_key;
            return View();
        }
        [Route("[controller]/[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,MaxParticipantAmt,StartTime,EndTime,IsPrivate,FkLocation,FkGameType")] Event @event)
        {
            @event.IsTeamEvent = false;
            var user = GetCurrentUser();
            @event.FkOwner = user.UserId;
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                _context.Participant.Add(new Models.Participant() { FkEvent = @event.EventId, FkUser = user.UserId });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Sports));
            }
            var sportbookDatabaseContext = _context.Location.Include(t => t.FkGameTypeNavigation).Where(e => e.FkGameTypeNavigation.IsOnline == false);
            List<LocationData> locations = new List<LocationData>();
            foreach (var item in sportbookDatabaseContext)
            {
                locations.Add(new LocationData(item.Longitude, item.Latitude, item.Address, item.FkGameTypeNavigation.Name, item.LocationId, item.FkGameType));
            }
            ViewData["Locations"] = locations;
            ViewData["GoogleApi"] = google_key;
            return View(@event);
        }
        [Route("[controller]/[action]")]
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyDateTime(string StartTime, string EndTime, string FkLocation)
        {
            if (FkLocation != null)
            {
                if (StartTime != null && EndTime != null)
                {
                    if (!DateTimeValidator(ref StartTime, ref EndTime, FkLocation))
                    {
                        return Json($"Time frame {StartTime} - {EndTime} is taken");
                    }
                }
            }
            return Json(true);
        }
        private bool DateTimeValidator(ref string startTime, ref string endTime, string FkLocation)
        {
            int fkKey = int.Parse(FkLocation);
            var events = _context.Event.Where(x => x.FkLocation != null && x.FkLocation == fkKey).ToList();
            var start = DateTime.Parse(startTime);
            var end = DateTime.Parse(endTime);
            foreach (var item in events)
            {
                if ((start < item.StartTime && end > item.EndTime) || (item.StartTime < start && end < item.EndTime) || (item.StartTime > start && end < item.EndTime && item.StartTime < end) || (item.StartTime < start && end > item.EndTime && item.EndTime > start))
                {
                    startTime = item.StartTime.Value.ToString("yyyy-MM-dd HH:mm");
                    endTime = item.EndTime.Value.ToString("yyyy-MM-dd HH:mm");
                    return false;
                }
                    
            }
            return true;
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

        private bool IsEventOwner(int? id)
        {

            return _context.Event.Find(id).FkOwner == GetCurrentUser().UserId;
        }

        private bool IsInvited(int? id)
        {
            var user = GetCurrentUser();
            var _event = _context.Event.Find(id);
            var member = _context.Participant.FirstOrDefault(x => x.FkEvent == id && x.FkUser == user.UserId);
            if (_event.IsPrivate && _event.FkOwner != user.UserId && member == null)
                return _context.EventInvitation.Any(e => (e.FkEvent == id) && (user.UserId == e.FkUser));
            else return true;
        }

        #region Event crud w.o create
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            if (!IsEventOwner(id))
            {
                return Forbid();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", @event.FkGameType);
            ViewData["FkLocation"] = new SelectList(_context.Location, "LocationId", "Address", @event.FkLocation);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", @event.FkOwner);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Title,MaxParticipantAmt,StartTime,EndTime,IsPrivate,IsTeamEvent,EventId,FkOwner,FkLocation,FkGameType")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewEvent), new { id = @event.EventId });
            }
            var tempEvent = await _context.Event.Include(e => e.FkGameTypeNavigation).FirstOrDefaultAsync(m => m.EventId == id);
            @event.FkGameTypeNavigation = tempEvent.FkGameTypeNavigation;
            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            ViewData["CurrentUser"] = currentUser;
            ViewData["EventInvites"] = _context.EventInvitation.Where(x => x.FkEvent == id);
            ViewData["isFailed"] = true;
            var eventMembers = await _context.Participant.Include(x => x.FkUserNavigation).Include(x => x.FkEventNavigation).Where(x => x.FkEvent == id).ToListAsync();
            EventData eventData = new EventData(@event, eventMembers, new Models.Participant());
            ViewData["FkLocation"] = new SelectList(_context.Location, "LocationId", "Address", @event.FkLocation);
            return View("ViewEvent", eventData);
        }

        // GET: Events/Delete/5
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!IsEventOwner(id))
            {
                return Forbid();
            }

            var @event = await _context.Event
                .Include(e => e.FkGameTypeNavigation)
                .Include(e => e.FkLocationNavigation)
                .Include(e => e.FkOwnerNavigation)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventParticipants = _context.Participant.Where(x => x.FkEvent == id);
            var eventInvitations = _context.EventInvitation.Where(x => x.FkEvent == id);
            var @event = await _context.Event.FindAsync(id);
            _context.Participant.RemoveRange(eventParticipants);
            _context.EventInvitation.RemoveRange(eventInvitations);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Sports));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LocationCreate([Bind("Latitude,Longitude,Address,LocationId,FkCity,FkGameType")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Sports));
        }

        [Route("[controller]/[action]")]
        public IActionResult LocationSuggestionVC(string addr, decimal latitude, decimal longitude)
        {
            addr = addr.Replace("_", " ");
            return ViewComponent("LocationSuggestion", new { addr, latitude, longitude });
        }

        [Route("[controller]/[action]/{userId}")]
        public IActionResult SportsEventMemberVC(int eventId, int userId)
        {
            return ViewComponent("EventMemberList", new { eventId, userId });
        }

        [Route("[controller]/[action]")]
        public IActionResult LocationEventListVC(int selectedLocationId)
        {
            return ViewComponent("LocationEventList", new { selectedLocationId });
        }

        [HttpGet("{userId}")]
        [Route("[controller]/[action]")]
        public IActionResult InvitableUserVC(int eventId, int userId)
        {
            return ViewComponent("InvitableUserList", new { eventId, userId });
        }
        [Route("[controller]/[action]")]
        public IActionResult ChatroomVC(string chatGroup)
        {
            return ViewComponent("Chatroom", chatGroup);
        }
        [Route("[controller]/[action]")]
        public async Task<IActionResult> ViewEvent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!IsInvited(id))
                return Forbid();

            var @event = await _context.Event
                .Include(e => e.FkGameTypeNavigation)
                .Include(e => e.FkLocationNavigation)
                .Include(e => e.FkOwnerNavigation)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null || @event.FkGameTypeNavigation.IsOnline)
            {
                return NotFound();
            }
            List<LocationData> locations = new List<LocationData>
            {
                new LocationData(@event.FkLocationNavigation.Longitude, @event.FkLocationNavigation.Latitude, @event.FkLocationNavigation.Address, @event.FkGameTypeNavigation.Name, 0, 0)
            };

            ViewData["Locations"] = locations;
            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            ViewData["CurrentUser"] = currentUser;
            ViewData["GoogleApi"] = google_key;
            ViewData["EventInvites"] = _context.EventInvitation.Where(x => x.FkEvent == id);
            ViewData["isFailed"] = false;
            var eventMembers = await _context.Participant.Include(x => x.FkUserNavigation).Include(x => x.FkEventNavigation).Where(x => x.FkEvent == id).ToListAsync();
            EventData eventData = new EventData(@event, eventMembers, new Models.Participant());

            return View(eventData);
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SendInvitation([FromBody]JsonElement jsonbody)
        {
            var data = JsonSerializer.Deserialize<EventInvite>(jsonbody.GetRawText());
            EventInvitation invitation = new EventInvitation() { FkUser = int.Parse(data.FkUser), FkEvent = int.Parse(data.FkEvent)};

            if (invitation.FkEvent > 0 && invitation.FkUser > 0)
            {
                _context.Add(invitation);
                await _context.SaveChangesAsync();
            }

            var users = new SelectList(_context.User.Where(u => u.UserId != GetCurrentUser().UserId), "UserId", "Username");

            return new JsonResult(JsonSerializer.Serialize(users));
        }
    }
}