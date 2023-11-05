using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels.Lesson;

namespace ElectronicJournal.Service.Interfaces
{
    public interface ILessonService
    {
        Task<IBaseResponse<Dictionary<DateOnly, List<Lesson>>>> GetLessonsOfDateAndClass(DateOnly date, int idClass);

        IBaseResponse<List<Lesson>> GetAllLessons();

        Task<IBaseResponse<Lesson>> CreateLesson(LessonViewModel model);
    }
}
