using System.Threading.Tasks;

namespace ClashServer.Contracts
{
    public interface IRepositoryWrapper
    {
        IProjectRepository Project { get; }
        IClashRepository Clash { get; }
        IClashGroupRepository ClashGroup { get; }
        ICommentRepository Comment { get; }
        IStatusRepository Status { get; }

        void Save();

        Task SaveAsync();
    }
}