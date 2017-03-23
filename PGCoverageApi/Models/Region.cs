using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGCoverageApi.Models
{
    [Table("tbl_region")]
    public class Region
    {
        [Key]
        [Column("region_id")]
        public long RegionId { get; set; }

        [Column("cid")]
        public long ClientId { get; set; }

        [Column("region_cd")]
        public string RegionCode { get; set; }

        [Column("region_nm")]
        public string RegionName { get; set; }

        [Column("region_rank_index")]
        public decimal RegionRankIndex { get; set; }

        [Column("active_ind")]
        public bool ActiveInd { get; set; }

        [Column("last_modified_user")]
        public long LastModifiedUserId { get; set; }

        [Column("last_modified_utc_dttm")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        public Channel Channel { get; set; }

        public ICollection<Branch> Branches{ get; set; }
    }
}
