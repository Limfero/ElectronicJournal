using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels.Lesson;
using ElectronicJournal.Service.Interfaces;
using System.Globalization;

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
                    var response = await _lessonRepository.GetByClassAndDateAsync(idClass, day);

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
                    Description = $"[LessonService.GetLessonsOfDateAndClass] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
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
                    Description = $"[LessonService.GetLessonsOfDateAndClass] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Lesson>> CreateLesson(LessonViewModel model)
        {
            try
            {
                var timeIntervalsOfLesson = _lessonRepository.GetAll()
                    .Where(lesson => lesson.IdClass == model.IdClass)
                    .Where(lesson => lesson.Date == model.Date)
                    .ToDictionary(lesson => lesson.StartTime, lesson => lesson.EndTime);

                foreach (var timeInterval in timeIntervalsOfLesson)
                {
                    if (model.StartTime >= timeInterval.Key && model.StartTime <= timeInterval.Value)
                    {
                        return new BaseResponse<Lesson>()
                        {
                            Description = "В это время уже идет урок",
                            StatusCode = StatusCode.TimeIsBusy
                        };
                    }
                }

                var lesson = new Lesson()
                {
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    IdClass = model.IdClass,
                    Date = model.Date,
                    ClassRoom = model.ClassRoom,
                    Description = model.Description,
                    IdSubject = model.IdSubject,
                    IdTeacher = model.IdTeacher
                };

                var responseLesson = await _lessonRepository.CreateAsync(lesson);

                if (responseLesson.Class == null)
                {
                    return new BaseResponse<Lesson>()
                    {
                        Description = $"Урок не был создан: {responseLesson.Description}",
                        StatusCode = StatusCode.LessonNotCreated
                    };
                } 

                return new BaseResponse<Lesson>()
                {
                    Description = "Урок был создан",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Lesson>()
                {
                    Description = $"[LessonService.CreateLesson] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
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
