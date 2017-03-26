using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Microsoft.EntityFrameworkCore;

namespace PGCoverageApi.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly CoverageContext _context;

        public BranchRepository(CoverageContext context)
        {
            _context = context;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
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

        public void AddBulk(string connectionString, ICollection<Branch> items)
        {


            int i = 0;
                _context.BranchItems.AddRange(items);
            
            //foreach (Branch branch in items)
            //{

            //    _context.BranchItems.Add(branch);

            //    // this will add max 10 items together
            //    if ((i % 10000) == 0)
            //    {
            //        _context.SaveChanges();
            //        // show some progress to user based on
            //        // value of i
            //    }
            //    i++;
            //}
            _context.SaveChanges();
            }
        
    }
}

