using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class TeamInvitation
    {
        //[MaxLength(60, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        //[RegularExpression(@"[A-Za-z0-9\s?.,!?]+", ErrorMessage = "Allowed letters,digits and ?.!, characters")]
        //public string Text { get; set; }
        public bool IsAccepted { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamInvitationId { get; set; }
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
