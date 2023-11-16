using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.DAL.Repositories
{
    public class ClassRepository : BaseRepository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Class> GetByIdAsync(int id)
        {
            return await _dbContext.Classes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public override IQueryable<Class> GetAll()
        {
            return _dbContext.Classes
                .Include(c => c.Students)
                .Include(c => c.Lessons);
        }
    }
}
