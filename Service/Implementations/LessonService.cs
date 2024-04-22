using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;
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
                    {
                        lesson.Subject.Lessons = new();
                        lesson.Teacher.Lessons = new();
                        lesson.Class.Lessons = new();
                        lesson.Scores.ForEach(score => { score.Lesson = new(); });

                        weeklyLessons[lesson.Date].Add(lesson);
                    }
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

        public async Task<IBaseResponse<List<Lesson>>> CreateRangeLessons(LessonViewModel model)
        {
            try
            {
                DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
                int dayInWeek = 7;
                List<Lesson> lessonsToAdd = new();
                var dateModel = model.Date;

                var timeIntervalsOfLesson = _lessonRepository.GetAll()
                    .Where(lesson => lesson.IdClass == model.IdClass)
                    .Where(lesson => lesson.Date == model.Date)
                    .ToDictionary(lesson => lesson.StartTime, lesson => lesson.EndTime);

                do
                {
                    var lesson = new Lesson()
                    {
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        IdClass = model.IdClass,
                        Date = dateModel,
                        ClassRoom = model.ClassRoom,
                        Description = model.Description,
                        IdSubject = model.IdSubject,
                        IdTeacher = model.IdTeacher
                    };

                    foreach (var timeInterval in timeIntervalsOfLesson)
                        if (lesson.StartTime.IsBetween(timeInterval.Key, timeInterval.Value) && lesson.EndTime.IsBetween(timeInterval.Key, timeInterval.Value) || lesson.StartTime == timeInterval.Key)
                            continue;

                    lessonsToAdd.Add(lesson);
                    dateModel = dateModel.AddDays(dayInWeek);
                }
                while (dateModel <= model.UntilWhatDate);

                var response = await _lessonRepository.CreateRangeAsync(lessonsToAdd);

                if (response.FirstOrDefault().Class == null)
                {
                    return new BaseResponse<List<Lesson>>()
                    {
                        Description = $"Урок не был создан: {response.FirstOrDefault().Description}",
                        StatusCode = StatusCode.LessonNotCreated
                    };
                }

                return new BaseResponse<List<Lesson>>()
                {
                    Description = "Уроки были созданы",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Lesson>>()
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
