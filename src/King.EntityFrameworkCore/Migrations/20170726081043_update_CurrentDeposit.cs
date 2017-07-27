using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.EntityFrameworkCore.Migrations
{
    public partial class update_CurrentDeposit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JsonObj",
                table: "CurrentDeposits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonObj",
                table: "CurrentDeposits");
        }
    }
}
