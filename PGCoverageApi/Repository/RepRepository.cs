using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace PGCoverageApi.Repository
{
    public class RepRepository : IRepRepository
    {
        private readonly CoverageContext _context;

        public RepRepository(CoverageContext context)
        {
            _context = context;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
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

        public void AddBulk(string connectionString, ICollection<Rep> items)
        {


            int i = 0;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            int blockSize = 10000;
            var group = items.Select((x, index) => new { x, index })
                               .GroupBy(x => x.index / blockSize, y => y.x);

            //foreach (var block in group)
            //{
            //    _context.RepItems.AddRange(block);
            //    _context.SaveChanges();
            //}

            foreach (var block in group)
            {
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                conn.Open();

                // Define a query
                NpgsqlCommand cmd = new NpgsqlCommand(FetchInsertStatement(block), conn);

                // Execute a query
                cmd.ExecuteNonQuery();

                conn.Close();
            }

            //foreach (Rep rep in items)
            //{

            //    _context.RepItems.AddRange(items);
            //    if ((i % 10000) == 0)
            //    {
            //        _context.SaveChanges();
            //        // show some progress to user based on
            //        // value of i
            //    }
            //    i++;
            //}

            //_context.SaveChanges();
        }

        private string FetchInsertStatement(IEnumerable<Rep> items)
        {
            StringBuilder fix = new StringBuilder(@"INSERT INTO ""Coverage"".tbl_rep(rep_id, active_ind, ""BranchId"", cid, last_modified_user, last_modified_utc_dttm, rep_cd, rep_nm, rep_rank_index) VALUES");

            foreach(Rep r in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(r.RepId.ToString());
                sb.Append(",");
                sb.Append(r.ActiveInd.ToString());
                sb.Append(",");
                sb.Append(r.Branch.BranchId.ToString());
                sb.Append(",");
                sb.Append(r.ClientId.ToString());
                sb.Append(",");
                sb.Append(r.LastModifiedUserId.ToString());
                sb.Append(",'");
                sb.Append(r.LastModifiedUtcDateTime.ToString());
                sb.Append("','");
                sb.Append(r.RepCode);
                sb.Append("','");
                sb.Append(r.RepName);
                sb.Append("',");
                sb.Append(r.RepRankIndex.ToString());
                sb.Append("),");
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";
        }
    }
}

