using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.DAL.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        protected readonly ApplicationDbContext _dbContext;

        public LessonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Lesson> CreateAsync(Lesson entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public IQueryable<Lesson> GetAll()
        {
            return _dbContext.Set<Lesson>();
        }

        public async Task<Lesson> RemoveAsync(Lesson entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Lesson> UpdateAsync(Lesson entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Lesson> GetByIdAsync(int id)
        {
            return await _dbContext.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == id);
        }

        public async Task<List<Lesson>> GetByClassAndDateAsync(int idClass, DateOnly date)
        {
            return await _dbContext.Lessons.Where(lesson => lesson.Class.Id == idClass)
                                         .Where(lesson => lesson.Date == date)
                                         .ToListAsync();
        }
    }
}
