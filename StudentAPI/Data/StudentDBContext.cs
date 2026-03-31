using Microsoft.EntityFrameworkCore;
using StudentAPI.Model;

namespace StudentAPI.Data
{
    public class StudentDBContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public StudentDBContext(DbContextOptions<StudentDBContext> options) : base(options) { }
    }
}
