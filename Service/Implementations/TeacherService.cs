using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.DAL.Repositories;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Helpers;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;
using ElectronicJournal.Service.Interfaces;

namespace ElectronicJournal.Service.Implementations
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<IBaseResponse<Teacher>> CreateTeacher(TeacherViewModel model)
        {
            try
            {
                var teacher = new Teacher()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Lessons = new(),
                    Login = model.Login,
                    MiddleName = model.MiddleName,
                    Password = HashPasswordHelper.HashPassword(model.Password),
                    Role = (Role)model.Role,                   
                    Subjects = model.Subjects
                };

                var response = await _teacherRepository.CreateAsync(teacher);

                return new BaseResponse<Teacher>()
                {
                    Data = response,
                    Description = "Учитель был создан",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Teacher>()
                {
                    Description = $"[TeacherService.CreateTeacher] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Teacher>> DeleteTeacher(int id)
        {
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(id);

                if (teacher == null)
                    return new BaseResponse<Teacher>()
                    {
                        Description = "Такого учителя нет!",
                        StatusCode = StatusCode.TeacherNotFound
                    };

                await _teacherRepository.RemoveAsync(teacher);

                return new BaseResponse<Teacher>()
                {
                    Description = "Учитель был удален",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Teacher>()
                {
                    Description = $"[TeacherService.CreateTeacher] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Teacher>> GetAllTeachers()
        {
            try
            {
                var response = _teacherRepository.GetAll().ToList();

                foreach (var teacher in response)
                {
                    teacher.Lessons = new();
                    foreach (var subject in teacher.Subjects)
                        subject.Teachers.Clear();
                }

                if (response == null)
                {
                    return new BaseResponse<List<Teacher>>()
                    {
                        Description = "Данные не найдены",
                        StatusCode = StatusCode.TeacherNotFound
                    };
                }

                return new BaseResponse<List<Teacher>>()
                {
                    Data = response,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Teacher>>()
                {
                    Description = $"[TeacherService.GetAllTeachers] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Teacher>> GetTeacherById(int id)
        {
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(id);

                if (teacher == null)
                    return new BaseResponse<Teacher>()
                    {
                        Description = "Такого учителя нет!",
                        StatusCode = StatusCode.TeacherNotFound
                    };

                return new BaseResponse<Teacher>()
                {
                    Data = teacher,
                    Description = "Учитель найден!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Teacher>()
                {
                    Description = $"[TeacherService.GetTeacherById] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Teacher>> UpdateTeacher(int id, TeacherViewModel model)
        {
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(id);

                if (teacher == null)
                    return new BaseResponse<Teacher>()
                    {
                        Description = "Такого учителя нет!",
                        StatusCode = StatusCode.TeacherNotFound
                    };

                teacher.FirstName = model.FirstName;
                teacher.LastName = model.LastName;
                teacher.Subjects = model.Subjects;
                teacher.MiddleName = model.MiddleName;
                teacher.Password = model.Password;

                await _teacherRepository.UpdateAsync(teacher);

                return new BaseResponse<Teacher>()
                {
                    Description = "Информация о учителе обновлена!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Teacher>()
                {
                    Description = $"[TeacherService.UpdateTeacher] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
