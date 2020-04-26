using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class TeamInvitation
    {
        public string Text { get; set; }
        public bool IsAccepted { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamInvitationId { get; set; }
        public int FkUser { get; set; }
        public int FkTeam { get; set; }

        public virtual Team FkTeamNavigation { get; set; }
        public virtual User FkUserNavigation { get; set; }
    }
}
