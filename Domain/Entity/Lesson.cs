namespace ElectronicJournal.Domain.Entity
{
    public class Lesson
    {
        public int Id { get; set; }

        public int ClassRoom { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string Description { get; set; }

        public int IdClass { get; set; }
        public Class Class { get; set; }

        public int IdTeacher { get; set; }
        public Teacher? Teacher { get; set; }

        public int IdSubject { get; set; }
        public Subject? Subject { get; set; }

        public List<Score>? Scores { get; set; }
    }
}
