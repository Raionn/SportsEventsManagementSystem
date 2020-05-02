using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportBook.Helpers;
using SportBook.Models;

namespace SportBook.Controllers
{
    public class AdminController : Controller
    {
        #region Private members
        private readonly SportbookDatabaseContext _context;
        private ChallongeService chall;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public AdminController(SportbookDatabaseContext context, IHttpClientFactory clientFactory, IConfiguration config)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = config;
            chall = new ChallongeService(_clientFactory, _configuration);
        }
        #endregion

        #region General Methods
        public IActionResult Index()
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

        private async Task<int> GenerateRandomInt()
        {
            bool isNew = true;
            Random rnd = new Random();
            int random = 0;
            while (isNew)
            {
                random = rnd.Next();
                isNew = await _context.Tournament.AnyAsync(x => x.TournamentId == random);
            }      
            return random;
        }
        #endregion

        #region Cities
        public async Task<IActionResult> Cities(string name)
        {
            var cities = from m in _context.City
                         select m;

            if (!String.IsNullOrEmpty(name))
            {
                cities = cities.Where(s => s.Name.Contains(name));
            }

            return View("~/Views/Admin/Cities/Index.cshtml", await cities.ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> CitiesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Cities/Details.cshtml",city);
        }

        // GET: Cities/Create
        public IActionResult CitiesCreate()
        {
            return View("~/Views/Admin/Cities/Create.cshtml");
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CitiesCreate([Bind("Name,CityId")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Cities));
            }
            return View("~/Views/Admin/Cities/Create.cshtml",city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> CitiesEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/Cities/Edit.cshtml",city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CitiesEdit(int id, [Bind("Name,CityId")] City city)
        {
            if (id != city.CityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Cities));
            }
            return View("~/Views/Admin/Cities/Edit.cshtml",city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> CitiesDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Cities/Delete.cshtml",city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("CitiesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CitiesDeleteConfirmed(int id)
        {
            var city = await _context.City.FindAsync(id);
            _context.City.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Cities));
        }

        private bool CityExists(int id)
        {
            return _context.City.Any(e => e.CityId == id);
        }
        #endregion

