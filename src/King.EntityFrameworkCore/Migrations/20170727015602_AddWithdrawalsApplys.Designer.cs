using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using King.EntityFrameworkCore;

namespace King.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(KingDBContext))]
    [Migration("20170727015602_AddWithdrawalsApplys")]
    partial class AddWithdrawalsApplys
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("King.Domain.Entities.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("ContactNumber");

                    b.Property<DateTime?>("CreateTime");

                    b.Property<Guid>("CreateUserId");

                    b.Property<int>("IsDeleted");

                    b.Property<string>("Manager");

                    b.Property<string>("Name");

                    b.Property<Guid>("ParentId");

                    b.Property<string>("Reamrks");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("King.Domain.Entities.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<Guid>("ParentId");

                    b.Property<string>("Remarks");

                    b.Property<int>("SerialNumber");

                    b.Property<int>("Type");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("King.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<DateTime?>("CreateTime");

                    b.Property<Guid>("CreateUserId");

                    b.Property<string>("Name");

                    b.Property<string>("Remarks");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("King.Domain.Entities.RoleMenu", b =>
                {
                    b.Property<Guid>("RoleId");

                    b.Property<Guid>("MenuId");

                    b.HasKey("RoleId", "MenuId");

                    b.HasIndex("MenuId");

                    b.ToTable("RoleMenus");
                });

            modelBuilder.Entity("King.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateTime");

                    b.Property<Guid>("CreateUserId");

                    b.Property<Guid>("DepartmentId");

                    b.Property<string>("EMail");

                    b.Property<string>("HeadImg");

                    b.Property<int>("IsDeleted");

                    b.Property<DateTime?>("LastLoginTime");

                    b.Property<int>("LoginTimes");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Remarks");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("King.Domain.Entities.UserRole", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("King.Domain.WagesEnities.CurrentDeposit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("JsonObj");

                    b.Property<int>("MType");

                    b.Property<string>("Remarks");

                    b.Property<Guid>("StaffId");

                    b.HasKey("Id");

                    b.ToTable("CurrentDeposits");
                });

            modelBuilder.Entity("King.Domain.WagesEnities.FixedProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AIRate");

                    b.Property<int>("DataState");

                    b.Property<int>("IsDel");

                    b.Property<string>("Name");

                    b.Property<int>("TimeLimit");

                    b.Property<int>("TimeLimitUnit");

                    b.HasKey("Id");

                    b.ToTable("FixedProducts");
                });

            modelBuilder.Entity("King.Domain.WagesEnities.Setting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("GeneralInterestRate");

                    b.Property<decimal>("MaxAllAmount");

                    b.Property<decimal>("MaxPersonalAmount");

                    b.Property<decimal>("MaxWithdrawals");

                    b.Property<decimal>("PoolAmountRemind");

                    b.Property<string>("RemindMobiles");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("King.Domain.WagesEnities.Staff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<decimal>("CurrentAmount");

                    b.Property<decimal>("FixedAmount");

                    b.Property<string>("HeadImg")
                        .HasMaxLength(200);

                    b.Property<string>("IDNumber")
                        .HasMaxLength(18);

                    b.Property<int>("IsDel");

                    b.Property<string>("MobileNumber")
                        .HasMaxLength(11);

                    b.Property<string>("Name")
                        .HasMaxLength(10);

                    b.Property<string>("Password")
                        .HasMaxLength(50);

                    b.Property<string>("RefToken")
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("King.Domain.WagesEnities.WagesImportRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreateBy");

                    b.Property<DateTime>("CreateTime");

                    b.Property<Guid>("GrpupId");

                    b.HasKey("Id");

                    b.ToTable("WagesImportRecord");
                });

            modelBuilder.Entity("King.Domain.WagesEnities.WagesTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("CreateTime");

                    b.Property<Guid>("GroupId");

                    b.Property<string>("IDNumber");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("StaffName");

                    b.HasKey("Id");

                    b.ToTable("WagesTemplates");
                });

            modelBuilder.Entity("King.Domain.WagesEnities.WithdrawalsApply", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<int>("ApplyState");

                    b.Property<DateTime>("ApplyTime");

                    b.Property<string>("Auditor");

                    b.Property<DateTime>("AuditorTime");

                    b.Property<string>("StaffId");

                    b.HasKey("Id");

                    b.ToTable("WithdrawalsApplys");
                });

            modelBuilder.Entity("King.Domain.Entities.RoleMenu", b =>
                {
                    b.HasOne("King.Domain.Entities.Menu", "Menu")
                        .WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("King.Domain.Entities.Role", "Role")
                        .WithMany("RoleMenus")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("King.Domain.Entities.User", b =>
                {
                    b.HasOne("King.Domain.Entities.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("King.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("King.Domain.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("King.Domain.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
