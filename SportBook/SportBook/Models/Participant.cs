using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class Participant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantId { get; set; }
        public int FkUser { get; set; }
        public int FkEvent { get; set; }
        public int FkTeam { get; set; }

        public virtual Event FkEventNavigation { get; set; }
        public virtual Team FkTeamNavigation { get; set; }
        public virtual User FkUserNavigation { get; set; }
    }
}
