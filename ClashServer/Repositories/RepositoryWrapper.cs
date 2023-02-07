using System.Threading.Tasks;
using ClashServer.Contracts;

namespace ClashServer.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly RepositoryContext _repositoryContext;
        private IProjectRepository _project;
        private IClashGroupRepository _clashGroup;
        private IClashRepository _clash;
        private ICommentRepository _comment;
        private IStatusRepository _status;

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IProjectRepository Project
        {
            get
            {
                if (_project == null) {
                    _project = new ProjectRepository(_repositoryContext);
                }
                return _project;
            }
        }

        public IClashGroupRepository ClashGroup
        {
            get
            {
                if (_clashGroup == null) {
                    _clashGroup = new ClashGroupRepository(_repositoryContext);
                }
                return _clashGroup;
            }
        }

        public IClashRepository Clash
        {
            get
            {
                if (_clash == null) {
                    _clash = new ClashRepository(_repositoryContext);
                }
                return _clash;
            }
        }

        public ICommentRepository Comment
        {
            get
            {
                if (_comment == null) {
                    _comment = new CommentRepository(_repositoryContext);
                }
                return _comment;
            }
        }

        public IStatusRepository Status
        {
            get
            {
                if (_status == null) {
                    _status = new StatusRepository(_repositoryContext);
                }
                return _status;
            }
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
            return;
        }
    }
}