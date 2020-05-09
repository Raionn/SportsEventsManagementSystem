using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
    //[Authorize(Roles = "user, admin")]
    [Route("[controller]/[action]")]
    public class EsportsController : Controller
    {
        private readonly SportbookDatabaseContext _context;
        private ChallongeService chall;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public EsportsController(SportbookDatabaseContext context, IHttpClientFactory clientFactory, IConfiguration config)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = config;
            chall = new ChallongeService(_clientFactory, _configuration);
        }
        public IActionResult Esports()
        {
            var events = _context.Event.Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation).Where(e => e.FkGameTypeNavigation.IsOnline);
            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            ViewData["CurrentUser"] = currentUser;
            return View(events);
        }
        public IActionResult CreateEvent()
        {
            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            var sportsGameTypes = _context.GameType.Where(x => x.IsOnline == true);

            ViewData["FkGameType"] = new SelectList(sportsGameTypes, "GameTypeId", "Name");
            ViewData["FkOwner"] = currentUser;
            return View();
        }
        [HttpGet("{userId}")]
        public IActionResult EventMemberVC(int eventId, int userId)
        {
            return ViewComponent("EventMemberList", new { eventId, userId });
        }

        // POST: Esports/CreateEvent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent([Bind("Title,MaxParticipantAmt,StartTime,EndTime,IsPrivate,IsTeamEvent,FkOwner,FkGameType")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Esports));
            }
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", @event.FkGameType);
            ViewData["FkOwner"] = @event.FkOwner;     //new SelectList(_context.User, "UserId", "Username", @event.FkOwner);
            return View(@event);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Task> ViewEvent([Bind("FkUser, FkEvent, FkTeam")] Models.Participant participant)
        {
            //Models.Participant p = new Models.Participant();
            //p.FkEvent = 1;
            //p.FkTeam = null;
            //p.FkUser = 10;
            //_context.Participant.Add(p);
            //await _context.SaveChangesAsync();
            return Task.CompletedTask;
        }
        public async Task<IActionResult> ViewEvent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.FkGameTypeNavigation)
                .Include(e => e.FkOwnerNavigation)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null)
            {
                return NotFound();
            }

            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            ViewData["CurrentUser"] = currentUser;

            var eventMembers = await _context.Participant.Include(x => x.FkUserNavigation).Include(x => x.FkEventNavigation).Where(x => x.FkEvent == id).ToListAsync();
            EventData eventData = new EventData(@event, eventMembers, new Models.Participant());

            return View(@eventData);
        }
        public async Task<IActionResult> Tournaments(string name, string gameType)
        {
            var sportbookDatabaseContext = _context.Tournament.Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation).Include(t => t.TournamentMember).OrderBy(t => t.StartTime);

            if (!String.IsNullOrEmpty(name))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.Name.Contains(name)).Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation).OrderBy(t => t.StartTime);
            }
            if (!String.IsNullOrEmpty(gameType))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.FkGameTypeNavigation.Name.Contains(gameType)).Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation).OrderBy(t => t.StartTime);
            }

            return View(await sportbookDatabaseContext.ToListAsync());
        }
        public async Task<IActionResult> Tournament(int id)
        {
            var tournaments = _context.Tournament.Include(t => t.TournamentMember);
            var tournament = await tournaments.Where(t => t.TournamentId == id).FirstOrDefaultAsync();
            var tournamentTeams = _context.TournamentMember.Where(x => x.FkTournament == id);
            var user = GetCurrentUser();
            var userTeams = _context.Team.Where(x => x.TeamMember.Any(y => y.FkUser == user.UserId));
            var tournamentMember = new TournamentMember();
            if (tournamentTeams.Count() > 0)
            {
                var alreadyParticipant = from first in tournamentTeams
                                   join second in userTeams
                                           on first.FkTeam equals second.TeamId
                                   select first;
                tournamentMember = alreadyParticipant.FirstOrDefault();
            }

            var teams = new SelectList(userTeams, "TeamId", "Name");

            TournamentData data = new TournamentData(tournament, teams, tournamentMember);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Task> Tournament([Bind("TournamentMemberId", "FkTeam", "FkTournament")] TournamentMember tournamentMember)
        {
            var tournament = await _context.Tournament.FindAsync(tournamentMember.FkTournament);
            var team = await _context.Team.FindAsync(tournamentMember.FkTeam);
            if (ModelState.IsValid)
            {    
                tournamentMember.ExternalID = await chall.OnPostParticipant(tournament.ExternalID, team.Name);
                _context.Add(tournamentMember);
                await _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Task> LeaveTournament([Bind("TournamentMemberId", "FkTeam", "FkTournament")] TournamentMember tournamentMember)
        {
            var tournament = _context.Tournament.Where(x => x.TournamentId == tournamentMember.FkTournament).FirstOrDefault();
            var deleteMember = _context.TournamentMember.FirstOrDefault(x => x.TournamentMemberId == tournamentMember.TournamentMemberId);
            await chall.OnDeleteParticipant(deleteMember,tournament.ExternalID);


            _context.TournamentMember.Remove(deleteMember);
            await _context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public IActionResult Teams()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
    }
}