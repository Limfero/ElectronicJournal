using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Service.Interfaces;

namespace ElectronicJournal.Service.Implementations
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<IBaseResponse<Class>> GetClassById(int id)
        {
            try
            {
                var response = await _classRepository.GetByIdAsync(id);

                if (response == null)
                {
                    return new BaseResponse<Class>()
                    {
                        Description = "Данные не найдены",
                        StatusCode = StatusCode.ClassNotFound
                    };
                }

                return new BaseResponse<Class>()
                {
                    Data = response,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Class>()
                {
                    Description = $"[ClassService.GetAllClasses] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Class>> GetAllClasses()
        {
            try
            {
                var response = _classRepository.GetAll().ToList();

                if (response == null)
                {
                    return new BaseResponse<List<Class>>()
                    {
                        Description = "Данные не найдены",
                        StatusCode = StatusCode.LessonNotFound
                    };
                }

                return new BaseResponse<List<Class>>()
                {
                    Data = response,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Class>>()
                {
                    Description = $"[ClassService.GetAllClasses] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
