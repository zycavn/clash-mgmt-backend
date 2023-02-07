using ClashServer.Contracts;
using ClashServer.Entities;

namespace ClashServer.Repositories
{
    public class CommentRepository : RepositioryBase<Comment>, ICommentRepository
    {
        public CommentRepository(RepositoryContext context) : base(context)
        {
        }
    }
}