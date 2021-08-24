using System;
using System.ComponentModel.DataAnnotations;

namespace PlatformWell.Models
{
    public class Well
    {
        [Required]
        public int Id { get; set; }

        public int PlatformId { get; set; }
        //public virtual Platform Platform { get; set; }

        [Required]
        public string UniqueName { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}