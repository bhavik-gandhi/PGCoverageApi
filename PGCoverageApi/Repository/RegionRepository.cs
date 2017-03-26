using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Microsoft.EntityFrameworkCore;

namespace PGCoverageApi.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly CoverageContext _context;

        public RegionRepository(CoverageContext context)
        {
            _context = context;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IEnumerable<Region> GetAll()
        {
            return _context.RegionItems.ToList();
        }

        public void Add(Region item)
        {
            _context.RegionItems.Add(item);
            _context.SaveChanges();
        }

        public Region Find(long key)
        {
            return _context.RegionItems.FirstOrDefault(t => t.RegionId == key);
        }

        public void Remove(long key)
        {
            var entity = _context.RegionItems.First(t => t.RegionId == key);
            _context.RegionItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Region item)
        {
            _context.RegionItems.Update(item);
            _context.SaveChanges();
        }

        public void AddBulk(string connectionString, ICollection<Region> items)
        {


            int i = 0;
            
            _context.RegionItems.AddRange(items);
            _context.SaveChanges();
            //using (CoverageContext contextC = new CoverageContext(_dbContextOption))
            //{

            //    foreach (Region region in items)
            //    {

            //        _context.RegionItems.Add(region);

            //        // this will add max 10 items together
            //        if ((i % 10000) == 0)
            //        {
            //            _context.SaveChanges();
            //            // show some progress to user based on
            //            // value of i
            //        }
            //        i++;
            //    }
            //    _context.SaveChanges();
            //}

        }
    }
}

