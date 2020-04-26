using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class TeamMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamMemberId { get; set; }
        public int FkUser { get; set; }
        public int FkTeam { get; set; }

        public virtual Team FkTeamNavigation { get; set; }
        public virtual User FkUserNavigation { get; set; }
    }
}
