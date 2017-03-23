﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public interface IRepRepository
    {
        void Add(Channel item);
        IEnumerable<Channel> GetAll();
        Channel Find(long key);
        void Remove(long key);
        void Update(Channel item);
    }
}
