﻿// <auto-generated />
using System;
using Hairdresser_Website.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hairdresser_Website.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241217105824_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hairdresser_Website.Models.Appointment", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AppointmentId"));

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<int>("ServiceId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AppointmentId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ServiceId");

                    b.HasIndex("UserId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Hairdresser_Website.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("Availability")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Expertise")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SalonId")
                        .HasColumnType("integer");

                    b.HasKey("EmployeeId");

                    b.HasIndex("SalonId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Hairdresser_Website.Models.Salon", b =>
                {
                    b.Property<int>("SalonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SalonId"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WorkingHours")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SalonId");

                    b.ToTable("Salons");
                });

            modelBuilder.Entity("Hairdresser_Website.Models.Service", b =>
                {
                    b.Property<int>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ServiceId"));

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("SalonId")
                        .HasColumnType("integer");

                    b.HasKey("ServiceId");

                    b.HasIndex("SalonId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Hairdresser_Website.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Hairdresser_Website.Models.Appointment", b =>
                {
                    b.HasOne("Hairdresser_Website.Models.Employee", "Employee")
                        .WithMany("Appointments")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hairdresser_Website.Models.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hairdresser_Website.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Service");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hairdresser_Website.Models.Employee", b =>
                {
                    b.HasOne("Hairdresser_Website.Models.Salon", "Salon")
                        .WithMany("Employees")
                        .HasForeignKey("SalonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Salon");
                });

            modelBuilder.Entity("Hairdresser_Website.Models.Service", b =>
                {
                    b.HasOne("Hairdresser_Website.Models.Salon", "Salon")
                        .WithMany("Services")
                        .HasForeignKey("SalonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Salon");
                });

            modelBuilder.Entity("Hairdresser_Website.Models.Employee", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("Hairdresser_Website.Models.Salon", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Services");
                });
#pragma warning restore 612, 618
        }
    }
}
