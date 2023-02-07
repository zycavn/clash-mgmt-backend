using ClashServer.Contracts;
using ClashServer.Entities;

namespace ClashServer.Repositories
{
    public class ProjectRepository : RepositioryBase<Project>, IProjectRepository
    {
        public ProjectRepository(RepositoryContext context) : base(context)
        {
        }
    }
}