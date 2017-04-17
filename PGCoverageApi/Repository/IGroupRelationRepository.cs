using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IGroupRelationRepository
    {
        void Add(GroupRelation item);
        IEnumerable<GroupRelation> GetAll();
        GroupRelation Find(long key);
        void Remove(long key);
        void Update(GroupRelation item);

        void AddBulk(string connectionString, ICollection<GroupRelation> items, bool storeDataAsJson, bool dataInSingleTable, int blockSize = 10000);

    }
}
