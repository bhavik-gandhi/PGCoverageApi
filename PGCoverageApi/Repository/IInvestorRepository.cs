using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IInvestorRepository
    {
        void Add(Investor item);
        IEnumerable<Investor> GetAll();
        Investor Find(long key);
        void Remove(long key);
        void Update(Investor item);

        void AddBulk(string connectionString, ICollection<Investor> items, long blockSize = 10000);
        ICollection<string> ListInvestorCodes(string connectionString, int selectionRepCount = 10000, int joinsCount = 1, bool dataInSingleTable = false, bool keepDataAsJson = false);
        ICollection<Investor> ListInvestors(string connectionString, ICollection<string> repCodes, int joinsCount = 1, bool dataInSingleTable = false);
    }
}
