using System.ComponentModel.DataAnnotations;

namespace ElectronicJournal.Domain.Enum
{
    public enum Role
    {
        [Display(Name = "Учитель")]
        Teacher = 1,

        [Display(Name = "Руководство")]
        Director = 2
    }
}
