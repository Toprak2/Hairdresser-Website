using System.Collections.Generic;

namespace Hairdresser_Website.Models
    {
        public class Salon
        {
            public int SalonId { get; set; }
            public string Name { get; set; }
            public string Location { get; set; }
            public string WorkingHours { get; set; } // e.g., "09:00-18:00"

            // Navigation Properties
            public ICollection<Employee> Employees { get; set; }
            public ICollection<Service> Services { get; set; }
        }
    }

