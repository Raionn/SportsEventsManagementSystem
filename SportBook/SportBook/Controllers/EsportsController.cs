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
            var userId = GetCurrentUser().UserId;
            var events = _context.Event.Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation).Where(e => e.FkGameTypeNavigation.IsOnline);
            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            ViewData["CurrentUser"] = currentUser;
            var participants = new Dictionary<int, int>();
            var eventList = events.ToList();
            foreach (var item in eventList)
            {
                participants.Add(item.EventId, _context.Participant.Where(x => x.FkEvent == item.EventId).Count());
            }
            var eventParticipations = _context.Participant.Where(e => e.FkUser == userId);

            var myEvents = events.Where(e => e.FkOwner == userId);
            var teamEvents = events.Where(e => e.IsTeamEvent == true);
            var joinedEvents = from first in events
                               join second in eventParticipations
                               on first.EventId equals second.FkEvent
                               select first; ;
            ViewData["myEvents"] = myEvents.OrderBy(x => x.StartTime);
            ViewData["teamEvents"] = teamEvents.OrderBy(x => x.StartTime).Except(myEvents).Except(joinedEvents);
            ViewData["joinedEvents"] = joinedEvents.OrderBy(x => x.StartTime).Except(myEvents);
            ViewData["Participants"] = participants;
            events = events.Except(myEvents).Except(teamEvents).Except(joinedEvents).OrderBy(x => x.StartTime);
            return View(events);
        }
        public IActionResult CreateEvent()
        {
            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            var sportsGameTypes = _context.GameType.Where(x => x.IsOnline == true);

            ViewData["FkGameType"] = new SelectList(sportsGameTypes, "GameTypeId", "Name");
            ViewData["FkOwner"] = currentUser;
            var sportbookDatabaseContext = _context.Team.Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation);
            var teamsParticipations = _context.TeamMember.Where(t => t.FkUser == currentUser.UserId);
            var data = from first in sportbookDatabaseContext
                       join second in teamsParticipations
                               on first.TeamId equals second.FkTeam
                       select first;
            ViewData["Teams"] = data;
            ViewData["SecondFailed"] = false;
            return View();
        }
        [HttpGet("{userId}")]
        public IActionResult EventMemberVC(int eventId, int userId)
        {
            return ViewComponent("EventMemberList", new { eventId, userId });
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

        // POST: Esports/CreateEvent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent([Bind("Title,MaxParticipantAmt,StartTime,EndTime,IsPrivate,IsTeamEvent,FkOwner,FkGameType")] Event @event)
        {
            var user = GetCurrentUser();
            int teamId = 0;
            if (@event.IsTeamEvent)
            {
                var team = _context.Team.FirstOrDefault(x => x.TeamId == @event.FkOwner);
                teamId = team.TeamId;
                @event.FkGameType = team.FkGameType;
                @event.FkOwner = user.UserId;
            }
            if (ModelState.IsValid)
            {

                _context.Add(@event);
                await _context.SaveChangesAsync();
                if (!@event.IsTeamEvent)
                {
                    var lastEvent = _context.Event.OrderByDescending(x => x.EventId).FirstOrDefault();
                    _context.Participant.Add(new Models.Participant() { FkEvent = lastEvent.EventId, FkUser = @event.FkOwner });
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var lastEvent = _context.Event.OrderByDescending(x => x.EventId).FirstOrDefault();
                    _context.Participant.Add(new Models.Participant() { FkEvent = lastEvent.EventId, FkUser = user.UserId, FkTeam = teamId });
                    await _context.SaveChangesAsync();
                }


                return RedirectToAction(nameof(Esports));
            }
            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            var sportsGameTypes = _context.GameType.Where(x => x.IsOnline == true);

            ViewData["FkGameType"] = new SelectList(sportsGameTypes, "GameTypeId", "Name");
            ViewData["FkOwner"] = currentUser;
            var sportbookDatabaseContext = _context.Team.Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation);
            var teamsParticipations = _context.TeamMember.Where(t => t.FkUser == currentUser.UserId);
            var data = from first in sportbookDatabaseContext
                       join second in teamsParticipations
                               on first.TeamId equals second.FkTeam
                       select first;
            ViewData["Teams"] = data;
            if (!@event.IsTeamEvent)
                ViewData["SecondFailed"] = false;
            else
            {
                ViewData["SecondFailed"] = true;
                ViewData["teamId"] = teamId;
            }
            return View(@event);
        }
        public async Task<IActionResult> ViewEvent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!IsInvited(id))
            {
                return Forbid();
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
            ViewData["EventInvites"] = _context.EventInvitation.Where(x => x.FkEvent == id);
            ViewData["isFailed"] = false;
            var eventMembers = await _context.Participant.Include(x => x.FkUserNavigation).Include(x => x.FkEventNavigation).Where(x => x.FkEvent == id).ToListAsync();
            EventData eventData = new EventData(@event, eventMembers, new Models.Participant());

            return View(@eventData);
        }

        public async Task<IActionResult> TeamEvent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!IsInvited(id))
            {
                return Forbid();
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
            var firstParticipant = _context.Participant.Where(x => x.FkEvent == id);
            var participantCount = firstParticipant.Count();
            var team = _context.Team.Where(x => x.TeamId == firstParticipant.FirstOrDefault().FkTeam).FirstOrDefault();
            var teamMembers = _context.TeamMember.Where(x => x.FkTeam == team.TeamId).Include(x => x.FkUserNavigation);
            if (participantCount > 1)
            {
                var otherParticipant = firstParticipant.Where(x => x.ParticipantId != firstParticipant.FirstOrDefault().ParticipantId).FirstOrDefault();
                var enemyTeam = _context.Team.Where(x => x.TeamId == otherParticipant.FkTeam).FirstOrDefault();
                var enemyMembers = _context.TeamMember.Where(x => x.FkTeam == enemyTeam.TeamId);
                ViewData["enemyMembers"] = enemyMembers;
                ViewData["enemyTeam"] = enemyTeam;
            }
            ViewData["pCount"] = participantCount;
            ViewData["teamMembers"] = teamMembers;
            ViewData["CurrentUser"] = currentUser;
            ViewData["eventTeam"] = team;
            ViewData["isFailed"] = false;
            var sportbookDatabaseContext = _context.Team.Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation);
            var teamsParticipations = _context.TeamMember.Where(t => t.FkUser == currentUser.UserId);
            var data = from first in sportbookDatabaseContext
                       join second in teamsParticipations
                               on first.TeamId equals second.FkTeam
                       select first;
            ViewData["Teams"] = data;
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
        public IActionResult ChatroomVC(string chatGroup)
        {
            return ViewComponent("Chatroom", chatGroup);
        }
        public async Task<IActionResult> Tournament(int id)
        {
            var tournaments = _context.Tournament.Include(t => t.TournamentMember);
            var tournament = await tournaments.Where(t => t.TournamentId == id).FirstOrDefaultAsync();
            var tournamentTeams = _context.TournamentMember.Where(x => x.FkTournament == id);
            var user = GetCurrentUser();
            var userTeams = _context.Team.Where(x => x.TeamMember.Any(y => y.FkUser == user.UserId) && x.FkGameType == tournament.FkGameType);
            var tournamentMember = new TournamentMember();
            if (tournamentTeams.Count() > 0)
            {
                var alreadyParticipant = from first in tournamentTeams
                                         join second in userTeams
                                                 on first.FkTeam equals second.TeamId
                                         select first;
                tournamentMember = alreadyParticipant.FirstOrDefault();
                
            }
            if (tournamentMember != null)
            {
                ViewData["myTeam"] = _context.Team.FirstOrDefault(x => x.TeamId == tournamentMember.FkTeam);
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
            await chall.OnDeleteParticipant(deleteMember, tournament.ExternalID);


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,MaxParticipantAmt,StartTime,EndTime,IsPrivate,IsTeamEvent,EventId,FkOwner,FkGameType")] Event @event)
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
                if (@event.IsTeamEvent)
                {
                    return RedirectToAction(nameof(TeamEvent), new { id = @event.EventId });
                }
                else
                {
                    return RedirectToAction(nameof(ViewEvent), new { id = @event.EventId });
                }

            }
            var tempEvent = await _context.Event.Include(e => e.FkGameTypeNavigation).FirstOrDefaultAsync(m => m.EventId == id);
            @event.FkGameTypeNavigation = tempEvent.FkGameTypeNavigation;
            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            ViewData["CurrentUser"] = currentUser;
            ViewData["EventInvites"] = _context.EventInvitation.Where(x => x.FkEvent == id);
            ViewData["isFailed"] = true;
            var eventMembers = await _context.Participant.Include(x => x.FkUserNavigation).Include(x => x.FkEventNavigation).Where(x => x.FkEvent == id).ToListAsync();
            EventData eventData = new EventData(@event, eventMembers, new Models.Participant());
            if (@event.IsTeamEvent)
            {
                var firstParticipant = _context.Participant.Where(x => x.FkEvent == id);
                var participantCount = firstParticipant.Count();
                var team = _context.Team.Where(x => x.TeamId == firstParticipant.FirstOrDefault().FkTeam).FirstOrDefault();
                var teamMembers = _context.TeamMember.Where(x => x.FkTeam == team.TeamId).Include(x => x.FkUserNavigation);
                if (participantCount > 1)
                {
                    var otherParticipant = firstParticipant.Where(x => x.ParticipantId != firstParticipant.FirstOrDefault().ParticipantId).FirstOrDefault();
                    var enemyTeam = _context.Team.Where(x => x.TeamId == otherParticipant.FkTeam).FirstOrDefault();
                    var enemyMembers = _context.TeamMember.Where(x => x.FkTeam == enemyTeam.TeamId);
                    ViewData["enemyMembers"] = enemyMembers;
                    ViewData["enemyTeam"] = enemyTeam;
                }
                ViewData["pCount"] = participantCount;
                ViewData["teamMembers"] = teamMembers;
                ViewData["eventTeam"] = team;
                var sportbookDatabaseContext = _context.Team.Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation);
                var teamsParticipations = _context.TeamMember.Where(t => t.FkUser == currentUser.UserId);
                var data = from first in sportbookDatabaseContext
                           join second in teamsParticipations
                                   on first.TeamId equals second.FkTeam
                           select first;
                ViewData["Teams"] = data;
                return View("TeamEvent", eventData);
            }
            else 
            {
                return View("ViewEvent", eventData);
            }

        }
        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventParticipants = _context.Participant.Where(x => x.FkEvent == id);
            var eventInvitations = _context.EventInvitation.Where(x => x.FkEvent == id);
            var @event = await _context.Event.FindAsync(id);
            _context.Participant.RemoveRange(eventParticipants);
            _context.EventInvitation.RemoveRange(eventInvitations);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Esports));
        }
        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
        [HttpGet]
        public IActionResult TeamEventFormVC(int teamId)
        {
            return ViewComponent("TeamEventForm", new { teamId });
        }

        [HttpGet("{userId}")]
        public IActionResult InvitableUserVC(int eventId, int userId)
        {
            return ViewComponent("InvitableUserList", new { eventId, userId });
        }

        [HttpGet("{teamId}")]
        public IActionResult TeamEventEnemyVC(int eventId, int teamId)
        {
            return ViewComponent("ScrimEnemyTeam", new { eventId, teamId });
        }
        
    }
}