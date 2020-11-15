using System;
using System.Configuration;
using Amonic_Airlines_CORE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Amonic_Airlines.Models
{
    public partial class AmonicContext : DbContext
    {
        private static AmonicContext _context;
        public AmonicContext()
        {
        }

        public AmonicContext(DbContextOptions<AmonicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityUser> ActivityUser { get; set; }
        public virtual DbSet<Airport> Airport { get; set; }
        public virtual DbSet<Flight> Flight { get; set; }
        public virtual DbSet<FlightSchedules> FlightSchedules { get; set; }
        public virtual DbSet<Office> Offices { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["AMONIC-Airlines"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityUser>(entity =>
            {
                entity.HasKey(e => new { e.Email, e.LoginDate })
                    .HasName("ActivityUser_PK");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.LogoutDate).HasColumnType("datetime");

                entity.Property(e => e.TimeSpent)
                    .HasColumnName("Time_spent")
                    .HasComputedColumnSql("(CONVERT([time],[LogoutDate]-[LoginDate]))");

                entity.Property(e => e.UnsuccessfulLogoutReason)
                    .HasColumnName("Unsuccessful_logout_reason")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.EmailNavigation)
                    .WithMany(p => p.ActivityUser)
                    .HasForeignKey(d => d.Email)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ActivityU__Email__3F466844");
            });

            modelBuilder.Entity<Airport>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FlightSchedules>(entity =>
            {
                entity.HasKey(e => new {e.CodeOfFlight, e.DateTimeOfRace })
                    .HasName("PK__Flight_S__5DD08D7872ECAEC2");

                entity.ToTable("Flight_Schedules");

                entity.Property(e => e.FlightNumber)
                    .HasColumnName("Flight_number")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateTimeOfRace).HasColumnType("datetime");

                entity.Property(e => e.EconomyPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.IsCanceled).HasColumnName("isCanceled");

                entity.HasOne(d => d.CodeOfFlightNavigation)
                    .WithMany(p => p.FlightSchedules)
                    .HasForeignKey(d => d.CodeOfFlight)
                    .HasConstraintName("FK__Flight_Sc__CodeO__4F7CD00D");

                entity.HasOne(d => d.FromAirNavigation)
                    .WithMany(p => p.FlightSchedulesFromAirNavigation)
                    .HasForeignKey(d => d.FromAir)
                    .HasConstraintName("FK__Flight_Sc__FromA__4D94879B");

                entity.HasOne(d => d.ToAirNavigation)
                    .WithMany(p => p.FlightSchedulesToAirNavigation)
                    .HasForeignKey(d => d.ToAir)
                    .HasConstraintName("FK__Flight_Sc__ToAir__4E88ABD4");
            });

            modelBuilder.Entity<Office>(entity =>
            {
                entity.HasKey(e => e.OfficeCode)
                    .HasName("PK__Offices__2CAD32AF1CA9E539");

                entity.Property(e => e.OfficeCode)
                    .HasColumnName("Office_code")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__Users__A9D105353CDF422A");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SecondName)
                    .IsRequired()
                    .HasColumnName("Second_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.OfficeNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Office)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__Office__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public static AmonicContext GetContext()
        {
            if (_context is null)
                _context = new AmonicContext();
            return _context;
        }
    }
}
