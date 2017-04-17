using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGCoverageApi.Models
{
    [Table("tbl_investor")]
    public class Investor
    {
        [Key]
        [Column("investor_id")]
        public long InvestorId { get; set; }

        [Column("client_id")]
        public long ClientId { get; set; }

        [Column("company_id")]
        public long CompanyId { get; set; }

        [Column("investor_cd")]
        public string InvestorCode{ get; set; }

        [Column("investor_name")]
        public string InvestorName { get; set; }

        [Column("investor_index")]
        public decimal InvestorIndex { get; set; }

        [Column("active_ind")]
        public bool ActiveInd { get; set; }

        [Column("investor_data")]
        public string InvestorData { get; set; }

        public EntityCode EntityCode{ get; set; }

    }
}

