using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.DAL.Repositories
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async override Task<Teacher> CreateAsync(Teacher entity)
        {
            var subjects = entity.Subjects;
            entity.Subjects = new();

            _dbContext.Teachers.Add(entity);

            entity.Subjects.AddRange(subjects);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public override IQueryable<Teacher> GetAll()
        {
            return _dbContext.Teachers.Include(t => t.Subjects);
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            return await _dbContext.Teachers.FirstOrDefaultAsync(teacher => teacher.Id == id);
        }
    }
}
