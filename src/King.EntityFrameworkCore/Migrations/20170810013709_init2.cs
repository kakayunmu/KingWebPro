using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.EntityFrameworkCore.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentInterests_CurrentDeposits_CurrentDepositId",
                table: "CurrentInterests");

            migrationBuilder.DropColumn(
                name: "Settled",
                table: "CurrentInterests");

            migrationBuilder.AddColumn<Guid>(
                name: "StaffId",
                table: "CurrentInterests",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentDepositId",
                table: "CurrentInterests",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentInterests_CurrentDeposits_CurrentDepositId",
                table: "CurrentInterests",
                column: "CurrentDepositId",
                principalTable: "CurrentDeposits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentInterests_CurrentDeposits_CurrentDepositId",
                table: "CurrentInterests");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "CurrentInterests");

            migrationBuilder.AddColumn<int>(
                name: "Settled",
                table: "CurrentInterests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentDepositId",
                table: "CurrentInterests",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentInterests_CurrentDeposits_CurrentDepositId",
                table: "CurrentInterests",
                column: "CurrentDepositId",
                principalTable: "CurrentDeposits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
