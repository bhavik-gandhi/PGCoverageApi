﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IBranchRepository
    {
        void Add(Branch item);
        IEnumerable<Branch> GetAll();
        Branch Find(long key);
        void Remove(long key);
        void Update(Branch item);
    }
}