namespace Hairdresser_Website.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Confirmed", "Cancelled"

        // Foreign Keys
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
