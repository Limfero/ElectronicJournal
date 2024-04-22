using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.ViewModels;
using ElectronicJournal.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [Route("getSubjects")]
        public List<Subject> GetAll()
        {
            return _subjectService.GetAllSubjects().Data;
        }

        [HttpPost]
        [Route("createSubject")]
        public async Task<IActionResult> Create(SubjectViewModel model)
        {
            var response = await _subjectService.CreateSubject(model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }

        [HttpDelete]
        [Route("deleteSubject/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _subjectService.DeleteSubject(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }

        [HttpPatch]
        [Route("updateSubject/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SubjectViewModel model)
        {
            var response = await _subjectService.UpdateSubject(id, model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }
    }
}
