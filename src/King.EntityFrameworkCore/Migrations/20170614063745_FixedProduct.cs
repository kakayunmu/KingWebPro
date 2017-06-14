using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.EntityFrameworkCore.Migrations
{
    public partial class FixedProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FixedProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AIRate = table.Column<decimal>(nullable: false),
                    DataState = table.Column<int>(nullable: false),
                    IsDel = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TimeLimit = table.Column<int>(nullable: false),
                    TimeLimitUnit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedProducts", x => x.Id);
                });

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Staffs",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HeadImg",
                table: "Staffs",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixedProducts");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Staffs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HeadImg",
                table: "Staffs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
