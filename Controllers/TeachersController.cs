using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.ViewModels;
using ElectronicJournal.Service.Implementations;
using ElectronicJournal.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        [Route("getTeachers")]
        public IEnumerable<Teacher> GetAll()
        {
            return _teacherService.GetAllTeachers().Data;
        }

        [HttpGet]
        [Route("getTeacher/{id}")]
        public async Task<Teacher> GetTeacher(int id)
        {
            var response = await _teacherService.GetTeacherById(id);

            return response.Data;
        }

        [HttpPost]
        [Route("createTeacher")]
        public async Task<IActionResult> Create(TeacherViewModel model)
        {
            var response = await _teacherService.CreateTeacher(model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }

        [HttpDelete]
        [Route("deleteTeacher/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _teacherService.DeleteTeacher(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }

        [HttpPatch]
        [Route("updateTeacher/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeacherViewModel model)
        {
            var response = await _teacherService.UpdateTeacher(id, model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }
    }
}
