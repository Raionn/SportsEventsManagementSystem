using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class TournamentMember
    {
        public int TournamentMemberId { get; set; }
        [DisplayName("External ID")]
        public int ExternalID { get; set; }
        [DisplayName("Tournament")]
        public int FkTournament { get; set; }
        [DisplayName("Team")]
        public int FkTeam { get; set; }
        [DisplayName("Team")]
        public virtual Team FkTeamNavigation { get; set; }
        [DisplayName("Tournament")]
        public virtual Tournament FkTournamentNavigation { get; set; }
    }
}
