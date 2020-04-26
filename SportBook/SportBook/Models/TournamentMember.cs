using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class TournamentMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TournamentMemberId { get; set; }
        public int FkTournament { get; set; }
        public int FkTeam { get; set; }

        public virtual Team FkTeamNavigation { get; set; }
        public virtual Tournament FkTournamentNavigation { get; set; }
    }
}
