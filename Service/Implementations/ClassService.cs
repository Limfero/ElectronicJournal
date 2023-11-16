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
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
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
                        StatusCode = StatusCode.ClassNotFound
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

        public async Task<IBaseResponse<Class>> CreateClass(ClassViewModel model)
        {
            try
            {
                var entity = await _classRepository.GetAll().FirstOrDefaultAsync(c => c.Name == model.Name);

                if (entity != null)
                    return new BaseResponse<Class>
                    {
                        Description = "Класс с таким названием уже есть",
                        StatusCode = StatusCode.ClassNotCreated
                    };

                var @class = new Class()
                {
                    Name = model.Name,
                    Lessons = new(),
                    Students = new(),
                };

                var response = await _classRepository.CreateAsync(@class);

                return new BaseResponse<Class>()
                {
                    Data = response,
                    Description = "Класс был создан",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Class>()
                {
                    Description = $"[ClassSerivce.CreateClass] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Class>> DeleteClass(int id)
        {
            try
            {
                var @class = await _classRepository.GetByIdAsync(id);

                if (@class == null)
                    return new BaseResponse<Class>()
                    {
                        Description = "Такого класса нет!",
                        StatusCode = StatusCode.ClassNotFound
                    };

                await _classRepository.RemoveAsync(@class);

                return new BaseResponse<Class>()
                {
                    Description = "Класс был удален",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Class>()
                {
                    Description = $"[ClassService.DeleteClass] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Class>> UpdateClass(int id, ClassViewModel model)
        {
            try
            {
                var @class = await _classRepository.GetByIdAsync(id);

                if (@class == null)
                    return new BaseResponse<Class>()
                    {
                        Description = "Такого класса нет!",
                        StatusCode = StatusCode.SubjectNotFound
                    };

                @class.Name = model.Name;

                await _classRepository.UpdateAsync(@class);

                return new BaseResponse<Class>()
                {
                    Description = "Информация о классе обновлена!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Class>()
                {
                    Description = $"[ClassService.Class] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
