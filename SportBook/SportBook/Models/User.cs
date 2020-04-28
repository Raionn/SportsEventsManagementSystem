using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class User
    {
        public User()
        {
            Event = new HashSet<Event>();
            EventInvitation = new HashSet<EventInvitation>();
            Participant = new HashSet<Participant>();
            Team = new HashSet<Team>();
            TeamInvitation = new HashSet<TeamInvitation>();
            TeamMember = new HashSet<TeamMember>();
            Tournament = new HashSet<Tournament>();
        }
        [MaxLength(60, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        [RegularExpression(@"[A-Za-z0-9\s?]+", ErrorMessage = "Allowed characters letters and digits")]
        public string Username { get; set; }
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage ="Invalid email input! Example: example@mail.com")]
        public string Email { get; set; }
        [RegularExpression(@"([A-Z][a-z]* ?)*", ErrorMessage = "Name contains only letters and starts with capital!")]
        public string Firstname { get; set; }
        [RegularExpression(@"([A-Z][a-z]* ?)*", ErrorMessage = "Name contains only letters and starts with capital!")]
        public string Lastname { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd}")]
        [DisplayName("Birth Date")]
        public DateTime? Birthdate { get; set; }
        public string ExternalId { get; set; }
        [RegularExpression(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", ErrorMessage = "Provided url is in wrong format!")]
        public string PictureUrl { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<EventInvitation> EventInvitation { get; set; }
        public virtual ICollection<Participant> Participant { get; set; }
        public virtual ICollection<Team> Team { get; set; }
        public virtual ICollection<TeamInvitation> TeamInvitation { get; set; }
        public virtual ICollection<TeamMember> TeamMember { get; set; }
        public virtual ICollection<Tournament> Tournament { get; set; }
    }
}
