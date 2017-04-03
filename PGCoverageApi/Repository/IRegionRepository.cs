using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IRegionRepository
    {
        void Add(Region item);
        IEnumerable<Region> GetAll();
        Region Find(long key);
        void Remove(long key);
        void Update(Region item);

        void AddBulk(string connectionString, ICollection<Region> items, bool storeDataAsJson, bool dataInSingleTable, int blockSize = 10000);
    }
}
