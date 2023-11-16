using ElectronicJournal.Domain.Entity;

namespace ElectronicJournal.DAL.Interfaces
{
    public interface ITeacherRepository : IBaseRepository<Teacher>
    {
        Task<Teacher> GetByIdAsync(int id);
    }
}
