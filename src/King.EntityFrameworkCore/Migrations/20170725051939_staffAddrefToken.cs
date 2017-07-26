using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.EntityFrameworkCore.Migrations
{
    public partial class staffAddrefToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefToken",
                table: "Staffs",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GrpupId",
                table: "WagesImportRecord",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefToken",
                table: "Staffs");

            migrationBuilder.AlterColumn<string>(
                name: "GrpupId",
                table: "WagesImportRecord",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
