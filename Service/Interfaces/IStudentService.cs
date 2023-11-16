using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;

namespace ElectronicJournal.Service.Interfaces
{
    public interface IStudentService
    {
        IBaseResponse<List<Student>> GetAllStudent();

        Task<IBaseResponse<Student>> CreateStudent(StudentViewModel model);

        Task<IBaseResponse<Student>> DeleteStudent(int id);

        Task<IBaseResponse<Student>> UpdateStudent(int id, StudentViewModel model);

        Task<IBaseResponse<Student>> GetStudentById(int id);
    }
}
