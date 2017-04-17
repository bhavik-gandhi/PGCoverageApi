using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGCoverageApi.Models
{
    [Table("tbl_investor_relation")]
    public class InvestorRelation
    {
        [Key]
        [Column("investor_relation_id")]
        public long InvestorRelationId { get; set; }

        [Column("investor_id")]
        public Investor Investor { get; set; }

        [Column("investor_parent_id")]
        public Investor InvestorParent { get; set; }

        [Column("investor_relation_data")]
        public string InvestorRelationData { get; set; }
       
    }
}

