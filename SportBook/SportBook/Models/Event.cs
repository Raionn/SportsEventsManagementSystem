using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class Event
    {
        public Event()
        {
            EventInvitation = new HashSet<EventInvitation>();
            Participant = new HashSet<Participant>();
        }

        public string Title { get; set; }
        public int? MaxParticipantAmt { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsTeamEvent { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }
        public int FkOwner { get; set; }
        public int FkLocation { get; set; }
        public int FkGameType { get; set; }

        public virtual GameType FkGameTypeNavigation { get; set; }
        public virtual Location FkLocationNavigation { get; set; }
        public virtual User FkOwnerNavigation { get; set; }
        public virtual ICollection<EventInvitation> EventInvitation { get; set; }
        public virtual ICollection<Participant> Participant { get; set; }
    }
}
