using System.Text;
using System.Collections.Generic;
using System.Linq;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Npgsql;
using Serilog;

namespace PGCoverageApi.Repository
{
    public class InvestorRelationRepository : IInvestorRelationRepository
    {
        private readonly CoverageContext _context;
        ILogger logger;

        public InvestorRelationRepository(CoverageContext context, ILogger _logger)
        {
            _context = context;
            this.logger = _logger;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IEnumerable<InvestorRelation> GetAll()
        {
            return _context.InvestorRelationItems.ToList();
        }

        public void Add(InvestorRelation item)
        {
            _context.InvestorRelationItems.Add(item);
            _context.SaveChanges();
        }

        public InvestorRelation Find(long key)
        {
            return _context.InvestorRelationItems.FirstOrDefault(t => t.InvestorRelationId == key);
        }

        public void Remove(long key)
        {
            var entity = _context.InvestorRelationItems.First(t => t.InvestorRelationId == key);
            _context.InvestorRelationItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(InvestorRelation item)
        {
            _context.InvestorRelationItems.Update(item);
            _context.SaveChanges();
        }

        public void AddBulk(string connectionString, ICollection<InvestorRelation> items, long blockSize = 10000)
        {

            var group = items.Select((x, index) => new { x, index })
                               .GroupBy(x => x.index / blockSize, y => y.x);

            IList<string> insertStatements = new List<string>();

            
            foreach (var block in group)
            {
                insertStatements.Add(FetchInsertStatement(block));
            }

            foreach (string s in insertStatements)
            {
                logger.Information("inserting investor relation code: {0}", s);

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

        private string FetchInsertStatement(IEnumerable<InvestorRelation> items)
        {
            StringBuilder fix = new StringBuilder(@"INSERT INTO ""Coverage"".tbl_investor_relation(investor_relation_id, investor_id, investor_parent_id, investor_relation_data) VALUES");
            
            foreach (InvestorRelation i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.InvestorRelationId.ToString());
                sb.Append(",");
                sb.Append(i.Investor.InvestorId.ToString());
                sb.Append(",");
                sb.Append(i.InvestorParent.InvestorId.ToString());
                if (string.IsNullOrWhiteSpace(i.InvestorRelationData))
                {
                    sb.Append(", NULL),");
                }
                else
                {
                    sb.Append(",'");
                    sb.Append(i.InvestorRelationData);
                    sb.Append("'),");
                }
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";
        }

            
    }
}

