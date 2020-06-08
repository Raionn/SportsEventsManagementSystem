using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.ViewModels
{
    public class LocationData
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public int LocationId { get; set; }
        public string Address { get; set; }
        public string Game { get; set; }
        public int GameId { get; set; }
        public int EventCount { get; set; }

        public LocationData(decimal? longtitude, decimal? latitude, string address, string game, int locationId, int gameId, int eventCount = 0)
        {
            Longitude = longtitude;
            Latitude = latitude;
            LocationId = locationId;
            Address = address;
            Game = game;
            GameId = gameId;
            EventCount = eventCount;
        }
    }
}
