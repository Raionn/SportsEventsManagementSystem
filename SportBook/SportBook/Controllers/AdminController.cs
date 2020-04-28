using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;

namespace SportBook.Controllers
{
    public class AdminController : Controller
    {
        private readonly SportbookDatabaseContext _context;

        public AdminController(SportbookDatabaseContext context)
        {
            _context = context;
        }

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
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name");
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
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", location.FkGameType);
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
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", location.FkGameType);
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
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", location.FkGameType);
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
        public async Task<IActionResult> GameTypesCreate([Bind("Name,GameTypeId")] GameType gameType)
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
        public async Task<IActionResult> GameTypesEdit(int id, [Bind("Name,GameTypeId")] GameType gameType)
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
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name");
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", GetCurrentUser());
            return View("~/Views/Admin/Tournaments/Create.cshtml");
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TournamentsCreate([Bind("Name,Description,MaxParticipantAmt,Start,TournamentId,FkGameType,FkOwner")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tournament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Tournaments));
            }
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", tournament.FkGameType);
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
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", tournament.FkGameType);
            ViewData["FkOwner"] = new SelectList(_context.User, "UserId", "Username", tournament.FkOwner);
            return View("~/Views/Admin/Tournaments/Edit.cshtml",tournament);
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TournamentsEdit(int id, [Bind("Name,Description,MaxParticipantAmt,Start,TournamentId,FkGameType,FkOwner")] Tournament tournament)
        {
            if (id != tournament.TournamentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["FkGameType"] = new SelectList(_context.GameType, "GameTypeId", "Name", tournament.FkGameType);
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
            _context.Tournament.Remove(tournament);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Tournaments));
        }

        private bool TournamentExists(int id)
        {
            return _context.Tournament.Any(e => e.TournamentId == id);
        }
        #endregion
    }
}
