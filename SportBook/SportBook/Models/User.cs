using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportBook.Models
{
    public partial class User
    {
        public int Id { get; set; }
        [StringLength(35, MinimumLength = 3)]
        [Required]
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 7)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$")]
        [Required]
        public string Email { get; set; }
    }
}