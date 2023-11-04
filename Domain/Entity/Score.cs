using ElectronicJournal.Domain.Enum;

namespace ElectronicJournal.Domain.Entity
{
    public class Score
    {
        public int Id { get; set; }

        public Grade Grade { get; set; }

        public string Description { get; set; }

        public int IdStudent { get; set; }
        public Student? Student { get; set; }

        public int IdLesson { get; set; }
        public Lesson Lesson { get; set; }
    }
}
