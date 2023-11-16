using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;

namespace ElectronicJournal.Service.Interfaces
{
    public interface IClassService
    {
        IBaseResponse<List<Class>> GetAllClasses();

        Task<IBaseResponse<Class>> CreateClass(ClassViewModel model);

        Task<IBaseResponse<Class>> DeleteClass(int id);

        Task<IBaseResponse<Class>> UpdateClass(int id, ClassViewModel model);
    }
}
