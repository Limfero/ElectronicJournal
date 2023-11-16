using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;

namespace ElectronicJournal.Service.Interfaces
{
    public interface ISubjectService
    {
        IBaseResponse<List<Subject>> GetAllSubjects();

        Task<IBaseResponse<Subject>> CreateSubject(SubjectViewModel model);

        Task<IBaseResponse<Subject>> DeleteSubject(int id);

        Task<IBaseResponse<Subject>> UpdateSubject(int id, SubjectViewModel model);
    }
}
