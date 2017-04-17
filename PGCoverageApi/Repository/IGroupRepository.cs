using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IGroupRepository
    {
        void Add(Group item);
        IEnumerable<Group> GetAll();
        Group Find(long key);
        void Remove(long key);
        void Update(Group item);

        void AddBulk(string connectionString, ICollection<Group> items, bool storeDataAsJson, bool dataInSingleTable, int blockSize = 10000);
        ICollection<string> ListGroupCodes(string connectionString, int selectionRepCount = 10000, int joinsCount = 1, bool dataInSingleTable = false, bool keepDataAsJson = false);
        ICollection<Group> ListGroups(string connectionString, ICollection<string> repCodes, int joinsCount = 1, bool dataInSingleTable = false);
    }
}
