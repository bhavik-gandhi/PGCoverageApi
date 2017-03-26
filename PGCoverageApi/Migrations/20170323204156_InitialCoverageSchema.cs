using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PGCoverageApi.Migrations
{
    public partial class InitialCoverageSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Coverage");

            migrationBuilder.CreateTable(
                name: "tbl_channel",
                schema: "Coverage",
                columns: table => new
                {
                    channel_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    active_ind = table.Column<bool>(nullable: false),
                    channel_cd = table.Column<string>(nullable: true),
                    channel_nm = table.Column<string>(nullable: true),
                    cid = table.Column<long>(nullable: false),
                    last_modified_user = table.Column<long>(nullable: false),
                    last_modified_utc_dttm = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_channel", x => x.channel_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_region",
                schema: "Coverage",
                columns: table => new
                {
                    region_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    active_ind = table.Column<bool>(nullable: false),
                    ChannelId = table.Column<long>(nullable: true),
                    cid = table.Column<long>(nullable: false),
                    last_modified_user = table.Column<long>(nullable: false),
                    last_modified_utc_dttm = table.Column<DateTime>(nullable: false),
                    region_cd = table.Column<string>(nullable: true),
                    region_nm = table.Column<string>(nullable: true),
                    region_rank_index = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_region", x => x.region_id);
                    table.ForeignKey(
                        name: "FK_tbl_region_tbl_channel_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "Coverage",
                        principalTable: "tbl_channel",
                        principalColumn: "channel_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_branch",
                schema: "Coverage",
                columns: table => new
                {
                    branch_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    active_ind = table.Column<bool>(nullable: false),
                    branch_cd = table.Column<string>(nullable: true),
                    branch_nm = table.Column<string>(nullable: true),
                    branch_rank_index = table.Column<decimal>(nullable: false),
                    cid = table.Column<long>(nullable: false),
                    last_modified_user = table.Column<long>(nullable: false),
                    last_modified_utc_dttm = table.Column<DateTime>(nullable: false),
                    RegionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_branch", x => x.branch_id);
                    table.ForeignKey(
                        name: "FK_tbl_branch_tbl_region_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "Coverage",
                        principalTable: "tbl_region",
                        principalColumn: "region_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_rep",
                schema: "Coverage",
                columns: table => new
                {
                    rep_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    active_ind = table.Column<bool>(nullable: false),
                    BranchId = table.Column<long>(nullable: true),
                    cid = table.Column<long>(nullable: false),
                    last_modified_user = table.Column<long>(nullable: false),
                    last_modified_utc_dttm = table.Column<DateTime>(nullable: false),
                    rep_cd = table.Column<string>(nullable: true),
                    rep_nm = table.Column<string>(nullable: true),
                    rep_rank_index = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rep", x => x.rep_id);
                    table.ForeignKey(
                        name: "FK_tbl_rep_tbl_branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Coverage",
                        principalTable: "tbl_branch",
                        principalColumn: "branch_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_branch_RegionId",
                schema: "Coverage",
                table: "tbl_branch",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_region_ChannelId",
                schema: "Coverage",
                table: "tbl_region",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rep_BranchId",
                schema: "Coverage",
                table: "tbl_rep",
                column: "BranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_rep",
                schema: "Coverage");

            migrationBuilder.DropTable(
                name: "tbl_branch",
                schema: "Coverage");

            migrationBuilder.DropTable(
                name: "tbl_region",
                schema: "Coverage");

            migrationBuilder.DropTable(
                name: "tbl_channel",
                schema: "Coverage");
        }
    }
}
