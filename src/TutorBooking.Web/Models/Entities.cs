using System.ComponentModel.DataAnnotations;

namespace TutorBooking.Web.Models;

public static class RoleNames
{
    public const string Student = "Student";
    public const string Tutor = "Tutor";
    public const string Admin = "Admin";
}

public class Role
{
    public int Id { get; set; }
    [Required, StringLength(30)] public string Name { get; set; } = "";
}

public class User
{
    public int Id { get; set; }
    [Required, StringLength(150)] public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required, StringLength(100)] public string FullName { get; set; } = "";
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class City
{
    public int Id { get; set; }
    [Required, StringLength(80)] public string Name { get; set; } = "";
}

public class Subject
{
    public int Id { get; set; }
    [Required, StringLength(80)] public string Name { get; set; } = "";
}

public enum LessonFormat { Online = 0, Offline = 1, Both = 2 }
public enum ProfileStatus { Pending = 0, Approved = 1, Rejected = 2 }
public enum BookingStatus { Confirmed = 0, Cancelled = 1, Rejected = 2, Completed = 3 }

public class TutorProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int? CityId { get; set; }
    public City? City { get; set; }
    [StringLength(2000)] public string Description { get; set; } = "";
    [StringLength(500)] public string Education { get; set; } = "";
    public int ExperienceYears { get; set; }
    public int PricePerHour { get; set; }
    public LessonFormat Format { get; set; } = LessonFormat.Online;
    public ProfileStatus Status { get; set; } = ProfileStatus.Pending;

    public List<TutorSubject> Subjects { get; set; } = new();
    public List<Slot> Slots { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}

public class TutorSubject
{
    public int TutorProfileId { get; set; }
    public TutorProfile TutorProfile { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
}

public class Slot
{
    public int Id { get; set; }
    public int TutorProfileId { get; set; }
    public TutorProfile TutorProfile { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    // ConcurrencyCheck защищает от двойного бронирования одного слота
    [ConcurrencyCheck] public bool IsBooked { get; set; }

    public List<Booking> Bookings { get; set; } = new();
}

public class Booking
{
    public int Id { get; set; }
    public int SlotId { get; set; }
    public Slot Slot { get; set; } = null!;
    public int StudentId { get; set; }
    public User Student { get; set; } = null!;
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Review? Review { get; set; }
}

public class Review
{
    public int Id { get; set; }
    public int TutorProfileId { get; set; }
    public TutorProfile TutorProfile { get; set; } = null!;
    public int StudentId { get; set; }
    public User Student { get; set; } = null!;
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    [Range(1, 5)] public int Rating { get; set; }
    [StringLength(1000)] public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class Complaint
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
    public int TutorProfileId { get; set; }
    public TutorProfile TutorProfile { get; set; } = null!;
    [Required, StringLength(1000)] public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsResolved { get; set; }
}

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    [Required, StringLength(500)] public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }
}
