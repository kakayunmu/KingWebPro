using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.EntityFrameworkCore.Migrations
{
    public partial class Staff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CurrentAmount = table.Column<decimal>(nullable: false),
                    FixedAmount = table.Column<decimal>(nullable: false),
                    HeadImg = table.Column<string>(nullable: true),
                    IDNumber = table.Column<string>(maxLength: 18, nullable: true),
                    IsDel = table.Column<int>(nullable: false),
                    MobileNumber = table.Column<string>(maxLength: 11, nullable: true),
                    Name = table.Column<string>(maxLength: 10, nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}
