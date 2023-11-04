using ElectronicJournal.Domain.Entity;

namespace ElectronicJournal.DAL.Interfaces
{
    public interface ILessonRepository : IBaseRepository<Lesson>
    {
        Task<Lesson> GetByIdAsync(int id);

        Task<List<Lesson>> GetByClassAndDateAsync(int idClass, DateOnly date);
    }
}
