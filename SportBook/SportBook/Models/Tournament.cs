using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class Tournament
    {
        public Tournament()
        {
            TournamentMember = new HashSet<TournamentMember>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int? MaxParticipantAmt { get; set; }
        public DateTime? Start { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TournamentId { get; set; }
        public int FkGameType { get; set; }
        public int FkOwner { get; set; }

        public virtual GameType FkGameTypeNavigation { get; set; }
        public virtual User FkOwnerNavigation { get; set; }
        public virtual ICollection<TournamentMember> TournamentMember { get; set; }
    }
}
