using ElectronicJournal.Domain.Enum;

namespace ElectronicJournal.Domain.ViewModels
{
    public class ScoreViewModel
    {
        public Grade Grade { get; set; }

        public string Description { get; set; }

        public int IdStudent { get; set; }

        public int IdLesson { get; set; }
    }
}
