using Microsoft.EntityFrameworkCore;
using King.Domain.Entities;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.Entity<RoleMenu>()
                .HasKey(rm => new { rm.RoleId, rm.MenuId });

            //builder.Entity<UserRole>()
            //   .HasOne(ur => ur.User)
            //   .WithMany(u => u.UserRoles)
            //   .HasForeignKey(ur => ur.UserId);
            //builder.Entity<UserRole>()
            //    .HasOne(ur => ur.Role)
            //    .WithMany(r => r.UserRoles)
            //    .HasForeignKey(ur => ur.RoleId);

            base.OnModelCreating(builder);



        }
    }
}
