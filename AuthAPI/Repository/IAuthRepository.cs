using AuthAPI.Model;

namespace AuthAPI.Repository
{
    public interface IAuthRepository
    {
        IQueryable<Account> GetAll();
        //Task<int> GetCountAsync();  //Khi nào có Odata thì sử dụng
        Task<Account?> GetByIdAsync(Guid id);
        Task AddAsync(Account item);
        Task UpdateAsync(Account item);
        Task DeleteAsync(Account item);

        Task<Account?> GetByUsernameAsync(string name); //Đặc trưng
        Task<Account?> GetByEmailAsync(string email);
    }
}
