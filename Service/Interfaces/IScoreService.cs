using ElectronicJournal.Domain.Entity;
using ElectronicJournal.Domain.Response;

namespace ElectronicJournal.Service.Interfaces
{
    public interface IScoreService
    {
        Task<IBaseResponse<List<Score>>> CreateRangeScore(string[] scores);

        IBaseResponse<List<Score>> GetAllScores();
    }
}
