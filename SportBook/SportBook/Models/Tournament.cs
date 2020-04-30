using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [MaxLength(60, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        [RegularExpression(@"[A-Za-z0-9\s?.,!?]+", ErrorMessage = "Allowed letters,digits and ?.!, characters")]
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        [MaxLength(250, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        [RegularExpression(@"[A-Za-z0-9\s?.,!?]+", ErrorMessage = "Allowed letters,digits and ?.!, characters")]
        public string Description { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid number")]
        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Max participants")]
        public int? MaxParticipantAmt { get; set; }
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "This field is required")]
        public DateTime? Start { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TournamentId { get; set; }
        [DisplayName("Game Type")]
        public int FkGameType { get; set; }
        [DisplayName("Owner")]
        public int FkOwner { get; set; }
        [DisplayName("Game Type")]
        public virtual GameType FkGameTypeNavigation { get; set; }
        [DisplayName("Owner")]
        public virtual User FkOwnerNavigation { get; set; }
        public virtual ICollection<TournamentMember> TournamentMember { get; set; }
    }
}
