namespace Application.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T input);
        Task InsertRangeAsync(List<T> input);
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(Guid id);
        void Update(T input);
        void Delete(T input);
        Task SaveChangesAsync();
    }
}
