using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBook.Models
{
    public partial class City
    {
        public City()
        {
            Location = new HashSet<Location>();
        }

        public string Name { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }

        public virtual ICollection<Location> Location { get; set; }
    }
}
