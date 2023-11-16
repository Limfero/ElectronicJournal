using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;
using ElectronicJournal.Service.Interfaces;

namespace ElectronicJournal.Service.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IBaseResponse<Student>> CreateStudent(StudentViewModel model)
        {
            try
            {
                var student = new Student()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Login = model.Login,
                    MiddleName = model.MiddleName,
                    Password = model.Password,
                    IdClass = model.IdClass
                };

                var response = await _studentRepository.CreateAsync(student);

                return new BaseResponse<Student>()
                {
                    Data = response,
                    Description = "Студент был создан",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Student>()
                {
                    Description = $"[StudentService.CreateStudent] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Student>> DeleteStudent(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);

                if (student == null)
                    return new BaseResponse<Student>()
                    {
                        Description = "Такого студента нет!",
                        StatusCode = StatusCode.SubjectNotFound
                    };

                var response = await _studentRepository.RemoveAsync(student);

                return new BaseResponse<Student>()
                {
                    Data = response,
                    Description = $"Студент был успешно удален!",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Student>()
                {
                    Description = $"[StudentService.DeleteStudent] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Student>> GetAllStudent()
        {
            try
            {
                var students = _studentRepository.GetAll().ToList();

                if (students == null)
                {
                    return new BaseResponse<List<Student>>()
                    {
                        Description = "Данные не найдены",
                        StatusCode = StatusCode.StudentNotFound
                    };
                }

                return new BaseResponse<List<Student>>()
                {
                    Data = students,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Student>>()
                {
                    Description = $"[StudentService.GetAllStudent] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Student>> GetStudentById(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);

                if (student == null)
                    return new BaseResponse<Student>()
                    {
                        Description = "Такого студента нет!",
                        StatusCode = StatusCode.StudentNotFound
                    };

                return new BaseResponse<Student>()
                {
                    Data = student,
                    Description = "Студент найден!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Student>()
                {
                    Description = $"[StudentService.UpdateStudent] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Student>> UpdateStudent(int id, StudentViewModel model)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);

                if (student == null)
                    return new BaseResponse<Student>()
                    {
                        Description = "Такого студента нет!",
                        StatusCode = StatusCode.StudentNotFound
                    };

                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.MiddleName = model.MiddleName;
                student.IdClass = model.IdClass;
                student.Password = model.Password;

                await _studentRepository.UpdateAsync(student);

                return new BaseResponse<Student>()
                {
                    Description = "Информация о студенте обновлена!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Student>()
                {
                    Description = $"[StudentService.UpdateStudent] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
