namespace ElectronicJournal.Domain.Entity
{
    public class Class
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Lesson> Lessons { get; set; } = new();

        public List<Student> Students { get; set; } = new();
    }
}
