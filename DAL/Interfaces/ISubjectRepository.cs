using ElectronicJournal.Domain.Entity;

namespace ElectronicJournal.DAL.Interfaces
{
    public interface ISubjectRepository : IBaseRepository<Subject>
    {
        Task<Subject> GetByIdAsync(int id);
    }
}
