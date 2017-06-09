using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.EntityFrameworkCore.Migrations
{
    public partial class user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeadImg",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadImg",
                table: "Users");
        }
    }
}
