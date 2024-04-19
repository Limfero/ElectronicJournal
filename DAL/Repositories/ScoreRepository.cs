using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;

namespace ElectronicJournal.DAL.Repositories
{
    public class ScoreRepository : BaseRepository<Score>, IScoreRepository
    {
        public ScoreRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
