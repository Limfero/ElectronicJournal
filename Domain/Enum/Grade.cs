using System.ComponentModel.DataAnnotations;

namespace ElectronicJournal.Domain.Enum
{
    public enum Grade
    {
        [Display(Name = "Неудовлетворительно")]
        Unsatisfactory = 2,

        [Display(Name = "Удовлетворительно")]
        Satisfactory = 3,

        [Display(Name = "Хорошо")]
        Good = 4,

        [Display(Name = "Отлично")]
        Excellent = 5
    }
}
