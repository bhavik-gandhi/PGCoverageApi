using Microsoft.EntityFrameworkCore;
using PGCoverageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGCoverageApi.DataContext
{
    public class RegionContext : DbContext
    {
        public RegionContext(DbContextOptions<ChannelContext> options) : base(options)
        {

        }

        public DbSet<Region> RegionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Coverage");

            base.OnModelCreating(builder);
        }
    }
}
