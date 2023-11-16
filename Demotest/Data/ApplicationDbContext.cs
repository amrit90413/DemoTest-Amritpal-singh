using Demotest.Models;
using Microsoft.EntityFrameworkCore;

namespace Demotest.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Countries> Countries { get; set; }
        public DbSet<States> States { get; set; }

    }
}
