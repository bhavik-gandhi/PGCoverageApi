using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGCoverageApi.Models
{

    [Table("tbl_branch")]
    public class Branch
    {
        [Key]
        [Column("branch_id")]
        public long BranchId { get; set; }

        [Column("cid")]
        public long ClientId { get; set; }

        [Column("branch_cd")]
        public string BranchCode { get; set; }

        [Column("branch_nm")]
        public string BranchName { get; set; }

        [Column("branch_rank_index")]
        public decimal BranchRankIndex { get; set; }

        [Column("active_ind")]
        public bool ActiveInd { get; set; }

        [Column("last_modified_user")]
        public long LastModifiedUserId { get; set; }

        [Column("last_modified_utc_dttm")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        public Region Region{ get; set; }

        public ICollection<Rep> Branches { get; set; }
    }
}
