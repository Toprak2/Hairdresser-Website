using Hairdresser_Website.Models;
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

        // Navigation Properties
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<SalonWorkingHours> WorkingHours { get; set; } = new List<SalonWorkingHours>();
    }
}