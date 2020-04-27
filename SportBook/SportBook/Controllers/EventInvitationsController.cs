using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;

namespace SportBook.Controllers
{
    public class EventInvitationsController : Controller
    {
        private readonly SportbookDatabaseContext _context;

        public EventInvitationsController(SportbookDatabaseContext context)
        {
            _context = context;
        }

        // GET: EventInvitations
        public async Task<IActionResult> Index()
        {
            var sportbookDatabaseContext = _context.EventInvitation.Include(e => e.FkEventNavigation).Include(e => e.FkUserNavigation);
            return View(await sportbookDatabaseContext.ToListAsync());
        }

        // GET: EventInvitations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInvitation = await _context.EventInvitation
                .Include(e => e.FkEventNavigation)
                .Include(e => e.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.EventInvitationId == id);
            if (eventInvitation == null)
            {
                return NotFound();
            }

            return View(eventInvitation);
        }

        // GET: EventInvitations/Create
        public IActionResult Create()
        {
            ViewData["FkEvent"] = new SelectList(_context.Event, "EventId", "EventId");
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: EventInvitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsAccepted,EventInvitationId,FkUser,FkEvent")] EventInvitation eventInvitation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventInvitation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkEvent"] = new SelectList(_context.Event, "EventId", "EventId", eventInvitation.FkEvent);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", eventInvitation.FkUser);
            return View(eventInvitation);
        }

        // GET: EventInvitations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInvitation = await _context.EventInvitation.FindAsync(id);
            if (eventInvitation == null)
            {
                return NotFound();
            }
            ViewData["FkEvent"] = new SelectList(_context.Event, "EventId", "EventId", eventInvitation.FkEvent);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", eventInvitation.FkUser);
            return View(eventInvitation);
        }

        // POST: EventInvitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IsAccepted,EventInvitationId,FkUser,FkEvent")] EventInvitation eventInvitation)
        {
            if (id != eventInvitation.EventInvitationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventInvitation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventInvitationExists(eventInvitation.EventInvitationId))
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
            ViewData["FkEvent"] = new SelectList(_context.Event, "EventId", "EventId", eventInvitation.FkEvent);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", eventInvitation.FkUser);
            return View(eventInvitation);
        }

        // GET: EventInvitations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInvitation = await _context.EventInvitation
                .Include(e => e.FkEventNavigation)
                .Include(e => e.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.EventInvitationId == id);
            if (eventInvitation == null)
            {
                return NotFound();
            }

            return View(eventInvitation);
        }

        // POST: EventInvitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventInvitation = await _context.EventInvitation.FindAsync(id);
            _context.EventInvitation.Remove(eventInvitation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventInvitationExists(int id)
        {
            return _context.EventInvitation.Any(e => e.EventInvitationId == id);
        }
    }
}
