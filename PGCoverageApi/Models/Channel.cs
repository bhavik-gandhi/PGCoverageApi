using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGCoverageApi.Models
{
    [Table("tbl_channel")]
    public class Channel
    {
        [Key]
        [Column("channel_id")]
        public long ChannelId { get; set; }

        [Column("cid")]
        public long ClientId { get; set; }

        [Column("channel_cd")]
        public string ChannelCode { get; set; }

        [Column("channel_nm")]
        public string ChannelName { get; set; }

        [Column("active_ind")]
        public bool ActiveInd { get; set; }

        [Column("last_modified_user")]
        public long LastModifiedUserId { get; set; }

        [Column("last_modified_utc_dttm")]
        public DateTime LastModifiedUtcDateTime{ get; set; }

        public ICollection<Region> Regions { get; set; }
    }
}
