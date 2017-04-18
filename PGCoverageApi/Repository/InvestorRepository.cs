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
    public class InvestorRepository : IInvestorRepository
    {
        private readonly CoverageContext _context;
        ILogger logger;

        public InvestorRepository(CoverageContext context, ILogger _logger)
        {
            _context = context;
            this.logger = _logger;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IEnumerable<Investor> GetAll()
        {
            return _context.InvestorItems.ToList();
        }

        public void Add(Investor item)
        {
            _context.InvestorItems.Add(item);
            _context.SaveChanges();
        }

        public Investor Find(long key)
        {
            return _context.InvestorItems.FirstOrDefault(t => t.InvestorId == key);
        }

        public void Remove(long key)
        {
            var entity = _context.InvestorItems.First(t => t.InvestorId == key);
            _context.InvestorItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Investor item)
        {
            _context.InvestorItems.Update(item);
            _context.SaveChanges();
        }

        public void AddBulk(string connectionString, ICollection<Investor> items, long blockSize = 10000)
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
                    logger.Information("inserting group: {0}", s);

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
            catch(Exception ex)
            {
                logger.Information(ex.ToString());
            }
        }

        public ICollection<string> ListInvestorCodes(string connectionString, int selectionRepCount = 10000, int joinsCount = 1, bool dataInSingleTable = false, bool keepDataAsJson = false)
        {
            var selectInvestor = new StringBuilder();
            IList<string> investorCodes = new List<string>();
            //try
            //{
            //    if (!dataInSingleTable && joinsCount == 1)
            //    {
            //        selectRep.Append("SELECT rep_cd as RepCode FROM \"Coverage\".tbl_rep LIMIT ");
            //        selectRep.Append(selectionRepCount.ToString());
            //    }

            //    if (dataInSingleTable && joinsCount == 1)
            //    {
            //        selectRep.Append("SELECT coverage_data ->> 'rep_cd' as RepCode FROM \"Coverage\".tbl_coverage WHERE coverage_data ? 'rep_cd' LIMIT ");
            //        selectRep.Append(selectionRepCount.ToString());
            //    }

            //    //logger.Information("selectRep : {0}", selectRep.ToString());

            //    using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            //    {
            //        conn.Open();

            //        NpgsqlCommand cmd = new NpgsqlCommand(selectRep.ToString(), conn);

            //        var dr = cmd.ExecuteReader();

            //        while (dr.Read())
            //        {
            //            repCodes.Add((string)dr["RepCode"]);
            //        }

            //        conn.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    logger.Information(ex.ToString());
            //}

            return investorCodes;
        }

        public ICollection<Investor> ListInvestors(string connectionString, ICollection<string> repCodes, int joinsCount = 1, bool dataInSingleTable = false)
        {
            var selectInvestor = new StringBuilder();
            IList<Investor> investors = new List<Investor>();

            //if (!dataInSingleTable && joinsCount == 1)
            //{
            //    selectRep.Append("select rep_id, active_ind, \"BranchId\", cid, last_modified_user, last_modified_utc_dttm, rep_cd, rep_nm, rep_rank_index ");
            //    selectRep.Append("from \"Coverage\".tbl_rep where rep_cd = '{0}';");
            //}

            //if (dataInSingleTable && joinsCount == 1)
            //{
            //    selectRep.Append("SELECT coverage_id as rep_id, (coverage_data->> 'active_ind')::boolean as active_ind, (coverage_data->'parents'->> 'parent_id') :: bigint as BranchId, ");
            //    selectRep.Append("(coverage_data->> 'cid') :: bigint as cid, (coverage_data->> 'last_modified_user') ::bigint as last_modified_user, ");
            //    selectRep.Append("(coverage_data->> 'last_modified_utc_dttm'):: timestamp without time zone as last_modified_utc_dttm, coverage_data->> 'rep_cd' as rep_cd,");
            //    selectRep.Append("coverage_data->> 'rep_nm' as rep_nm, (coverage_data->> 'rep_rank_index') :: numeric as rep_rank_index ");
            //    selectRep.Append("FROM \"Coverage\".tbl_coverage WHERE coverage_data ->> 'rep_cd' = '{0}' AND coverage_data ? 'rep_cd';");
                
            //}
            //try
            //{
            //    int i = 0;
            //    string repStat = string.Empty;
            //    string s = string.Empty;

            //    foreach (var repCode in repCodes)
            //    {
            //        repStat = selectRep.ToString();
            //        s = repStat.Replace("{0}", repCode).ToString();
            //        logger.Information("Count: {0}, {1}", i++.ToString(), s);

            //        using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            //        {
            //            conn.Open();

            //            using (NpgsqlCommand cmd = new NpgsqlCommand(s, conn))
            //            {

            //                //logger.Information("Count: {0}, {1}", i.ToString(), "Executed");

            //                using (NpgsqlDataReader dr = cmd.ExecuteReader())
            //                {
            //                    //logger.Information("Count: {0}, {1}", i.ToString(), "Reading");
            //                    if (dr.Read())
            //                    {
            //                        var rep = new Rep()
            //                        {
            //                            RepId = (long)dr["rep_id"],
            //                            ClientId = (long)dr["cid"],
            //                            RepCode = (string)dr["rep_cd"],
            //                            RepName = (string)dr["rep_nm"],
            //                            RepRankIndex = (decimal)dr["rep_rank_index"],
            //                            ActiveInd = (bool)dr["active_ind"],
            //                            LastModifiedUserId = (long)dr["last_modified_user"],
            //                            LastModifiedUtcDateTime = (DateTime)dr["last_modified_utc_dttm"],
            //                            Branch = new Branch() { BranchId = (long)dr["BranchId"] }
            //                        };

            //                        reps.Add(rep);

            //                    }
            //                }
            //            }

            //            conn.Close();
            //        }
                    
            //    }
            //}
            //catch(Exception ex)
            //{
            //    logger.Information(ex.ToString());
            //}
            return investors;
        }

        public IEnumerable<string> ListInvestorsAsEntities(string connectionString, int selectionRepCount = 10000, int joinsCount = 1, bool dataInSingleTable = false, bool keepDataAsJson = false)
        {
            var selectInvestor = new StringBuilder();
            IList<string> investorCodes = new List<string>();

            //if (!dataInSingleTable && joinsCount == 1)
            //{
            //    selectRep.Append("SELECT rep_cd as RepCode FROM \"Coverage\".tbl_rep LIMIT ");
            //    selectRep.Append(selectionRepCount.ToString());
            //}

            //if (dataInSingleTable && joinsCount == 1)
            //{
            //    selectRep.Append("SELECT coverage_data ->> 'rep_cd' as RepCode FROM \"Coverage\".tbl_coverage WHERE coverage_data ? 'rep_cd' LIMIT ");
            //    selectRep.Append(selectionRepCount.ToString());
            //}

            //logger.Information("selectRep : {0}", selectRep.ToString());

            //using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            //{
            //    conn.Open();

            //    NpgsqlCommand cmd = new NpgsqlCommand(selectRep.ToString(), conn);

            //    var dr = cmd.ExecuteReader();

            //    while (dr.Read())
            //    {
            //        repCodes.Add((string)dr["RepCode"]);
            //    }

            //    conn.Close();
            //}

            return investorCodes;
        }

        private string FetchInsertStatement(IEnumerable<Investor> items)
        {
            StringBuilder fix = new StringBuilder(@"INSERT INTO ""Coverage"".tbl_investor(investor_id, client_id, company_id, entity_cd_id, investor_cd, investor_name, investor_index, active_ind, investor_data) VALUES");

            foreach(Investor i in items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(i.InvestorId.ToString());
                sb.Append(",");
                sb.Append(i.ClientId.ToString());
                sb.Append(",");
                sb.Append(i.CompanyId.ToString());
                sb.Append(",");
                sb.Append(i.EntityCode.EntityCodeId.ToString());
                sb.Append(",'");
                sb.Append(i.InvestorCode);
                sb.Append("','");
                sb.Append(i.InvestorName);
                sb.Append("',");
                sb.Append(i.InvestorIndex.ToString());
                sb.Append(",");
                sb.Append(i.ActiveInd.ToString());
                if (string.IsNullOrWhiteSpace(i.InvestorData))
                {
                    sb.Append(", NULL),");
                }
                else
                {
                    sb.Append(",'");
                    sb.Append(i.InvestorData);
                    sb.Append("'),");
                }
                fix.Append(sb.ToString());
            }

            return fix.ToString().TrimEnd(',') + ";";
        }

    }
}

