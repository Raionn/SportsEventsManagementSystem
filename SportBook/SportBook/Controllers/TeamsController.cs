using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportBook.Helpers;
using SportBook.Models;

namespace SportBook.Controllers
{
    [Route("[controller]/[action]")]
    public class TeamsController : Controller
    {
        private readonly SportbookDatabaseContext _context;
        private ChallongeService chall;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public TeamsController(SportbookDatabaseContext context, IHttpClientFactory clientFactory, IConfiguration config)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = config;
            chall = new ChallongeService(_clientFactory, _configuration);
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
        // GET: Teams
        public IActionResult Index()
        {
            var sportbookDatabaseContext = _context.Team.Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation);
            var teamsParticipations = _context.TeamMember.Where(t => t.FkUser == GetCurrentUser().UserId);
            var data = from first in sportbookDatabaseContext
                                     join second in teamsParticipations
                                             on first.TeamId equals second.FkTeam
                                     select first;
            ViewData["Teams"] = data;
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name");
            return View(new Team());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .Include(t => t.FkGameTypeNavigation)
                .Include(t => t.FkOwnerNavigation)
                .FirstOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            ViewData["Members"] = _context.TeamMember.Where(x => x.FkTeam == id).Include(x => x.FkUserNavigation);
            ViewData["CurrentUser"] = GetCurrentUser();
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", team.FkGameType);
            ViewData["TeamInvites"] = _context.TeamInvitation.Where(x => x.FkTeam == id);
            ViewData["isFailed"] = false;
            return View(team);
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,TeamId,FkGameType")] Team team)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = GetCurrentUser().UserId;
                team.FkOwner = currentUserId;
                _context.Add(team);
                await _context.SaveChangesAsync();
                var teamMember = new TeamMember() { FkTeam = team.TeamId, FkUser = currentUserId };
                _context.Add(teamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name");
            var sportbookDatabaseContext = _context.Team.Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation);
            var teamsParticipations = _context.TeamMember.Where(t => t.FkUser == GetCurrentUser().UserId);
            var data = from first in sportbookDatabaseContext
                       join second in teamsParticipations
                               on first.TeamId equals second.FkTeam
                       select first;
            ViewData["Teams"] = data;
            return View("Index",team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,TeamId,FkOwner,FkGameType")] Team team)
        {
            if (id != team.TeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TeamId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = team.TeamId});
            }

            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", team.FkGameType);
            ViewData["Members"] = _context.TeamMember.Where(x => x.FkTeam == id).Include(x => x.FkUserNavigation);
            ViewData["CurrentUser"] = GetCurrentUser();
            ViewData["TeamInvites"] = _context.TeamInvitation.Where(x => x.FkTeam == id);
            ViewData["isFailed"] = true;
            return View("Details", team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamInvites = _context.TeamInvitation.Where(x => x.FkTeam == id);
            var teamMembers = _context.TeamMember.Where(x => x.FkTeam == id);
            var teamParticipations = _context.Participant.Where(x => x.FkTeam == id);
            var tournamentParticipations = _context.TournamentMember.Where(x => x.FkTeam == id).Include(x => x.FkTournamentNavigation);
            foreach (var item in tournamentParticipations)
            {
                await chall.OnDeleteParticipant(item, item.FkTournamentNavigation.ExternalID);
            }
            var team = await _context.Team.FindAsync(id);
            _context.RemoveRange(teamInvites);
            _context.RemoveRange(teamMembers);
            _context.RemoveRange(teamParticipations);
            _context.RemoveRange(tournamentParticipations);
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.TeamId == id);
        }

        [HttpGet("{userId}")]
        public IActionResult TeamMemberVC(int teamId, int userId)
        {
            return ViewComponent("TeamMemberList", new { teamId, userId });
        }

        [HttpGet("{teamId}")]
        public IActionResult TeamInvitationListVC(int userId, int teamId)
        {
            return ViewComponent("InviteTeamMemberList", new { teamId, userId });
        }
    }
}
