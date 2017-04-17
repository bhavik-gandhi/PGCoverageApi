using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGCoverageApi.Models
{
    [Table("tbl_entity_code")]
    public class EntityCode
    {
        [Key]
        [Column("entity_cd_id")]
        public long EntityCodeId { get; set; }

        [Column("entity_cd")]
        public string EntityCd { get; set; }

        [Column("entity_cd_name")]
        public string EntityCodeName { get; set; }

        [Column("entity_cd_type")]
        public string EntityCodeType { get; set; }

        [Column("active_ind")]
        public bool ActiveInd { get; set; }
       
    }
}
