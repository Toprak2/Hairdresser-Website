namespace Hairdresser_Website.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Expertise { get; set; } // e.g., "Haircut, Hair Coloring"
        //public string Availability { get; set; } // e.g., "09:00-12:00"

        // Foreign Key
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }

        // Navigation Properties
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<EmployeeAvailability> EmployeeAvailabilities { get; set; } = new List<EmployeeAvailability>();
    }
}
