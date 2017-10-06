using Microsoft.EntityFrameworkCore;
using King.Domain.Entities;
using King.Domain.WagesEnities;
using System.Collections.Generic;
using System;
using System.Reflection;

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
        public DbSet<PaymentQRTmp> PaymentQRTmps { get; set; }
        public DbSet<FixedInterest> FixedInterests { get; set; }
        public DbSet<CurrentInterest> CurrentInterests { get; set; }


        //public List<T> GetList<T>(string sql,params object[] sqlParames) where T : new()
        //{
            
        //    List<T> ret = new List<T>();
        //    Type t = typeof(T);
        //    var connection = this.Database.GetDbConnection();

        //    if (connection.State == System.Data.ConnectionState.Closed)
        //        connection.Open();
        //    using (var command = connection.CreateCommand())
        //    {
        //        command.CommandText = sql;
        //        command.Parameters.AddRange(sqlParames);
        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                if (t.IsPointer && t.Name == "String")
        //                {
        //                    ret.Add(reader[0] != DBNull.Value ? (T)reader[0] : default(T));
        //                }
        //                else
        //                {
        //                    T tObj = new T();
        //                    Type tobjType = tObj.GetType();
        //                    PropertyInfo[] propertyInfos = tobjType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        //                    foreach (var item in propertyInfos)
        //                    {
        //                        if (reader[item.Name] != DBNull.Value)
        //                        {
        //                            item.SetValue(tObj, reader[item.Name], null);
        //                        }
        //                    }
        //                    ret.Add(tObj);
        //                }
        //            }
        //        }
        //    }
        //    return ret;
        //}

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
