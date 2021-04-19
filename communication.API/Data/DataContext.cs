
using communication.API.Models;
using Microsoft.EntityFrameworkCore;

namespace communication.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<Value> Values { get; set; }
        public virtual DbSet<User> Users { get; set; }






    }
}