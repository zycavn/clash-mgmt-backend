using ClashServer.Contracts;
using ClashServer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClashServer.Repositories
{
    public class ClashRepository : RepositioryBase<Clash>, IClashRepository
    {
        private RepositoryContext _repositoryContext;

        public ClashRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<Clash>> GetClashByClashGroupCode(string clashGroupCode)
        {
            var clashes = await (from c in _repositoryContext.Clashes
                                 join g in _repositoryContext.ClashGroups
                                   on c.ClashGroupId equals g.Id
                                where g.ClashCode.Equals(clashGroupCode)
                               select c).ToListAsync();

            return clashes;
        }

        public async Task UpdateClash(Clash clash)
        {
            var entry = _repositoryContext.Clashes.First(c => c.Id.Equals(clash.Id));

            entry.Status = clash.Status;
            entry.AssignTo = clash.AssignTo;
            entry.GridLocation = clash.GridLocation;
            entry.Description = clash.Description;
            entry.DateFound = clash.DateFound;
            entry.ClashPoint = clash.ClashPoint;
            entry.Distance = clash.Distance;
            entry.ElementId1 = clash.ElementId1;
            entry.ElementId2 = clash.ElementId2;
            entry.Layer1 = clash.Layer1;
            entry.Layer2 = clash.Layer2;
            entry.ItemName1 = clash.ItemName1;
            entry.ItemName2 = clash.ItemName2;
            entry.ItemType1 = clash.ItemType1;
            entry.ItemType2 = clash.ItemType2;
            entry.ItemPath1 = clash.ItemPath1;
            entry.ItemPath2 = clash.ItemPath2;
            entry.ClashImagePath = clash.ClashImagePath;

            _repositoryContext.Entry(entry).CurrentValues.SetValues(entry);
            await _repositoryContext.SaveChangesAsync();
        }
    }
}