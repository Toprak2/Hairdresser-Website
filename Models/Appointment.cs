namespace Hairdresser_Website.Models
{
    public enum AppointmentStatus
    {
        Confirmed,
        Cancelled,
        Done
    }
    public class Appointment
    {
        private DateTime appointmentDate;
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate
        {
            get => appointmentDate;
            set => appointmentDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        public AppointmentStatus Status { get; set; } // Use the enum

        // Foreign Keys
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
