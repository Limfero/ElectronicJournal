namespace ElectronicJournal.DAL.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> RemoveAsync(TEntity entity);

        Task<List<TEntity>> CreateRangeAsync(List<TEntity> entity);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
