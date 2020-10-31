using System;
using System.Configuration;
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
                    .HasComputedColumnSql("(CONVERT([time],[LoginDate]-[LogoutDate]))");

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
