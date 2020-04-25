using System;
using System.Collections.Generic;

namespace SportBook.Models
{
    public partial class TeamMember
    {
        public int TeamMemberId { get; set; }
        public int FkUser { get; set; }
        public int FkTeam { get; set; }

        public virtual Team FkTeamNavigation { get; set; }
        public virtual User FkUserNavigation { get; set; }
    }
}
