using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class EventInvitation
    {
        public bool IsAccepted { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventInvitationId { get; set; }
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
