using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.DAL.Repositories
{
    public class LessonRepository : BaseRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Lesson> GetByIdAsync(int id)
        {
            return await _dbContext.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == id);
        }

        public async Task<List<Lesson>> GetByClassAndDateAsync(int idClass, DateOnly date)
        {
            return await _dbContext.Lessons.Include(lesson => lesson.Scores)
                .Include(lesson => lesson.Teacher)
                .Include(lesson => lesson.Class)
                .Include(lesson => lesson.Subject)
                .Where(lesson => lesson.Class.Id == idClass)
                .Where(lesson => lesson.Date == date)
                .ToListAsync();
        }

        public override IQueryable<Lesson> GetAll()
        {
            return _dbContext.Lessons
                .Include(lesson => lesson.Scores)
                .Include(lesson => lesson.Teacher)
                .Include(lesson => lesson.Class)
                .Include(lesson => lesson.Subject);      
        }
    }
}
