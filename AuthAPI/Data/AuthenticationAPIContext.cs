using AuthAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class AuthenticationAPIContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AuthenticationAPIContext(DbContextOptions<AuthenticationAPIContext> options) : base(options) { }
    }
}
