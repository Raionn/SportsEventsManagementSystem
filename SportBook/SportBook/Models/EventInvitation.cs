using System.ComponentModel;

namespace SportBook.Models
{
    public partial class EventInvitation
    {
        public int EventInvitationId { get; set; }
        public bool IsAccepted { get; set; }
        [DisplayName("User")]
        public int FkUser { get; set; }
        [DisplayName("Event")]
        public int FkEvent { get; set; }
        [DisplayName("Event")]
        public virtual Event FkEventNavigation { get; set; }
        [DisplayName("User")]
        public virtual User FkUserNavigation { get; set; }
    }
}
