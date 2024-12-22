using System.ComponentModel.DataAnnotations;

namespace Hairdresser_Website.Models
{
    public class EmployeeAvailability
    {
        public int EmployeeAvailabilityId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        // Foreign Key
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
