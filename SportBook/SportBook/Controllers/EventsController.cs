using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;

namespace SportBook.Controllers
{
    [Authorize(Roles = "user, admin")]
    public class EventsController : Controller
    {
        private readonly SportbookDatabaseContext _context;

        public EventsController(SportbookDatabaseContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var sportbookDatabaseContext = _context.Event.Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation);
            return View(await sportbookDatabaseContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
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

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "GameTypeId");
            ViewData["FkLocation"] = new SelectList(_context.Location, "LocationId", "LocationId");
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,MaxParticipantAmt,StartTime,EndTime,IsPrivate,IsTeamEvent,EventId,FkOwner,FkLocation,FkGameType")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "GameTypeId", @event.FkGameType);
            ViewData["FkLocation"] = new SelectList(_context.Location, "LocationId", "LocationId", @event.FkLocation);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "UserId", @event.FkOwner);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "GameTypeId", @event.FkGameType);
            ViewData["FkLocation"] = new SelectList(_context.Location, "LocationId", "LocationId", @event.FkLocation);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "UserId", @event.FkOwner);
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
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "GameTypeId", @event.FkGameType);
            ViewData["FkLocation"] = new SelectList(_context.Location, "LocationId", "LocationId", @event.FkLocation);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "UserId", @event.FkOwner);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
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
    }
}
