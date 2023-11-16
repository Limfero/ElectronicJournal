using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.ViewModels;
using ElectronicJournal.Service.Implementations;
using ElectronicJournal.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Route("getStudents")]
        public List<Student> GetAll()
        {
            return _studentService.GetAllStudent().Data;
        }

        [HttpGet]
        [Route("getStudent/{id}")]
        public async Task<Student> GetAll(int id)
        {
            var response = await _studentService.GetStudentById(id);

            return response.Data;
        }

        [HttpPost]
        [Route("createStudent")]
        public async Task<IActionResult> Create(StudentViewModel model)
        {
            var response = await _studentService.CreateStudent(model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }

        [HttpDelete]
        [Route("deleteStudent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _studentService.DeleteStudent(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }

        [HttpPatch]
        [Route("updateStudent/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentViewModel model)
        {
            var response = await _studentService.UpdateStudent(id, model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }
    }
}
