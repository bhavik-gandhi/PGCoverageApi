using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly CoverageContext _context;

        public BranchRepository(CoverageContext context)
        {
            _context = context;
        }

        public IEnumerable<Branch> GetAll()
        {
            return _context.BranchItems.ToList();
        }

        public void Add(Branch item)
        {
            _context.BranchItems.Add(item);
            _context.SaveChanges();
        }

        public Branch Find(long key)
        {
            return _context.BranchItems.FirstOrDefault(t => t.BranchId == key);
        }

        public void Remove(long key)
        {
            var entity = _context.BranchItems.First(t => t.BranchId == key);
            _context.BranchItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Branch item)
        {
            _context.BranchItems.Update(item);
            _context.SaveChanges();
        }
    }
}

