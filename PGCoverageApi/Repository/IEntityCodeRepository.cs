using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IEntityCodeRepository
    {
        void Add(EntityCode item);
        IEnumerable<EntityCode> GetAll();
        EntityCode Find(long key);
        void Remove(long key);
        void Update(EntityCode item);

        void AddBulk(string connectionString, ICollection<EntityCode> items, bool storeDataAsJson, bool dataInSingleTable, int blockSize = 10000);

    }
}
