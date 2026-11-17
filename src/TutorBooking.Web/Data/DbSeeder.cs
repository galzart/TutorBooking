using TutorBooking.Web.Models;
using TutorBooking.Web.Services;

namespace TutorBooking.Web.Data;

/// <summary>Начальные данные: роли, справочники, демо-пользователи.</summary>
public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Roles.Any()) return;

        var student = new Role { Name = RoleNames.Student };
        var tutor = new Role { Name = RoleNames.Tutor };
        var admin = new Role { Name = RoleNames.Admin };
        db.Roles.AddRange(student, tutor, admin);

        var subjects = new[] { "Математика", "Русский язык", "Физика", "Информатика",
                               "Английский язык", "Обществознание", "Химия", "Биология" }
            .Select(n => new Subject { Name = n }).ToList();
        db.Subjects.AddRange(subjects);

        var cities = new[] { "Москва", "Санкт-Петербург", "Казань", "Екатеринбург" }
            .Select(n => new City { Name = n }).ToList();
        db.Cities.AddRange(cities);
        db.SaveChanges();

        // Демо-аккаунты (пароли указаны в README)
        db.Users.Add(new User { Email = "admin@tutor.local", FullName = "Администратор",
            PasswordHash = PasswordService.Hash("Admin123!"), RoleId = admin.Id });
        var demoStudent = new User { Email = "student@tutor.local", FullName = "Иван Петров",
            PasswordHash = PasswordService.Hash("Student123!"), RoleId = student.Id };
        var demoTutor = new User { Email = "tutor@tutor.local", FullName = "Анна Сергеевна Иванова",
            PasswordHash = PasswordService.Hash("Tutor123!"), RoleId = tutor.Id };
        db.Users.AddRange(demoStudent, demoTutor);
        db.SaveChanges();

        var profile = new TutorProfile
        {
            UserId = demoTutor.Id,
            CityId = cities[0].Id,
            Description = "Готовлю к ОГЭ и ЕГЭ по математике. Индивидуальный подход, разбор типовых ошибок.",
            Education = "МГУ, механико-математический факультет",
            ExperienceYears = 8,
            PricePerHour = 1500,
            Format = LessonFormat.Both,
            Status = ProfileStatus.Approved
        };
        profile.Subjects.Add(new TutorSubject { SubjectId = subjects[0].Id });
        db.TutorProfiles.Add(profile);
        db.SaveChanges();

        // Слоты на ближайшие 5 дней
        for (int d = 1; d <= 5; d++)
            foreach (var hour in new[] { 10, 12, 16 })
            {
                var start = DateTime.Today.AddDays(d).AddHours(hour);
                db.Slots.Add(new Slot { TutorProfileId = profile.Id, Start = start, End = start.AddHours(1) });
            }
        db.SaveChanges();
    }
}