        #region Locations
        public async Task<IActionResult> Locations(string address, string city, string gametype)
        {

            var sportbookDatabaseContext = _context.Location.Include(l => l.FkCityNavigation).Include(l => l.FkGameTypeNavigation);
            if (!String.IsNullOrEmpty(address))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.Address.Contains(address)).Include(l => l.FkCityNavigation).Include(l => l.FkGameTypeNavigation);
            }
            if (!String.IsNullOrEmpty(city))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.FkCityNavigation.Name.Contains(city)).Include(l => l.FkCityNavigation).Include(l => l.FkGameTypeNavigation);
            }
            if (!String.IsNullOrEmpty(gametype))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.FkGameTypeNavigation.Name.Contains(gametype)).Include(l => l.FkCityNavigation).Include(l => l.FkGameTypeNavigation);
            }
            return View("~/Views/Admin/Locations/Index.cshtml",await sportbookDatabaseContext.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> LocationsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .Include(l => l.FkCityNavigation)
                .Include(l => l.FkGameTypeNavigation)
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Locations/Details.cshtml",location);
        }

        // GET: Locations/Create
        public IActionResult LocationsCreate()
        {
            ViewData["FkCity"] = new SelectList(_context.City, "CityId", "Name");
            ViewData["FkGameType"] = new SelectList(GetOfflineGames(), "GameTypeId", "Name");
            return View("~/Views/Admin/Locations/Create.cshtml");
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LocationsCreate([Bind("Latitude,Longitude,Address,LocationId,FkCity,FkGameType")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Locations));
            }
            ViewData["FkCity"] = new SelectList(_context.City, "CityId", "Name", location.FkCity);
            ViewData["FkGameType"] = new SelectList(GetOfflineGames(), "GameTypeId", "Name", location.FkGameType);
            return View("~/Views/Admin/Locations/Create.cshtml",location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> LocationsEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["FkCity"] = new SelectList(_context.City, "CityId", "Name", location.FkCity);
            ViewData["FkGameType"] = new SelectList(GetOfflineGames(), "GameTypeId", "Name", location.FkGameType);
            return View("~/Views/Admin/Locations/Edit.cshtml",location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LocationsEdit(int id, [Bind("Latitude,Longitude,Address,LocationId,FkCity,FkGameType")] Location location)
        {
            if (id != location.LocationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.LocationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Locations));
            }
            ViewData["FkCity"] = new SelectList(_context.City, "CityId", "Name", location.FkCity);
            ViewData["FkGameType"] = new SelectList(GetOfflineGames(), "GameTypeId", "Name", location.FkGameType);
            return View("~/Views/Admin/Locations/Edit.cshtml",location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> LocationsDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .Include(l => l.FkCityNavigation)
                .Include(l => l.FkGameTypeNavigation)
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Locations/Delete.cshtml",location);
        }

        private IQueryable<GameType> GetOfflineGames()
        {
            var offlineGames = _context.GameType.Where(x => x.IsOnline == false);
            return offlineGames;
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("LocationsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LocationsDeleteConfirmed(int id)
        {
            var location = await _context.Location.FindAsync(id);
            _context.Location.Remove(location);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Locations));
        }

        private bool LocationExists(int id)
        {
            return _context.Location.Any(e => e.LocationId == id);
        }
        #endregion

        #region Game Types
        public async Task<IActionResult> GameTypes(string name)
        {
            var gameTypes = from m in _context.GameType
                         select m;

            if (!String.IsNullOrEmpty(name))
            {
                gameTypes = gameTypes.Where(s => s.Name.Contains(name));
            }
            return View("~/Views/Admin/GameTypes/Index.cshtml",await gameTypes.ToListAsync());
        }

        // GET: GameTypes/Details/5
        public async Task<IActionResult> GameTypesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameType = await _context.GameType
                .FirstOrDefaultAsync(m => m.GameTypeId == id);
            if (gameType == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/GameTypes/Details.cshtml",gameType);
        }

        // GET: GameTypes/Create
        public IActionResult GameTypesCreate()
        {
            return View("~/Views/Admin/GameTypes/Create.cshtml");
        }

        // POST: GameTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GameTypesCreate([Bind("Name,IsOnline,GameTypeId")] GameType gameType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GameTypes));
            }
            return View("~/Views/Admin/GameTypes/Create.cshtml",gameType);
        }

        // GET: GameTypes/Edit/5
        public async Task<IActionResult> GameTypesEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameType = await _context.GameType.FindAsync(id);
            if (gameType == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/GameTypes/Edit.cshtml",gameType);
        }

        // POST: GameTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GameTypesEdit(int id, [Bind("Name,IsOnline,GameTypeId")] GameType gameType)
        {
            if (id != gameType.GameTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameTypeExists(gameType.GameTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GameTypes));
            }
            return View("~/Views/Admin/GameTypes/Edit.cshtml",gameType);
        }

        // GET: GameTypes/Delete/5
        public async Task<IActionResult> GameTypesDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameType = await _context.GameType
                .FirstOrDefaultAsync(m => m.GameTypeId == id);
            if (gameType == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/GameTypes/Delete.cshtml",gameType);
        }

        // POST: GameTypes/Delete/5
        [HttpPost, ActionName("GameTypesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameType = await _context.GameType.FindAsync(id);
            _context.GameType.Remove(gameType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GameTypes));
        }

        private bool GameTypeExists(int id)
        {
            return _context.GameType.Any(e => e.GameTypeId == id);
        }

        #endregion

        #region Tournaments

        private IQueryable<GameType> GetOnlineGames()
        {
            var offlineGames = _context.GameType.Where(x => x.IsOnline == true);
            return offlineGames;
        }

        public async Task<IActionResult> Tournaments(string name, string gametype)
        {
            var sportbookDatabaseContext = _context.Tournament.Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation);
            if (!String.IsNullOrEmpty(name))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.Name.Contains(name)).Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation);
            }
            if (!String.IsNullOrEmpty(gametype))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.FkGameTypeNavigation.Name.Contains(gametype)).Include(t => t.FkGameTypeNavigation).Include(t => t.FkOwnerNavigation);
            }

            return View("~/Views/Admin/Tournaments/Index.cshtml",await sportbookDatabaseContext.ToListAsync());
        }

        // GET: Tournaments/Details/5
        public async Task<IActionResult> TournamentsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournament
                .Include(t => t.FkGameTypeNavigation)
                .Include(t => t.FkOwnerNavigation)
                .FirstOrDefaultAsync(m => m.TournamentId == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Tournaments/Details.cshtml",tournament);
        }

        // GET: Tournaments/Create
        public IActionResult TournamentsCreate()
        {
            ViewData["FkGameType"] = new SelectList(GetOnlineGames(), "GameTypeId", "Name");
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", GetCurrentUser());
            return View("~/Views/Admin/Tournaments/Create.cshtml");
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TournamentsCreate([Bind("Name,Description,MaxParticipantAmt,StartTime,TournamentId,FkGameType,FkOwner")] Tournament tournament)
        {
            var user = GetCurrentUser();
            tournament.FkOwner = user.UserId;
            tournament.FkOwnerNavigation = user;
            if (ModelState.IsValid)
            {
                var urlId = await GenerateRandomInt();
                tournament = await chall.OnPostTournament(tournament, urlId);
                _context.Add(tournament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Tournaments));
            }
            ViewData["FkGameType"] = new SelectList(GetOnlineGames(), "GameTypeId", "Name", tournament.FkGameType);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", tournament.FkOwner);
            return View("~/Views/Admin/Tournaments/Create.cshtml",tournament);
        }

        // GET: Tournaments/Edit/5
        public async Task<IActionResult> TournamentsEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournament.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }
            ViewData["FkGameType"] = new SelectList(GetOnlineGames(), "GameTypeId", "Name", tournament.FkGameType);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", tournament.FkOwner);
            return View("~/Views/Admin/Tournaments/Edit.cshtml",tournament);
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TournamentsEdit(int id, [Bind("Name,Description,MaxParticipantAmt,StartTime,TournamentId,FkGameType,FkOwner,ExternalID,TournamentUrl")] Tournament tournament)
        {
            if (id != tournament.TournamentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await chall.OnPutTournament(tournament);
                    _context.Update(tournament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.TournamentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Tournaments));
            }
            ViewData["FkGameType"] = new SelectList(GetOnlineGames(), "GameTypeId", "Name", tournament.FkGameType);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", tournament.FkOwner);
            return View("~/Views/Admin/Tournaments/Edit.cshtml",tournament);
        }

        // GET: Tournaments/Delete/5
        public async Task<IActionResult> TournamentsDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournament
                .Include(t => t.FkGameTypeNavigation)
                .Include(t => t.FkOwnerNavigation)
                .FirstOrDefaultAsync(m => m.TournamentId == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Tournaments/Delete.cshtml",tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("TournamentsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TournamentsDeleteConfirmed(int id)
        {
            var tournament = await _context.Tournament.FindAsync(id);
            await chall.OnDeleteTournament(tournament);
            _context.Tournament.Remove(tournament);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Tournaments));
        }

        private bool TournamentExists(int id)
        {
            return _context.Tournament.Any(e => e.TournamentId == id);
        }
        #endregion

        #region Events
        public async Task<IActionResult> Events(string title,string gametype, string address)
        {
            var sportbookDatabaseContext = _context.Event.Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation);
            if (!String.IsNullOrEmpty(title))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.Title.Contains(title)).Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation);
            }
            if (!String.IsNullOrEmpty(gametype))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.FkGameTypeNavigation.Name.Contains(gametype)).Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation);
            }
            if (!String.IsNullOrEmpty(address))
            {
                sportbookDatabaseContext = sportbookDatabaseContext.Where(s => s.FkLocationNavigation.Address.Contains(address)).Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation);
            }
            return View("~/Views/Admin/Events/Index.cshtml",await sportbookDatabaseContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> EventsDetails(int? id)
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

            return View("~/Views/Admin/Events/Details.cshtml",@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> EventsEdit(int? id)
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
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", @event.FkGameType);
            ViewData["FkLocation"] = new SelectList(_context.Location, "LocationId", "Address", @event.FkLocation);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", @event.FkOwner);
            return View("~/Views/Admin/Events/Edit.cshtml",@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EventsEdit(int id, [Bind("Title,MaxParticipantAmt,StartTime,EndTime,IsPrivate,IsTeamEvent,EventId,FkOwner,FkLocation,FkGameType")] Event @event)
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
                return RedirectToAction(nameof(Events));
            }
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", @event.FkGameType);
            ViewData["FkLocation"] = new SelectList(_context.Location, "LocationId", "Address", @event.FkLocation);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", @event.FkOwner);
            return View("~/Views/Admin/Events/Edit.cshtml",@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> EventsDelete(int? id)
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

            return View("~/Views/Admin/Events/Delete.cshtml",@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("EventsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EventsDeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Events));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
        #endregion

    }
}
