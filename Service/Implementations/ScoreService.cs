using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Enum;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Service.Interfaces;

namespace ElectronicJournal.Service.Implementations
{
    public class ScoreService : IScoreService
    {
        private readonly IScoreRepository _scoreRepository;

        public async Task<IBaseResponse<List<Score>>> CreateRangeScore(List<string> scores)
        {
            try
            {
                List<Score> scoresToAdd = new();

                foreach (string score in scores)
                {
                    var scoreInfo = score.Split('/');

                    var chekScore = scoresToAdd.Find(score => score.IdStudent == int.Parse(scoreInfo[0]));

                    if (chekScore != null)
                        scoresToAdd.Remove(chekScore);

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
    }
}
