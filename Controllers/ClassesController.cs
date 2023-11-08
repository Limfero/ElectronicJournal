using Microsoft.AspNetCore.Mvc;
using ElectronicJournal.DAL;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Service.Interfaces;

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

        [HttpGet]
        [Route("getClass/{id}")]
        public async Task<ActionResult<Class>> Get(int id)
        {
            var response = await _classService.GetClassById(id);

            return response.Data;
        }
    }
}
