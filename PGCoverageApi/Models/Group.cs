using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGCoverageApi.Models
{
    [Table("tbl_group")]
    public class Group
    {
        [Key]
        [Column("group_id")]
        public long GroupId { get; set; }

        [Column("client_id")]
        public long ClientId { get; set; }

        [Column("company_id")]
        public long CompanyId { get; set; }

        [Column("group_cd")]
        public string GroupCode{ get; set; }

        [Column("group_name")]
        public string GroupName { get; set; }

        [Column("group_index")]
        public decimal GroupIndex { get; set; }

        [Column("active_ind")]
        public bool ActiveInd { get; set; }

        [Column("group_data")]
        public string GroupData { get; set; }

        public EntityCode EntityCode{ get; set; }

    }
}
