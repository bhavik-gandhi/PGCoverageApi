using System.Text;
using System.Collections.Generic;
using System.Linq;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Npgsql;
using Serilog;

namespace PGCoverageApi.Repository
{
    public class RepRepository : IRepRepository
    {
        private readonly CoverageContext _context;
        ILogger logger;

        public RepRepository(CoverageContext context, ILogger _logger)
        {
            _context = context;
            this.logger = _logger;
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

        public void AddBulk(string connectionString, ICollection<Rep> items, bool storeDataAsJson = false, bool dataInSingleTable = false)
        {
            int blockSize = 10000;
            var group = items.Select((x, index) => new { x, index })
                               .GroupBy(x => x.index / blockSize, y => y.x);

            IList<string> insertStatements = new List<string>();

            if (!storeDataAsJson)
            {
                foreach (var block in group)
                {
                    insertStatements.Add(FetchInsertStatement(block));
                }
            }
            else
            {
                foreach (var block in group)
                {
                    insertStatements.Add(FetchInsertStatementAsJson(block, dataInSingleTable));
                }
            }

            foreach (string s in insertStatements)
            {
                //logger.Information("inserting regions: {0}", s);

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(s, conn);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }

            }
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

        private string FetchInsertStatementAsJson(IEnumerable<Rep> items, bool dataInSingleTable)
        {
            StringBuilder fix = new StringBuilder();
            if (!dataInSingleTable)
            {
                fix.Append(@"INSERT INTO ""Coverage"".tbl_rep(rep_id, rep_data) VALUES");
            }
            else
            {
                fix.Append(@"INSERT INTO ""Coverage"".tbl_coverage(coverage_id, coverage_data) VALUES");
            }

            foreach (Rep i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.RepId.ToString());
                sb.Append(",'{\"active_ind\":");
                sb.Append(i.ActiveInd.ToString().ToLower());
                sb.Append(",");
                sb.Append("\"rep_cd\":");
                sb.Append("\"");
                sb.Append(i.RepCode);
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"rep_nm\":");
                sb.Append("\"");
                sb.Append(i.RepName);
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"rep_rank_index\":");
                sb.Append(i.RepRankIndex.ToString());
                sb.Append(",");
                sb.Append("\"cid\":");
                sb.Append(i.ClientId.ToString());
                sb.Append(",");
                sb.Append("\"last_modified_user\":");
                sb.Append(i.LastModifiedUserId.ToString());
                sb.Append(",");
                sb.Append("\"last_modified_utc_dttm\":");
                sb.Append("\"");
                sb.Append(i.LastModifiedUtcDateTime.ToString());
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"parents\":");
                sb.Append("{");
                sb.Append("\"parent_id\":");
                sb.Append(i.Branch.BranchId);
                sb.Append("}");
                sb.Append("}'");
                sb.Append("),");
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";

        }
    }
}

