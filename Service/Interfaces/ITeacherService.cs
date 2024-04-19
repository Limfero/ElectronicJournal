using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;

namespace ElectronicJournal.Service.Interfaces
{
    public interface ITeacherService
    {
        IBaseResponse<List<Teacher>> GetAllTeachers();

        Task<IBaseResponse<Teacher>> CreateTeacher(TeacherViewModel model);

        Task<IBaseResponse<Teacher>> DeleteTeacher(int id);

        Task<IBaseResponse<Teacher>> UpdateTeacher(int id, TeacherViewModel model);

        Task<IBaseResponse<Teacher>> GetTeacherById(int id);
    }
}
