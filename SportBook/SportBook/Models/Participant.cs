using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class Participant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantId { get; set; }
        [DisplayName("User")]
        public int FkUser { get; set; }
        [DisplayName("Event")]
        public int FkEvent { get; set; }
        [DisplayName("Team")]
        public int FkTeam { get; set; }
        [DisplayName("Event")]
        public virtual Event FkEventNavigation { get; set; }
        [DisplayName("Team")]
        public virtual Team FkTeamNavigation { get; set; }
        [DisplayName("User")]
        public virtual User FkUserNavigation { get; set; }
    }
}
