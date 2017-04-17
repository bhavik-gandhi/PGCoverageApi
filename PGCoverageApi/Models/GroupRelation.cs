using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGCoverageApi.Models
{
    [Table("tbl_group_relation")]
    public class GroupRelation
    {
        [Key]
        [Column("group_relation_id")]
        public long GroupRelationId { get; set; }

        [Column("group_id")]
        public Group Group { get; set; }

        [Column("group_parent_id")]
        public Group GroupParent { get; set; }

        [Column("group_relation_data")]
        public string GroupRelationData { get; set; }
       
    }
}
