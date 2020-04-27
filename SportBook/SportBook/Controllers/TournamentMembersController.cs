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
    public class TournamentMembersController : Controller
    {
        private readonly SportbookDatabaseContext _context;

        public TournamentMembersController(SportbookDatabaseContext context)
        {
            _context = context;
        }

        // GET: TournamentMembers
        public async Task<IActionResult> Index()
        {
            var sportbookDatabaseContext = _context.TournamentMember.Include(t => t.FkTeamNavigation).Include(t => t.FkTournamentNavigation);
            return View(await sportbookDatabaseContext.ToListAsync());
        }

        // GET: TournamentMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournamentMember = await _context.TournamentMember
                .Include(t => t.FkTeamNavigation)
                .Include(t => t.FkTournamentNavigation)
                .FirstOrDefaultAsync(m => m.TournamentMemberId == id);
            if (tournamentMember == null)
            {
                return NotFound();
            }

            return View(tournamentMember);
        }

        // GET: TournamentMembers/Create
        public IActionResult Create()
        {
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId");
            ViewData["FkTournament"] = new SelectList(_context.Tournament, "TournamentId", "TournamentId");
            return View();
        }

        // POST: TournamentMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TournamentMemberId,FkTournament,FkTeam")] TournamentMember tournamentMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tournamentMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", tournamentMember.FkTeam);
            ViewData["FkTournament"] = new SelectList(_context.Tournament, "TournamentId", "TournamentId", tournamentMember.FkTournament);
            return View(tournamentMember);
        }

        // GET: TournamentMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournamentMember = await _context.TournamentMember.FindAsync(id);
            if (tournamentMember == null)
            {
                return NotFound();
            }
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", tournamentMember.FkTeam);
            ViewData["FkTournament"] = new SelectList(_context.Tournament, "TournamentId", "TournamentId", tournamentMember.FkTournament);
            return View(tournamentMember);
        }

        // POST: TournamentMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TournamentMemberId,FkTournament,FkTeam")] TournamentMember tournamentMember)
        {
            if (id != tournamentMember.TournamentMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournamentMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentMemberExists(tournamentMember.TournamentMemberId))
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
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", tournamentMember.FkTeam);
            ViewData["FkTournament"] = new SelectList(_context.Tournament, "TournamentId", "TournamentId", tournamentMember.FkTournament);
            return View(tournamentMember);
        }

        // GET: TournamentMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournamentMember = await _context.TournamentMember
                .Include(t => t.FkTeamNavigation)
                .Include(t => t.FkTournamentNavigation)
                .FirstOrDefaultAsync(m => m.TournamentMemberId == id);
            if (tournamentMember == null)
            {
                return NotFound();
            }

            return View(tournamentMember);
        }

        // POST: TournamentMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournamentMember = await _context.TournamentMember.FindAsync(id);
            _context.TournamentMember.Remove(tournamentMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TournamentMemberExists(int id)
        {
            return _context.TournamentMember.Any(e => e.TournamentMemberId == id);
        }
    }
}
