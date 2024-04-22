using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScoresController : ControllerBase
    {
        private readonly IScoreService _scoreService;

        public ScoresController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpGet]
        [Route("getScores")]
        public List<Score> GetAll()
        {
            return _scoreService.GetAllScores().Data;
        }

        [HttpGet]
        [Route("getScoresByIdStudent/{id}")]
        public List<Score> GetScoresByIdStudent(int id)
        {
            return _scoreService.GetAllScores().Data.Where(score => score.IdStudent == id).ToList();
        }

        [HttpPost]
        [Route("createScores")]
        public async Task<IActionResult> Create(string[] model)
        {
            var response = await _scoreService.CreateRangeScore(model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(new { description = response.Description });

            return BadRequest(new { description = response.Description });
        }
    }
}
