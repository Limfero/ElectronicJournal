namespace ElectronicJournal.Domain.ViewModels.Lesson
{
    public class LessonViewModel
    {
        public int ClassRoom { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string Description { get; set; }

        public int IdClass { get; set; }

        public int IdTeacher { get; set; }

        public int IdSubject { get; set; }
    }
}
