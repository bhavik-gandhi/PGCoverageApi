using System.Text;
using System.Collections.Generic;
using System.Linq;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Npgsql;
using Serilog;


namespace PGCoverageApi.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly CoverageContext _context;
        ILogger logger;

        public BranchRepository(CoverageContext context, ILogger logger)
        {
            _context = context;
            this.logger = logger;
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

        public void AddBulk(string connectionString, ICollection<Branch> items, bool storeDataAsJson = false, bool dataInSingleTable = false, int blockSize = 10000)
        {

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
                //logger.Information("inserting branches: {0}", s);

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(s, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

            }
        }

        private string FetchInsertStatement(IEnumerable<Branch> items)
        {
            StringBuilder fix = new StringBuilder(@"INSERT INTO ""Coverage"".tbl_branch(branch_id, active_ind, ""RegionId"", cid, last_modified_user, last_modified_utc_dttm, branch_cd, branch_nm, branch_rank_index) VALUES");

            foreach (Branch i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.BranchId.ToString());
                sb.Append(",");
                sb.Append(i.ActiveInd.ToString());
                sb.Append(",");
                sb.Append(i.Region.RegionId.ToString());
                sb.Append(",");
                sb.Append(i.ClientId.ToString());
                sb.Append(",");
                sb.Append(i.LastModifiedUserId.ToString());
                sb.Append(",'");
                sb.Append(i.LastModifiedUtcDateTime.ToString());
                sb.Append("','");
                sb.Append(i.BranchCode);
                sb.Append("','");
                sb.Append(i.BranchName);
                sb.Append("',");
                sb.Append(i.BranchRankIndex);
                sb.Append("),");
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";
        }

        private string FetchInsertStatementAsJson(IEnumerable<Branch> items, bool dataInSingleTable)
        {
            StringBuilder fix = new StringBuilder();

            if (!dataInSingleTable)
            {
                fix.Append(@"INSERT INTO ""Coverage"".tbl_branch(branch_id, branch_data) VALUES");
            }
            else
            {
                fix.Append(@"INSERT INTO ""Coverage"".tbl_coverage(coverage_id, coverage_data) VALUES");
            }


            foreach (Branch i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.BranchId.ToString());
                sb.Append(",'{\"active_ind\":");
                sb.Append(i.ActiveInd.ToString().ToLower());
                sb.Append(",");
                sb.Append("\"branch_cd\":");
                sb.Append("\"");
                sb.Append(i.BranchCode);
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"branch_nm\":");
                sb.Append("\"");
                sb.Append(i.BranchName);
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"branch_rank_index\":");
                sb.Append(i.BranchRankIndex.ToString());
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
                sb.Append(i.Region.RegionId);
                sb.Append("}");
                sb.Append("}'");
                sb.Append("),");
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";

        }

    }
}

