using ClashServer.Contracts;
using ClashServer.Entities;

namespace ClashServer.Repositories
{
    public class StatusRepository : RepositioryBase<Status>, IStatusRepository
    {
        public StatusRepository(RepositoryContext context) : base(context)
        {
        }
    }
}