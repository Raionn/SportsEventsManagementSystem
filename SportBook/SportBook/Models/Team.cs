using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int TeamId { get; set; }
        [MaxLength(60, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        [RegularExpression(@"[A-Za-z0-9\s?]+", ErrorMessage = "Allowed characters letters and digits")]
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        [MaxLength(250, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        [RegularExpression(@"[A-Za-z0-9\s?.,!?]+", ErrorMessage = "Allowed letters,digits and ?.!, characters")]
        public string Description { get; set; }
        [RegularExpression(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", ErrorMessage = "Provided url is in wrong format!")]
        public string LogoUrl { get; set; }
        [DisplayName("Owner")]
        public int FkOwner { get; set; }
        [DisplayName("Game Type")]
        public int FkGameType { get; set; }
        [DisplayName("Game Type")]
        public virtual GameType FkGameTypeNavigation { get; set; }
        [DisplayName("Owner")]
        public virtual User FkOwnerNavigation { get; set; }
        public virtual ICollection<Participant> Participant { get; set; }
        public virtual ICollection<TeamInvitation> TeamInvitation { get; set; }
        public virtual ICollection<TeamMember> TeamMember { get; set; }
        public virtual ICollection<TournamentMember> TournamentMember { get; set; }
    }
}
