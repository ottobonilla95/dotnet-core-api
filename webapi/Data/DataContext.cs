using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<MenuItemRelation> MenuItemRelation { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
