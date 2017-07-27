using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.EntityFrameworkCore.Migrations
{
    public partial class AddWithdrawalsApplys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WithdrawalsApplys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Amount = table.Column<decimal>(nullable: false),
                    ApplyState = table.Column<int>(nullable: false),
                    ApplyTime = table.Column<DateTime>(nullable: false),
                    Auditor = table.Column<string>(nullable: true),
                    AuditorTime = table.Column<DateTime>(nullable: false),
                    StaffId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalsApplys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithdrawalsApplys");
        }
    }
}
