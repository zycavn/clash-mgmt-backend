using ClashServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClashServer.Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Clash> Clashes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Status> States { get; set; }
        public DbSet<ClashGroup> ClashGroups { get; set; }
    }
}