using PlatformWell.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlatformWell.Dtos
{
    public class PlatformUpdateDto
    {
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