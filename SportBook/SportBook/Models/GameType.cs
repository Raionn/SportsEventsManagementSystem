using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameTypeId { get; set; }

        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<Team> Team { get; set; }
        public virtual ICollection<Tournament> Tournament { get; set; }
    }
}
