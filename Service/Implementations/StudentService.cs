using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Helpers;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;
using ElectronicJournal.Service.Interfaces;
using System.Reflection;

namespace ElectronicJournal.Service.Implementations
{
    public class StudentService : IStudentService
    {
        private static string s_currentDirectory = Assembly.GetExecutingAssembly().Location;

        private readonly IStudentRepository _studentRepository;
        private readonly string _path = s_currentDirectory[0..s_currentDirectory.IndexOf("ElectronicJournal")] + "ElectronicJournal\\ClientApp\\public\\image\\";

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IBaseResponse<Student>> CreateStudent(StudentViewModel model)
        {
            try
            {
                var image = model.Image;

                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)image.Length);
                }
                byte[] picture = imageData;

                File.WriteAllBytes(_path + $"{model.Login}.png", picture);

                var student = new Student()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Login = model.Login,
                    MiddleName = model.MiddleName,
                    ImagePath = $"image/{model.Login}.png",
                    Password = HashPasswordHelper.HashPassword(model.Password),
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

                foreach (var student in students)
                {
                    student.Class.Students = new();
                    student.Scores.ForEach(score => score.Student = null);
                }

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

                student.Class.Students = new();
                student.Scores.ForEach(score => 
                {
                    score.Student = null;
                    score.Lesson.Scores = new();
                    score.Lesson.Class = new();
                    score.Lesson.Subject.Lessons = new();
                });

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
