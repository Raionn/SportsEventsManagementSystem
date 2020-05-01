using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class TeamMember
    {
        public int TeamMemberId { get; set; }
        [DisplayName("User")]
        public int FkUser { get; set; }
        [DisplayName("Team")]
        public int FkTeam { get; set; }
        [DisplayName("Team")]
        public virtual Team FkTeamNavigation { get; set; }
        [DisplayName("Team")]
        public virtual User FkUserNavigation { get; set; }
    }
}
