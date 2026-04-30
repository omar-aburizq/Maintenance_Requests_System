namespace Application.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task InsertAsync(T input);
        public Task InsertRange(List<T> input);
        public IQueryable<T> GetAll();
        public Task<T> GetByIdAsync(Guid id);
        public void Update(T input);
        public void Delete(T input);
        public Task SaveChangesAsync();
    }
}
