using PlatformWell.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlatformWell.Dtos
{
    public class WellCreateDto
    {
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