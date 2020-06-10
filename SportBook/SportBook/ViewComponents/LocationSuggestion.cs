using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.ViewComponents
{
    public class LocationSuggestion : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public LocationSuggestion(SportbookDatabaseContext context)
        {
            _context = context;
        }
        private IQueryable<GameType> GetOfflineGames()
        {
            var offlineGames = _context.GameType.Where(x => x.IsOnline == false);
            return offlineGames;
        }
        public async Task<IViewComponentResult> InvokeAsync(string addr, decimal latitude, decimal longitude)
        {
            var _gameType = GetOfflineGames().First().GameTypeId;
            var locations = _context.Location.ToList();
            SelectList gameTypes = new SelectList(GetOfflineGames(), "GameTypeId", "Name", _gameType);
            ViewData["GameTypes"] = gameTypes;
            int dummyCity = _context.City.First(c => c.Name == "SportbookCity").CityId;
            ViewData["DummyCity"] = dummyCity;
            Location location = new Location();
            location.Address = addr;
            location.FkCity = dummyCity;
            location.FkGameType = _gameType;
            location.Latitude = latitude;
            location.Longitude = longitude;

            return View(location);
        }
    }
}
