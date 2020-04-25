using System;
using System.Collections.Generic;

namespace SportBook.Models
{
    public partial class City
    {
        public City()
        {
            Location = new HashSet<Location>();
        }

        public string Name { get; set; }
        public int CityId { get; set; }

        public virtual ICollection<Location> Location { get; set; }
    }
}
