using Microsoft.EntityFrameworkCore;
using TutorBooking.Web.Models;

namespace TutorBooking.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<TutorProfile> TutorProfiles => Set<TutorProfile>();
    public DbSet<TutorSubject> TutorSubjects => Set<TutorSubject>();
    public DbSet<Slot> Slots => Set<Slot>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Complaint> Complaints => Set<Complaint>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>().HasIndex(u => u.Email).IsUnique();
        b.Entity<Role>().HasIndex(r => r.Name).IsUnique();

        b.Entity<TutorSubject>().HasKey(ts => new { ts.TutorProfileId, ts.SubjectId });
        b.Entity<TutorSubject>()
            .HasOne(ts => ts.Subject).WithMany().HasForeignKey(ts => ts.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<TutorProfile>().HasIndex(t => t.UserId).IsUnique();
        b.Entity<TutorProfile>()
            .HasOne(t => t.City).WithMany().HasForeignKey(t => t.CityId)
            .OnDelete(DeleteBehavior.SetNull);

        b.Entity<Booking>()
            .HasOne(x => x.Student).WithMany().HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        b.Entity<Booking>()
            .HasOne(x => x.Review).WithOne(r => r.Booking).HasForeignKey<Review>(r => r.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Review>()
            .HasOne(r => r.Student).WithMany().HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        b.Entity<Review>()
            .HasOne(r => r.TutorProfile).WithMany(t => t.Reviews).HasForeignKey(r => r.TutorProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Complaint>()
            .HasOne(c => c.Author).WithMany().HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        b.Entity<Complaint>()
            .HasOne(c => c.TutorProfile).WithMany().HasForeignKey(c => c.TutorProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Notification>()
            .HasOne(n => n.User).WithMany().HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
