using Microsoft.AspNetCore.Mvc;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Service.Interfaces;
using ElectronicJournal.Domain.ViewModels;

namespace ElectronicJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        [Route("getClasses")]
        public List<Class> GetAll()
        {
            return _classService.GetAllClasses().Data;
        }

        [HttpPost]
        [Route("createClass")]
        public async Task<Class> Create(ClassViewModel model)
        {
            var response = await _classService.CreateClass(model);
            return response.Data;
        }

        [HttpDelete]
        [Route("deleteClass/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _classService.DeleteClass(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }

        [HttpPatch]
        [Route("updateClass/{id}")]
        public async Task<IActionResult> Update(ClassViewModel model, int id)
        {
            var response = await _classService.UpdateClass(id, model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }
    }
}
