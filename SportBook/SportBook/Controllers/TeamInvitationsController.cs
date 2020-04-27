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
    public class TeamInvitationsController : Controller
    {
        private readonly SportbookDatabaseContext _context;

        public TeamInvitationsController(SportbookDatabaseContext context)
        {
            _context = context;
        }

        // GET: TeamInvitations
        public async Task<IActionResult> Index()
        {
            var sportbookDatabaseContext = _context.TeamInvitation.Include(t => t.FkTeamNavigation).Include(t => t.FkUserNavigation);
            return View(await sportbookDatabaseContext.ToListAsync());
        }

        // GET: TeamInvitations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamInvitation = await _context.TeamInvitation
                .Include(t => t.FkTeamNavigation)
                .Include(t => t.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.TeamInvitationId == id);
            if (teamInvitation == null)
            {
                return NotFound();
            }

            return View(teamInvitation);
        }

        // GET: TeamInvitations/Create
        public IActionResult Create()
        {
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId");
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: TeamInvitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,IsAccepted,TeamInvitationId,FkUser,FkTeam")] TeamInvitation teamInvitation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamInvitation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", teamInvitation.FkTeam);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", teamInvitation.FkUser);
            return View(teamInvitation);
        }

        // GET: TeamInvitations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamInvitation = await _context.TeamInvitation.FindAsync(id);
            if (teamInvitation == null)
            {
                return NotFound();
            }
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", teamInvitation.FkTeam);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", teamInvitation.FkUser);
            return View(teamInvitation);
        }

        // POST: TeamInvitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Text,IsAccepted,TeamInvitationId,FkUser,FkTeam")] TeamInvitation teamInvitation)
        {
            if (id != teamInvitation.TeamInvitationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamInvitation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamInvitationExists(teamInvitation.TeamInvitationId))
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
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", teamInvitation.FkTeam);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", teamInvitation.FkUser);
            return View(teamInvitation);
        }

        // GET: TeamInvitations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamInvitation = await _context.TeamInvitation
                .Include(t => t.FkTeamNavigation)
                .Include(t => t.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.TeamInvitationId == id);
            if (teamInvitation == null)
            {
                return NotFound();
            }

            return View(teamInvitation);
        }

        // POST: TeamInvitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamInvitation = await _context.TeamInvitation.FindAsync(id);
            _context.TeamInvitation.Remove(teamInvitation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamInvitationExists(int id)
        {
            return _context.TeamInvitation.Any(e => e.TeamInvitationId == id);
        }
    }
}
