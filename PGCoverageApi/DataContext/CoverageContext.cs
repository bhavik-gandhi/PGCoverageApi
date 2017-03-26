using Microsoft.EntityFrameworkCore;
using PGCoverageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGCoverageApi.DataContext
{
    public class CoverageContext : DbContext
    {
        public CoverageContext(DbContextOptions<CoverageContext> options) : base(options)
        {

        }

        public DbSet<Channel> ChannelItems { get; set; }
        public DbSet<Region> RegionItems { get; set; }
        public DbSet<Branch> BranchItems { get; set; }
        public DbSet<Rep> RepItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Coverage");

            base.OnModelCreating(builder);
        }
    }
}
