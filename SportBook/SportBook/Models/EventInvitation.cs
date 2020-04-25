using System;
using System.Collections.Generic;

namespace SportBook.Models
{
    public partial class EventInvitation
    {
        public bool IsAccepted { get; set; }
        public int EventInvitationId { get; set; }
        public int FkUser { get; set; }
        public int FkEvent { get; set; }

        public virtual Event FkEventNavigation { get; set; }
        public virtual User FkUserNavigation { get; set; }
    }
}
