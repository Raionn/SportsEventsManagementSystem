using System;
using System.Collections.Generic;

namespace SportBook.Models
{
    public partial class GameType
    {
        public GameType()
        {
            Event = new HashSet<Event>();
            Location = new HashSet<Location>();
            Team = new HashSet<Team>();
            Tournament = new HashSet<Tournament>();
        }

        public string Name { get; set; }
        public int GameTypeId { get; set; }

        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<Team> Team { get; set; }
        public virtual ICollection<Tournament> Tournament { get; set; }
    }
}
