using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.ViewModels.Lesson;
using ElectronicJournal.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        [Route("schedule/{date}/{idClass}")]
        public async Task<Dictionary<DateOnly, List<Lesson>>> GetSchedule(DateOnly date, int idClass)
        {
            var response = await _lessonService.GetLessonsOfDateAndClass(date, idClass);
            return response.Data;
        }

        [HttpGet]
        [Route("listLessons")]
        public List<Lesson> GetAll()
        {
            return _lessonService.GetAllLessons().Data;
        }

        [HttpPost]
        [Route("createLesson")]
        public async Task<IActionResult> Create(LessonViewModel model)
        {
            var response = await _lessonService.CreateLesson(model);

            if(response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new {description = response.Description});

            return BadRequest(new { description = response.Data.Description });
        }
    }
}
