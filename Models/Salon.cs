using Hairdresser_Website.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hairdresser_Website.Models
{
    public class Salon
    {
        public int SalonId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string WorkingHours { get; set; } // e.g., "09:00-18:00"

        // Navigation Properties
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
