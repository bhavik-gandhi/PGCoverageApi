using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IChannelRepository
    {
        void Add(Channel item);
        IEnumerable<Channel> GetAll();
        Channel Find(long key);
        void Remove(long key);
        void Update(Channel item);

        void AddBulk(string connectionString, ICollection<Channel> items, bool storeDataAsJson, bool dataInSingleTable, int blockSize = 10000);

    }
}
