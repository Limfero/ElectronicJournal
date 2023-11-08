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

        public override async Task<Lesson> CreateAsync(Lesson entity)
        {
            try
            {
                _dbContext.Teachers.FirstOrDefault(teacher => teacher.Id == entity.IdTeacher).Lessons.Add(entity);
                _dbContext.Classes.FirstOrDefault(c => c.Id == entity.IdClass).Lessons.Add(entity);
                _dbContext.Subjects.FirstOrDefault(subject => subject.Id == entity.IdSubject).Lessons.Add(entity);

                entity.Teacher = _dbContext.Teachers.FirstOrDefault(teacher => teacher.Id == entity.IdTeacher);
                entity.Subject = _dbContext.Subjects.FirstOrDefault(subject => subject.Id == entity.IdSubject);
                entity.Class = _dbContext.Classes.FirstOrDefault(c => c.Id == entity.IdClass);

                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new Lesson() { Description = ex.Message };
            }

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
