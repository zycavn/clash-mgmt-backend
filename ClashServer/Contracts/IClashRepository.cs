using ClashServer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClashServer.Contracts
{
    public interface IClashRepository : IRepositoryBase<Clash>
    {
        Task<IEnumerable<Clash>> GetClashByClashGroupCode(string clashGroupCode);

        Task UpdateClash(Clash clash);
    }
}