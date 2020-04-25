using System;
using System.Collections.Generic;

namespace SportBook.Models
{
    public partial class Location
    {
        public Location()
        {
            Event = new HashSet<Event>();
        }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Address { get; set; }
        public int LocationId { get; set; }
        public int FkCity { get; set; }
        public int FkGameType { get; set; }

        public virtual City FkCityNavigation { get; set; }
        public virtual GameType FkGameTypeNavigation { get; set; }
        public virtual ICollection<Event> Event { get; set; }
    }
}
