using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;

namespace ElectronicJournal.Service.Interfaces
{
    public interface ILessonService
    {
        Task<IBaseResponse<Dictionary<DateOnly, List<Lesson>>>> GetLessonsOfDateAndClass(DateOnly date, int idClass);

        IBaseResponse<List<Lesson>> GetAllLessons();

        Task<IBaseResponse<List<Lesson>>> CreateRangeLessons(LessonViewModel model);
    }
}
