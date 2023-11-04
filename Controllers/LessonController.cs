using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet("{idClass}/{date}")]
        public async Task<Dictionary<DateOnly, List<Lesson>>> GetSchedule(DateOnly date, int idClass)
        {
            var response = await _lessonService.GetLessonsOfDateAndClass(date, idClass);
            return response.Data;
        }

        [HttpGet]
        public List<Lesson> GetAll()
        {
            return _lessonService.GetAllLessons().Data;
        }
    }
}
