using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;

namespace Store.DAL
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Criteria> Criterias { get; set; }
        public DbSet<CriteriaItem> CriteriaItems { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<Movie> Movies { get; set; }
        //public DbSet<Person> Persons { get; set; }
        //public DbSet<Rate> Rates { get; set; }
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<Tag> Tags { get; set; }
        //public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>().Property(x => x.Value).HasColumnType("decimal(12,10)");
            modelBuilder.Entity<Item>().Property(x => x.Cost).HasColumnType("decimal(12,10)");
            modelBuilder.Entity<Article>().Property(x => x.Cost).HasColumnType("decimal(12,10)");
            //modelBuilder.Entity<Role>().HasKey(x => new { x.UserId, x.RoleName });
        }
    }
}