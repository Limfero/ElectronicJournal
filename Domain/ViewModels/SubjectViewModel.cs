using ElectronicJournal.Domain.Entity;

namespace ElectronicJournal.Domain.ViewModels
{
    public class SubjectViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Teacher> Teachers { get; set; } = new();
    }
}
