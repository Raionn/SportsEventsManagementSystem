using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;
using SportBook.ViewModels;

namespace SportBook.Controllers
{

    //[Authorize(Roles ="admin")]
    public class SportsController : Controller
    {
        private readonly SportbookDatabaseContext _context;

        public SportsController(SportbookDatabaseContext context)
        {
            _context = context;
        }
        [Route("[action]")]
        public async Task<IActionResult> Sports(string title, string gametype, string city)
        {
            var sportbookDatabaseContext = _context.Event.Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation).Where(e => !e.FkGameTypeNavigation.IsOnline).Where(x => x.IsPrivate == false).OrderBy(x => x.StartTime);
            if (!String.IsNullOrEmpty(title))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.Title.Contains(title)).Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation).OrderBy(x => x.StartTime);
            }
            if (!String.IsNullOrEmpty(gametype))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.FkGameTypeNavigation.Name.Contains(gametype)).Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation).OrderBy(x => x.StartTime);
            }
            if (!String.IsNullOrEmpty(city))
            {
                var _cities = _context.City;
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.FkLocationNavigation.FkCityNavigation.Name.Contains(city)).Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation).OrderBy(x => x.StartTime);
            }
            // user - eventmember - event 
            var eventParticipations = _context.Participant.Where(e => e.FkUser == GetCurrentUser().UserId);

            //ViewData["joinedEvents"];
            ViewData["joinedEvents"] = from first in sportbookDatabaseContext
                                     join second in eventParticipations
                                     on first.EventId equals second.FkEvent
                                     select first;

            ViewData["myEvents"] = sportbookDatabaseContext.Where(e => e.FkOwner == GetCurrentUser().UserId);

            //var a = from filteredParticipant in (from participants in _context.Participant
            //                                     where participants.FkUser == GetCurrentUser().UserId
            //                                     select participants)
            //        join events in sportbookDatabaseContext on filteredParticipant.FkEvent equals events.EventId
            //        select events;

            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            ViewData["CurrentUser"] = currentUser;
            return View(await sportbookDatabaseContext.ToListAsync());
        }
        [Route("[controller]/[action]")]
        public IActionResult SportsEvents()
        {
            var sportbookDatabaseContext = _context.Location.Include(t => t.FkGameTypeNavigation).Where(e => e.FkGameTypeNavigation.IsOnline == false);
            List<LocationData> locations = new List<LocationData>();
            foreach (var item in sportbookDatabaseContext)
            {
                locations.Add(new LocationData(item.Longitude, item.Latitude, item.Address, item.FkGameTypeNavigation.Name, item.LocationId, item.FkGameType));
            }
            ViewData["Locations"] = locations;
            return View();
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SportsEvents([Bind("Title,MaxParticipantAmt,StartTime,EndTime,IsPrivate,IsTeamEvent,FkLocation,FkGameType")] Event @event)
        {
            var user = GetCurrentUser();
            @event.FkOwner = user.UserId;
            if (ModelState.IsValid)
            {
                _context.Add(@event);
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
            return View(@event);
        }

        [Route("[controller]/[action]")]
        public IActionResult Teams()
        {
            return View();
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
            if (_event.IsPrivate && _event.FkOwner != user.UserId)
                return _context.EventInvitation.Any(e => (e.FkEvent == id) && (user.UserId == e.FkUser));
            else return true;
        }

        #region Event crud w.o create
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
        public async Task<IActionResult> Edit(int id, [Bind("Title,MaxParticipantAmt,StartTime,EndTime,IsPrivate,IsTeamEvent,EventId,FkOwner,FkLocation,FkGameType")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }
            if (!IsEventOwner(id))
            {
                return Forbid();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", @event.FkGameType);
            ViewData["FkLocation"] = new SelectList(_context.Location, "LocationId", "Address", @event.FkLocation);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", @event.FkOwner);
            return View(@event);
        }

        // GET: Events/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }

        public async Task<IActionResult> Details(int? id)
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
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }
        #endregion
    }
}