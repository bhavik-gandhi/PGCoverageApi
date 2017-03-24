using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IRepRepository
    {
        void Add(Rep item);
        IEnumerable<Rep> GetAll();
        Rep Find(long key);
        void Remove(long key);
        void Update(Rep item);

        void AddBulk(ICollection<Rep> items);
    }
}
