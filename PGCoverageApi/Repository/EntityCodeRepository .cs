using System.Text;
using System.Collections.Generic;
using System.Linq;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;
using Npgsql;
using Serilog;

namespace PGCoverageApi.Repository
{
    public class EntityCodeRepository : IEntityCodeRepository
    {
        private readonly CoverageContext _context;
        ILogger logger;

        public EntityCodeRepository(CoverageContext context, ILogger _logger)
        {
            _context = context;
            this.logger = _logger;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IEnumerable<EntityCode> GetAll()
        {
            return _context.EntityCodeItems.ToList();
        }

        public void Add(EntityCode item)
        {
            _context.EntityCodeItems.Add(item);
            _context.SaveChanges();
        }

        public EntityCode Find(long key)
        {
            return _context.EntityCodeItems.FirstOrDefault(t => t.EntityCodeId == key);
        }

        public void Remove(long key)
        {
            var entity = _context.EntityCodeItems.First(t => t.EntityCodeId == key);
            _context.EntityCodeItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(EntityCode item)
        {
            _context.EntityCodeItems.Update(item);
            _context.SaveChanges();
        }

        public void AddBulk(string connectionString, ICollection<EntityCode> items, bool storeDataAsJson = false, bool dataInSingleTable = false, int blockSize = 10000)
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

            foreach (string s in insertStatements)
            {
                logger.Information("inserting entity code: {0}", s);

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

        private string FetchInsertStatement(IEnumerable<EntityCode> items)
        {
            StringBuilder fix = new StringBuilder(@"INSERT INTO ""Coverage"".tbl_entity_code(entity_cd_id, entity_cd, entity_cd_name, entity_cd_type, active_ind) VALUES");
            
            foreach (EntityCode i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.EntityCodeId.ToString());
                sb.Append(",'");
                sb.Append(i.EntityCd);
                sb.Append("','");
                sb.Append(i.EntityCodeName);
                sb.Append("','");
                sb.Append(i.EntityCodeType);
                sb.Append("',");
                sb.Append(i.ActiveInd.ToString());
                sb.Append("),");
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";
        }

            
    }
}

