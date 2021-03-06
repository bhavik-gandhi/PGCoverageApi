﻿using System;
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

        void AddBulk(string connectionString, ICollection<Rep> items, bool storeDataAsJson, bool dataInSingleTable, int blockSize = 10000);
        ICollection<string> ListRepCodes(string connectionString, int selectionRepCount = 10000, int joinsCount = 1, bool dataInSingleTable = false, bool keepDataAsJson = false);
        ICollection<Rep> ListReps(string connectionString, ICollection<string> repCodes, int joinsCount = 1, bool dataInSingleTable = false);
    }
}
