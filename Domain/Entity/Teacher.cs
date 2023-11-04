using ElectronicJournal.Domain.Enum;

namespace ElectronicJournal.Domain.Entity
{
    public class Teacher
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
        public List<Lesson> Lessons { get; set; }

        public List<Subject> Subjects { get; set; }
    }
}
