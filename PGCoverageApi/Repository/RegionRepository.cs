using System.Text;
using System.Collections.Generic;
using System.Linq;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Npgsql;
using Serilog;

namespace PGCoverageApi.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly CoverageContext _context;
        ILogger logger;

        public RegionRepository(CoverageContext context, ILogger _logger)
        {
            this.logger = _logger;
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

        public void AddBulk(string connectionString, ICollection<Region> items, bool storeDataAsJson = false, bool dataInSingleTable = false, int blockSize = 10000)
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
                logger.Information("inserting regions: {0}", s);

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

        private string FetchInsertStatement(IEnumerable<Region> items)
        {
            StringBuilder fix = new StringBuilder(@"INSERT INTO ""Coverage"".tbl_region(region_id, active_ind, ""ChannelId"", cid, last_modified_user, last_modified_utc_dttm, region_cd, region_nm, region_rank_index) VALUES");

            foreach (Region i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.RegionId.ToString());
                sb.Append(",");
                sb.Append(i.ActiveInd.ToString());
                sb.Append(",");
                sb.Append(i.Channel.ChannelId.ToString());
                sb.Append(",");
                sb.Append(i.ClientId.ToString());
                sb.Append(",");
                sb.Append(i.LastModifiedUserId.ToString());
                sb.Append(",'");
                sb.Append(i.LastModifiedUtcDateTime.ToString());
                sb.Append("','");
                sb.Append(i.RegionCode);
                sb.Append("','");
                sb.Append(i.RegionName);
                sb.Append("',");
                sb.Append(i.RegionRankIndex);
                sb.Append("),");
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";
        }

        private string FetchInsertStatementAsJson(IEnumerable<Region> items, bool dataInSingleTable)
        {

            StringBuilder fix = new StringBuilder();

            if (!dataInSingleTable)
            {
                fix.Append(@"INSERT INTO ""Coverage"".tbl_region(region_id, region_data) VALUES");
            }
            else
            {
                fix.Append(@"INSERT INTO ""Coverage"".tbl_coverage(coverage_id, coverage_data, coverage_relation) VALUES");
            }

            foreach (Region i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.RegionId.ToString());
                sb.Append(",'{\"active_ind\":");
                sb.Append(i.ActiveInd.ToString().ToLower());
                sb.Append(",");
                sb.Append("\"region_cd\":");
                sb.Append("\"");
                sb.Append(i.RegionCode);
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"region_nm\":");
                sb.Append("\"");
                sb.Append(i.RegionName);
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"region_rank_index\":");
                sb.Append(i.RegionRankIndex.ToString());
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
                sb.Append("}'");
                sb.Append(",'{");
                sb.Append("\"companies\":[");
                sb.Append("{");
                sb.Append("\"company_id\":");
                sb.Append("\"CompanyID-1\"");
                sb.Append("}]");
                sb.Append(",");
                sb.Append("\"parents\":[");
                sb.Append("{");
                sb.Append("\"parent_id\":");
                sb.Append(i.Channel.ChannelId);
                sb.Append("}]");
                sb.Append("}'),");
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";

        }

    }
}

