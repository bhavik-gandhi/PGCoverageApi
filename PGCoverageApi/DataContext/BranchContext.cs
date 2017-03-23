using Microsoft.EntityFrameworkCore;
using PGCoverageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGCoverageApi.DataContext
{
    public class BranchContext : DbContext
    {
        public BranchContext(DbContextOptions<ChannelContext> options) : base(options)
        {

        }

        public DbSet<Branch> BranchItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Coverage");

            base.OnModelCreating(builder);
        }
    }
}
