using System;
using System.Collections.Generic;

namespace SportBook.Models
{
    public partial class TournamentMember
    {
        public int TournamentMemberId { get; set; }
        public int FkTournament { get; set; }
        public int FkTeam { get; set; }

        public virtual Team FkTeamNavigation { get; set; }
        public virtual Tournament FkTournamentNavigation { get; set; }
    }
}
