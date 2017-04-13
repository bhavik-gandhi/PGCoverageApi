using System.Text;
using System.Collections.Generic;
using System.Linq;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Npgsql;
using Serilog;

namespace PGCoverageApi.Repository
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly CoverageContext _context;
        ILogger logger;

        public ChannelRepository(CoverageContext context, ILogger _logger)
        {
            _context = context;
            this.logger = _logger;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IEnumerable<Channel> GetAll()
        {
            return _context.ChannelItems.ToList();
        }

        public void Add(Channel item)
        {
            _context.ChannelItems.Add(item);
            _context.SaveChanges();
        }

        public Channel Find(long key)
        {
            return _context.ChannelItems.FirstOrDefault(t => t.ChannelId == key);
        }

        public void Remove(long key)
        {
            var entity = _context.ChannelItems.First(t => t.ChannelId == key);
            _context.ChannelItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Channel item)
        {
            _context.ChannelItems.Update(item);
            _context.SaveChanges();
        }

        public void AddBulk(string connectionString, ICollection<Channel> items, bool storeDataAsJson = false, bool dataInSingleTable = false, int blockSize = 10000)
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
                logger.Information("inserting channels: {0}", s);

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

        private string FetchInsertStatement(IEnumerable<Channel> items)
        {
            StringBuilder fix = new StringBuilder(@"INSERT INTO ""Coverage"".tbl_channel(channel_id, active_ind, channel_cd, channel_nm, cid, last_modified_user, last_modified_utc_dttm) VALUES");

            foreach (Channel i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.ChannelId.ToString());
                sb.Append(",");
                sb.Append(i.ActiveInd.ToString());
                sb.Append(",'");
                sb.Append(i.ChannelCode);
                sb.Append("','");
                sb.Append(i.ChannelName);
                sb.Append("',");
                sb.Append(i.ClientId.ToString());
                sb.Append(",");
                sb.Append(i.LastModifiedUserId.ToString());
                sb.Append(",'");
                sb.Append(i.LastModifiedUtcDateTime.ToString());
                sb.Append("'),");
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";
        }

        private string FetchInsertStatementAsJson(IEnumerable<Channel> items, bool dataInSingleTable)
        {

            StringBuilder fix = new StringBuilder();
            if (!dataInSingleTable)
            {
                fix.Append(@"INSERT INTO ""Coverage"".tbl_channel(channel_id, channel_data) VALUES");
            }
            else
            {
                fix.Append(@"INSERT INTO ""Coverage"".tbl_coverage(coverage_id, coverage_data) VALUES");
            }

            foreach (Channel i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.ChannelId.ToString());
                sb.Append(",'{\"active_ind\":");
                sb.Append(i.ActiveInd.ToString().ToLower());
                sb.Append(",");
                sb.Append("\"channel_cd\":");
                sb.Append("\"");
                sb.Append(i.ChannelCode);
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"channel_nm\":");
                sb.Append("\"");
                sb.Append(i.ChannelName);
                sb.Append("\"");
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
                sb.Append("),");
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";

        }
            
    }
}

