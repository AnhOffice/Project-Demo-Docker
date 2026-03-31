namespace StudentAPI.Repository
{
    public interface ICommonRepository<T, TKey> where T : class
    {
        IQueryable<T> GetAllCanLinQ();
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(TKey id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(TKey id);
    }
}
