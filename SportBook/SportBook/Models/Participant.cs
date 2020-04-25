using System;
using System.Collections.Generic;

namespace SportBook.Models
{
    public partial class Participant
    {
        public int ParticipantId { get; set; }
        public int FkUser { get; set; }
        public int FkEvent { get; set; }
        public int FkTeam { get; set; }

        public virtual Event FkEventNavigation { get; set; }
        public virtual Team FkTeamNavigation { get; set; }
        public virtual User FkUserNavigation { get; set; }
    }
}
