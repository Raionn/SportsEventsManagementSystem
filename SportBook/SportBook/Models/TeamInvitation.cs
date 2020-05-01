using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class TeamInvitation
    {
        public int TeamInvitationId { get; set; }
        public bool IsAccepted { get; set; }
        [DisplayName("User")]
        public int FkUser { get; set; }
        [DisplayName("Team")]
        public int FkTeam { get; set; }
        [DisplayName("Team")]
        public virtual Team FkTeamNavigation { get; set; }
        [DisplayName("User")]
        public virtual User FkUserNavigation { get; set; }
    }
}
