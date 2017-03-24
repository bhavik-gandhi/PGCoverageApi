﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public class RepRepository : IRepRepository
    {
        private readonly CoverageContext _context;

        public RepRepository(CoverageContext context)
        {
            _context = context;
        }

        public IEnumerable<Rep> GetAll()
        {
            return _context.RepItems.ToList();
        }

        public void Add(Rep item)
        {
            _context.RepItems.Add(item);
            _context.SaveChanges();
        }

        public Rep Find(long key)
        {
            return _context.RepItems.FirstOrDefault(t => t.RepId == key);
        }

        public void Remove(long key)
        {
            var entity = _context.RepItems.First(t => t.RepId == key);
            _context.RepItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Rep item)
        {
            _context.RepItems.Update(item);
            _context.SaveChanges();
        }
    }
}
