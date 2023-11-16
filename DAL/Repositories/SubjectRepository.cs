using ElectronicJournal.DAL.Interfaces;
using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.DAL.Repositories
{
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async override Task<Subject> CreateAsync(Subject entity)
        {
            var teachers = entity.Teachers;
            entity.Teachers = new();

            _dbContext.Subjects.Add(entity);

            entity.Teachers = teachers;

            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public override IQueryable<Subject> GetAll()
        {
            return _dbContext.Subjects.Include(s => s.Teachers);
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            return await _dbContext.Subjects.FirstOrDefaultAsync(subject => subject.Id == id);
        }
    }
}
