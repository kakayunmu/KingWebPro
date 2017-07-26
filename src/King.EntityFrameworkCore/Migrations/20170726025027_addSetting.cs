using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.EntityFrameworkCore.Migrations
{
    public partial class addSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    GeneralInterestRate = table.Column<decimal>(nullable: false),
                    MaxAllAmount = table.Column<decimal>(nullable: false),
                    MaxPersonalAmount = table.Column<decimal>(nullable: false),
                    MaxWithdrawals = table.Column<decimal>(nullable: false),
                    PoolAmountRemind = table.Column<decimal>(nullable: false),
                    RemindMobiles = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
