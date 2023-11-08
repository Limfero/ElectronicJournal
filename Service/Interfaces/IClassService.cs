using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Response;

namespace ElectronicJournal.Service.Interfaces
{
    public interface IClassService
    {
        IBaseResponse<List<Class>> GetAllClasses();

        Task<IBaseResponse<Class>> GetClassById(int id);
    }
}
