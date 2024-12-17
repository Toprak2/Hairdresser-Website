namespace Hairdresser_Website.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; } // In minutes
        public decimal Price { get; set; }

        // Foreign Key
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
