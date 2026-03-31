using AuthAPI.Data;
using AuthAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthenticationAPIContext _context;

        public AuthRepository(AuthenticationAPIContext context)
        {
            _context = context;
        }

        public IQueryable<Account> GetAll()
        {
            return _context.Accounts.AsQueryable();
        }

        public async Task<Account?> GetByIdAsync(Guid id)
            => await _context.Accounts.FindAsync(id);

        public async Task AddAsync(Account student)
        {
            await _context.Accounts.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account student)
        {
            _context.Accounts.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Account student)
        {
            _context.Accounts.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
