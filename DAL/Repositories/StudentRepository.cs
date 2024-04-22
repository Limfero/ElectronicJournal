using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.DAL.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _dbContext.Students
                .Include(student => student.Class)
                .Include(student => student.Scores)
                .ThenInclude(score => score.Lesson)
                .ThenInclude(lesson => lesson.Subject)
                .FirstOrDefaultAsync(student => student.Id == id);
        }

        public override IQueryable<Student> GetAll()
        {
            return _dbContext.Students
                .Include(student => student.Scores)
                .Include(student => student.Class);
        }
    }
}
