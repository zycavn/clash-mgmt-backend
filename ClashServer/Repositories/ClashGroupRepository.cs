using ClashServer.Contracts;
using ClashServer.Entities;

namespace ClashServer.Repositories
{
    public class ClashGroupRepository : RepositioryBase<ClashGroup>, IClashGroupRepository
    {
        public ClashGroupRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}