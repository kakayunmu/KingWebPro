using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.EntityFrameworkCore.Migrations
{
    public partial class UpdateWithdrawalsApplys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "StaffId",
                table: "WithdrawalsApplys",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StaffId",
                table: "WithdrawalsApplys",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
