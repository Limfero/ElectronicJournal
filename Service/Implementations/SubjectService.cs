using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.DAL.Repositories;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;
using ElectronicJournal.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.Service.Implementations
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<IBaseResponse<Subject>> CreateSubject(SubjectViewModel model)
        {
            try
            {
                var entity = await _subjectRepository.GetAll().FirstOrDefaultAsync(subject => subject.Name == model.Name);

                if (entity != null)
                    return new BaseResponse<Subject>
                    {
                        Description = "Предмет с таким названием уже есть",
                        StatusCode = StatusCode.SubjectNotCreated
                    };

                var subject = new Subject()
                {
                     Description = model.Description,
                     Teachers = model.Teachers,
                     Name = model.Name,
                     Lessons = new()
                };

                var response = await _subjectRepository.CreateAsync(entity);

                return new BaseResponse<Subject>()
                {
                    Data = response,
                    Description = "предмет был создан",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Subject>()
                {
                    Description = $"[SubjectService.CreateSubject] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Subject>> DeleteSubject(int id)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(id);

                if (subject == null)
                    return new BaseResponse<Subject>()
                    {
                        Description = "Такого предмета нет!",
                        StatusCode = StatusCode.SubjectNotFound
                    };

                var response = await _subjectRepository.RemoveAsync(subject);

                return new BaseResponse<Subject>()
                {
                    Data = response,
                    Description = $"Предмет был успещно удален!",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Subject>()
                {
                    Description = $"[SubjectService.DeleteSubject] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Subject>> GetAllSubjects()
        {
            try
            {
                var response = _subjectRepository.GetAll().ToList();

                foreach (var subject in response)
                    foreach (var teacher in subject.Teachers)
                        teacher.Subjects.Clear();

                if (response == null)
                {
                    return new BaseResponse<List<Subject>>()
                    {
                        Description = "Данные не найдены",
                        StatusCode = StatusCode.SubjectNotFound
                    };
                }

                return new BaseResponse<List<Subject>>()
                {
                    Data = response,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Subject>>()
                {
                    Description = $"[SubjectService.GetAllSubjects] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Subject>> UpdateSubject(int id, SubjectViewModel model)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(id);

                if (subject == null)
                    return new BaseResponse<Subject>()
                    {
                        Description = "Такого предмета нет!",
                        StatusCode = StatusCode.SubjectNotFound
                    };

                subject.Description = model.Description;
                subject.Teachers = model.Teachers;
                subject.Name = model.Name;

                await _subjectRepository.UpdateAsync(subject);

                return new BaseResponse<Subject>()
                {
                    Description = "Информация о предмете обновлена!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Subject>()
                {
                    Description = $"[SubjectService.UpdateSubject] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
