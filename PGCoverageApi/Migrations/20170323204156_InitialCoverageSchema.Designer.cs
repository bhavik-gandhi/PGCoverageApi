using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PGCoverageApi.DataContext;

namespace PGCoverageApi.Migrations
{
    [DbContext(typeof(CoverageContext))]
    [Migration("20170323204156_InitialCoverageSchema")]
    partial class InitialCoverageSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("Coverage")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("PGCoverageApi.Models.Branch", b =>
                {
                    b.Property<long>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("branch_id");

                    b.Property<bool>("ActiveInd")
                        .HasColumnName("active_ind");

                    b.Property<string>("BranchCode")
                        .HasColumnName("branch_cd");

                    b.Property<string>("BranchName")
                        .HasColumnName("branch_nm");

                    b.Property<decimal>("BranchRankIndex")
                        .HasColumnName("branch_rank_index");

                    b.Property<long>("ClientId")
                        .HasColumnName("cid");

                    b.Property<long>("LastModifiedUserId")
                        .HasColumnName("last_modified_user");

                    b.Property<DateTime>("LastModifiedUtcDateTime")
                        .HasColumnName("last_modified_utc_dttm");

                    b.Property<long?>("RegionId");

                    b.HasKey("BranchId");

                    b.HasIndex("RegionId");

                    b.ToTable("tbl_branch");
                });

            modelBuilder.Entity("PGCoverageApi.Models.Channel", b =>
                {
                    b.Property<long>("ChannelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("channel_id");

                    b.Property<bool>("ActiveInd")
                        .HasColumnName("active_ind");

                    b.Property<string>("ChannelCode")
                        .HasColumnName("channel_cd");

                    b.Property<string>("ChannelName")
                        .HasColumnName("channel_nm");

                    b.Property<long>("ClientId")
                        .HasColumnName("cid");

                    b.Property<long>("LastModifiedUserId")
                        .HasColumnName("last_modified_user");

                    b.Property<DateTime>("LastModifiedUtcDateTime")
                        .HasColumnName("last_modified_utc_dttm");

                    b.HasKey("ChannelId");

                    b.ToTable("tbl_channel");
                });

            modelBuilder.Entity("PGCoverageApi.Models.Region", b =>
                {
                    b.Property<long>("RegionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("region_id");

                    b.Property<bool>("ActiveInd")
                        .HasColumnName("active_ind");

                    b.Property<long?>("ChannelId");

                    b.Property<long>("ClientId")
                        .HasColumnName("cid");

                    b.Property<long>("LastModifiedUserId")
                        .HasColumnName("last_modified_user");

                    b.Property<DateTime>("LastModifiedUtcDateTime")
                        .HasColumnName("last_modified_utc_dttm");

                    b.Property<string>("RegionCode")
                        .HasColumnName("region_cd");

                    b.Property<string>("RegionName")
                        .HasColumnName("region_nm");

                    b.Property<decimal>("RegionRankIndex")
                        .HasColumnName("region_rank_index");

                    b.HasKey("RegionId");

                    b.HasIndex("ChannelId");

                    b.ToTable("tbl_region");
                });

            modelBuilder.Entity("PGCoverageApi.Models.Rep", b =>
                {
                    b.Property<long>("RepId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("rep_id");

                    b.Property<bool>("ActiveInd")
                        .HasColumnName("active_ind");

                    b.Property<long?>("BranchId");

                    b.Property<long>("ClientId")
                        .HasColumnName("cid");

                    b.Property<long>("LastModifiedUserId")
                        .HasColumnName("last_modified_user");

                    b.Property<DateTime>("LastModifiedUtcDateTime")
                        .HasColumnName("last_modified_utc_dttm");

                    b.Property<string>("RepCode")
                        .HasColumnName("rep_cd");

                    b.Property<string>("RepName")
                        .HasColumnName("rep_nm");

                    b.Property<decimal>("RepRankIndex")
                        .HasColumnName("rep_rank_index");

                    b.HasKey("RepId");

                    b.HasIndex("BranchId");

                    b.ToTable("tbl_rep");
                });

            modelBuilder.Entity("PGCoverageApi.Models.Branch", b =>
                {
                    b.HasOne("PGCoverageApi.Models.Region", "Region")
                        .WithMany("Branches")
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("PGCoverageApi.Models.Region", b =>
                {
                    b.HasOne("PGCoverageApi.Models.Channel", "Channel")
                        .WithMany("Regions")
                        .HasForeignKey("ChannelId");
                });

            modelBuilder.Entity("PGCoverageApi.Models.Rep", b =>
                {
                    b.HasOne("PGCoverageApi.Models.Branch", "Branch")
                        .WithMany("Branches")
                        .HasForeignKey("BranchId");
                });
        }
    }
}
