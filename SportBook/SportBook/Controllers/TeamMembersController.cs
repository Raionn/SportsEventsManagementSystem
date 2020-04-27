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
    public class TeamMembersController : Controller
    {
        private readonly SportbookDatabaseContext _context;

        public TeamMembersController(SportbookDatabaseContext context)
        {
            _context = context;
        }

        // GET: TeamMembers
        public async Task<IActionResult> Index()
        {
            var sportbookDatabaseContext = _context.TeamMember.Include(t => t.FkTeamNavigation).Include(t => t.FkUserNavigation);
            return View(await sportbookDatabaseContext.ToListAsync());
        }

        // GET: TeamMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMember
                .Include(t => t.FkTeamNavigation)
                .Include(t => t.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.TeamMemberId == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // GET: TeamMembers/Create
        public IActionResult Create()
        {
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId");
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: TeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamMemberId,FkUser,FkTeam")] TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", teamMember.FkTeam);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", teamMember.FkUser);
            return View(teamMember);
        }

        // GET: TeamMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMember.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", teamMember.FkTeam);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", teamMember.FkUser);
            return View(teamMember);
        }

        // POST: TeamMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamMemberId,FkUser,FkTeam")] TeamMember teamMember)
        {
            if (id != teamMember.TeamMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamMemberExists(teamMember.TeamMemberId))
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
            ViewData["FkTeam"] = new SelectList(_context.Team, "TeamId", "TeamId", teamMember.FkTeam);
            ViewData["FkUser"] = new SelectList(_context.User, "UserId", "UserId", teamMember.FkUser);
            return View(teamMember);
        }

        // GET: TeamMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMember
                .Include(t => t.FkTeamNavigation)
                .Include(t => t.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.TeamMemberId == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMember = await _context.TeamMember.FindAsync(id);
            _context.TeamMember.Remove(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamMemberExists(int id)
        {
            return _context.TeamMember.Any(e => e.TeamMemberId == id);
        }
    }
}
