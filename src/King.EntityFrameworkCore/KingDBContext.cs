﻿using Microsoft.EntityFrameworkCore;
using King.Domain.Entities;
using King.Domain.WagesEnities;

namespace King.EntityFrameworkCore
{
    public class KingDBContext : DbContext
    {
        public KingDBContext(DbContextOptions<KingDBContext> options) : base(options)
        {

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<FixedProduct> FixedProducts { get; set; }
        public DbSet<WagesTemplate> WagesTemplates { get; set; }
        public DbSet<WagesImportRecord> WagesImportRecord { get; set; }
        public DbSet<CurrentDeposit> CurrentDeposits { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<WithdrawalsApply> WithdrawalsApplys { get; set; }
        public DbSet<FixedDeposit> FixedDeposits { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.Entity<RoleMenu>()
                .HasKey(rm => new { rm.RoleId, rm.MenuId });
            base.OnModelCreating(builder);



        }
    }
}
