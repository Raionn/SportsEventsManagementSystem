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
    public class ParticipantsController : Controller
    {
        private readonly SportbookDatabaseContext _context;

        public ParticipantsController(SportbookDatabaseContext context)
        {
            _context = context;
        }

        // GET: Participants
        public async Task<IActionResult> Index()
        {
            var sportbookDatabaseContext = _context.Participant.Include(p => p.FkEventNavigation).Include(p => p.FkTeamNavigation).Include(p => p.FkUserNavigation);
            return View(await sportbookDatabaseContext.ToListAsync());
        }

        // GET: Participants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Participant
                .Include(p => p.FkEventNavigation)
                .Include(p => p.FkTeamNavigation)
                .Include(p => p.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // GET: Participants/Create
        public IActionResult Create()
        {
            ViewData["FkEvent"] = new SelectList(_context.Event, "EventId", "EventId");
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId");
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParticipantId,FkUser,FkEvent,FkTeam")] Participant participant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkEvent"] = new SelectList(_context.Event, "EventId", "EventId", participant.FkEvent);
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", participant.FkTeam);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", participant.FkUser);
            return View(participant);
        }

        // GET: Participants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Participant.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }
            ViewData["FkEvent"] = new SelectList(_context.Event, "EventId", "EventId", participant.FkEvent);
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", participant.FkTeam);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", participant.FkUser);
            return View(participant);
        }

        // POST: Participants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParticipantId,FkUser,FkEvent,FkTeam")] Participant participant)
        {
            if (id != participant.ParticipantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantExists(participant.ParticipantId))
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
            ViewData["FkEvent"] = new SelectList(_context.Event, "EventId", "EventId", participant.FkEvent);
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", participant.FkTeam);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", participant.FkUser);
            return View(participant);
        }

        // GET: Participants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Participant
                .Include(p => p.FkEventNavigation)
                .Include(p => p.FkTeamNavigation)
                .Include(p => p.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var participant = await _context.Participant.FindAsync(id);
            _context.Participant.Remove(participant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantExists(int id)
        {
            return _context.Participant.Any(e => e.ParticipantId == id);
        }
    }
}
