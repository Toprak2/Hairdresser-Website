﻿using Hairdresser_Website.Models;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser_Website.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Salon> Salons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmployeeAvailability> EmployeeAvailability { get; set; } // Add this
        public DbSet<SalonWorkingHours> SalonWorkingHours { get; set; } // Add this

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Add configurations here if necessary
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId); // Explicitly defining primary key if required
        }
    }
}