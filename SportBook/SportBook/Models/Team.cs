using System;
using System.Collections.Generic;

namespace SportBook.Models
{
    public partial class Team
    {
        public Team()
        {
            Participant = new HashSet<Participant>();
            TeamInvitation = new HashSet<TeamInvitation>();
            TeamMember = new HashSet<TeamMember>();
            TournamentMember = new HashSet<TournamentMember>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public int TeamId { get; set; }
        public int FkOwner { get; set; }
        public int FkGameType { get; set; }

        public virtual GameType FkGameTypeNavigation { get; set; }
        public virtual User FkOwnerNavigation { get; set; }
        public virtual ICollection<Participant> Participant { get; set; }
        public virtual ICollection<TeamInvitation> TeamInvitation { get; set; }
        public virtual ICollection<TeamMember> TeamMember { get; set; }
        public virtual ICollection<TournamentMember> TournamentMember { get; set; }
    }
}
