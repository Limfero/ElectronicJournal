using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Service.Interfaces;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectronicJournal.Service.Implementations
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<IBaseResponse<Dictionary<DateOnly, List<Lesson>>>> GetLessonsOfDateAndClass(DateOnly date, int idClass)
        {
            try
            {
                var daysOfWeek = GetCurrentWeekByDay(date);

                var weeklyLessons = CreateWeeklyLessonDictionary(daysOfWeek);

                foreach (var day in daysOfWeek)
                {
                    var response = await _lessonRepository.GetByClassAndDateAsync(idClass, date);

                    foreach (var lesson in response)
                        weeklyLessons[lesson.Date].Add(lesson);
                }

                return new BaseResponse<Dictionary<DateOnly, List<Lesson>>>()
                {
                    Data = weeklyLessons,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<DateOnly, List<Lesson>>>()
                {
                    Description = $"[GetLessonsOfDateAndClass] - {ex.Message}",
                    StatusCode = StatusCode.IternalServerError
                };
            }
        }

        public IBaseResponse<List<Lesson>> GetAllLessons()
        {
            try
            {
                var response = _lessonRepository.GetAll().ToList();

                if(response == null)
                {
                    return new BaseResponse<List<Lesson>>()
                    {
                        Description = "Данные не найдены",
                        StatusCode = StatusCode.LessonNotFound
                    };
                }

                return new BaseResponse<List<Lesson>>()
                {
                    Data = response,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Lesson>>()
                {
                    Description = $"[GetLessonsOfDateAndClass] - {ex.Message}",
                    StatusCode = StatusCode.IternalServerError
                };
            }
        }

        private static IEnumerable<DateOnly> GetCurrentWeekByDay(DateOnly date)
        {
            var countDaysInWeek = 6;
            var dateValue = date;
            var culture = CultureInfo.CurrentCulture;
            var weekOffset = culture.DateTimeFormat.FirstDayOfWeek - dateValue.DayOfWeek;
            var startOfWeek = dateValue.AddDays(weekOffset);

            return Enumerable.Range(0, countDaysInWeek).Select(i => startOfWeek.AddDays(i));
        }

        private static Dictionary<DateOnly, List<Lesson>> CreateWeeklyLessonDictionary(IEnumerable<DateOnly> daysOfWeek)
        {
            Dictionary<DateOnly, List<Lesson>> weeklyLessons = new();

            foreach (var day in daysOfWeek)
            {
                if (!weeklyLessons.ContainsKey(day))
                    weeklyLessons.Add(day, new List<Lesson>());
            }

            return weeklyLessons;
        }
    }
}
