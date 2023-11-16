using ElectronicJournal.Domain.Entity;

namespace ElectronicJournal.DAL.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Student> GetByIdAsync(int id);
    }
}
