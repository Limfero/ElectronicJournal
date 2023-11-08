using ElectronicJournal.Domain.Entity;

namespace ElectronicJournal.DAL.Interfaces
{
    public interface IClassRepository : IBaseRepository<Class>
    {
        Task<Class> GetByIdAsync(int id);
    }
}
