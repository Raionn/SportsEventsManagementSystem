using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }
        public int FkCity { get; set; }
        public int FkGameType { get; set; }

        public virtual City FkCityNavigation { get; set; }
        public virtual GameType FkGameTypeNavigation { get; set; }
        public virtual ICollection<Event> Event { get; set; }
    }
}
