using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlatformWell.Models
{
    public class Platform
    {
        public Platform()
        {
            /* This constructor is for deserialization / ORM purpose */
            Well = new List<Well>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string UniqueName { get; set; }
        
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Well> Well { get; set; }
    }
}