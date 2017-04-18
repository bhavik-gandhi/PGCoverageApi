using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IInvestorRelationRepository
    {
        void Add(InvestorRelation item);
        IEnumerable<InvestorRelation> GetAll();
        InvestorRelation Find(long key);
        void Remove(long key);
        void Update(InvestorRelation item);

        void AddBulk(string connectionString, ICollection<InvestorRelation> items, bool storeDataAsJson, bool dataInSingleTable, long blockSize = 10000);

    }
}
