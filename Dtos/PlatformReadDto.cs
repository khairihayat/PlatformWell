using PlatformWell.Models;
using System;
using System.Collections.Generic;

namespace PlatformWell.Dtos
{
    public class PlatformReadDto
    {
        public int Id { get; set; }

        public string UniqueName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Well> Well { get; set; }

    }
}