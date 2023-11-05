namespace ElectronicJournal.Domain.Entity
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Teacher> Teachers { get; set; } = new();

        public List<Lesson> Lessons { get; set; } = new();
    }
}
