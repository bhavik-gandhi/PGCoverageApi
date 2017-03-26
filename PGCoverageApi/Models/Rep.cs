using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGCoverageApi.Models
{
    [Table("tbl_rep")]
    public class Rep
    {
        [Key]
        [Column("rep_id")]
        public long RepId { get; set; }

        [Column("cid")]
        public long ClientId { get; set; }

        [Column("rep_cd")]
        public string RepCode{ get; set; }

        [Column("rep_nm")]
        public string RepName { get; set; }

        [Column("rep_rank_index")]
        public decimal RepRankIndex { get; set; }

        [Column("active_ind")]
        public bool ActiveInd { get; set; }

        [Column("last_modified_user")]
        public long LastModifiedUserId { get; set; }

        [Column("last_modified_utc_dttm")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        public Branch Branch{ get; set; }

    }
}
