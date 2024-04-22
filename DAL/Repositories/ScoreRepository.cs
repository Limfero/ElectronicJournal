using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.DAL.Repositories
{
    public class ScoreRepository : BaseRepository<Score>, IScoreRepository
    {
        public ScoreRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override IQueryable<Score> GetAll()
        {
            return _dbContext.Scores
                .Include(score => score.Lesson);
        }
    }
}
