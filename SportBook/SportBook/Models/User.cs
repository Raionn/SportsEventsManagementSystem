using System;
using System.Collections.Generic;

namespace SportBook.Models
{
    public partial class User
    {
        public User()
        {
            Event = new HashSet<Event>();
            EventInvitation = new HashSet<EventInvitation>();
            Participant = new HashSet<Participant>();
            Team = new HashSet<Team>();
            TeamInvitation = new HashSet<TeamInvitation>();
            TeamMember = new HashSet<TeamMember>();
            Tournament = new HashSet<Tournament>();
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string ExternalId { get; set; }
        public string PictureUrl { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<EventInvitation> EventInvitation { get; set; }
        public virtual ICollection<Participant> Participant { get; set; }
        public virtual ICollection<Team> Team { get; set; }
        public virtual ICollection<TeamInvitation> TeamInvitation { get; set; }
        public virtual ICollection<TeamMember> TeamMember { get; set; }
        public virtual ICollection<Tournament> Tournament { get; set; }
    }
}
