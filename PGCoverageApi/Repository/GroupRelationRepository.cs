using System.Text;
using System.Collections.Generic;
using System.Linq;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Npgsql;
using Serilog;
using System;

namespace PGCoverageApi.Repository
{
    public class GroupRelationRepository : IGroupRelationRepository
    {
        private readonly CoverageContext _context;
        ILogger logger;

        public GroupRelationRepository(CoverageContext context, ILogger _logger)
        {
            _context = context;
            this.logger = _logger;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IEnumerable<GroupRelation> GetAll()
        {
            return _context.GroupRelationItems.ToList();
        }

        public void Add(GroupRelation item)
        {
            _context.GroupRelationItems.Add(item);
            _context.SaveChanges();
        }

        public GroupRelation Find(long key)
        {
            return _context.GroupRelationItems.FirstOrDefault(t => t.GroupRelationId == key);
        }

        public void Remove(long key)
        {
            var entity = _context.GroupRelationItems.First(t => t.GroupRelationId == key);
            _context.GroupRelationItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(GroupRelation item)
        {
            _context.GroupRelationItems.Update(item);
            _context.SaveChanges();
        }

        public void AddBulk(string connectionString, ICollection<GroupRelation> items, long blockSize = 10000)
        {

            var group = items.Select((x, index) => new { x, index })
                               .GroupBy(x => x.index / blockSize, y => y.x);

            try
            {
                IList<string> insertStatements = new List<string>();

                foreach (var block in group)
                {
                    insertStatements.Add(FetchInsertStatement(block));
                }


                foreach (string s in insertStatements)
                {
                    logger.Information("inserting group relation: {0}", s);

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
            catch (Exception ex)
            {
                logger.Information(ex.ToString());
            }
        }

        private string FetchInsertStatement(IEnumerable<GroupRelation> items)
        {
            StringBuilder fix = new StringBuilder(@"INSERT INTO ""Coverage"".tbl_group_relation(group_relation_id, group_id, group_parent_id, group_relation_data) VALUES");
            
            foreach (GroupRelation i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.GroupRelationId.ToString());
                sb.Append(",");
                sb.Append(i.Group.GroupId.ToString());
                if (i.GroupParent == null)
                {
                    sb.Append(",NULL");
                }
                else
                {
                    sb.Append(",");
                    sb.Append(i.GroupParent.GroupId.ToString());
                }
                if (string.IsNullOrWhiteSpace(i.GroupRelationData))
                {
                    sb.Append(", NULL),");
                }
                else
                {
                    sb.Append(",'");
                    sb.Append(i.GroupRelationData);
                    sb.Append("'),");
                }
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";
        }

            
    }
}

