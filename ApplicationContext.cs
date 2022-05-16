using Microsoft.EntityFrameworkCore;
using System;

namespace MySQLApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=Mindvan@2802;database=mysql;",
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}