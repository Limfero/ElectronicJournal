using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.DAL.Repositories;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Service.Interfaces;

namespace ElectronicJournal.Service.Implementations
{
    public class ScoreService : IScoreService
    {
        private readonly IScoreRepository _scoreRepository;

        public ScoreService(IScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        public async Task<IBaseResponse<List<Score>>> CreateRangeScore(string[] scores)
        {
            try
            {
                List<Score> scoresToAdd = new();

                foreach (string score in scores)
                {
                    var scoreInfo = score.Split('/');

                    scoresToAdd.Add(new Score()
                    {
                        IdStudent = int.Parse(scoreInfo[0]),
                        IdLesson = int.Parse(scoreInfo[1]),
                        Grade = (Grade)int.Parse(scoreInfo[1])
                    });
                }

                var response = await _scoreRepository.CreateRangeAsync(scoresToAdd);             

                return new BaseResponse<List<Score>>()
                {
                    Description = "Оценки были созданы",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Score>>()
                {
                    Description = $"[ScoreService.CreateRangeScore] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Score>> GetAllScores()
        {
            try
            {
                var response = _scoreRepository.GetAll().ToList();

                foreach (var score in response)
                {
                    score.Student.Scores = new();
                }

                if (response == null)
                {
                    return new BaseResponse<List<Score>>()
                    {
                        Description = "Данные не найдены",
                        StatusCode = StatusCode.ClassNotFound
                    };
                }

                return new BaseResponse<List<Score>>()
                {
                    Data = response,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Score>>()
                {
                    Description = $"[ScoreService.GetAllScores] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
