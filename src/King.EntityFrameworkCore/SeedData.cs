using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using King.Domain.Entities;
using System.Linq;

namespace King.EntityFrameworkCore
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new KingDBContext(serviceProvider.GetRequiredService<DbContextOptions<KingDBContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }
                //增加一个部门
                Guid departmentId = Guid.NewGuid();
                context.Departments.Add(new Department
                {
                    Id = departmentId,
                    Name = "总公司",
                    ParentId = Guid.Empty
                });
                //增加一个超级管理员
                context.Users.Add(new User
                {
                    UserName = "admin",
                    Password = "123456",
                    Name = "超级管理员",
                    DepartmentId = departmentId
                });
                //增加四个基本功能菜单
                var pmenuId = Guid.NewGuid();
                context.Menus.AddRange(
                    new Menu
                    {
                        Id = pmenuId,
                        Name = "系统管理",
                        Code = "SysMG",
                        SerialNumber = 0,
                        ParentId = Guid.Empty,
                        Icon = "fa fa-link",
                        Type = 0
                    },
                    new Menu
                    {
                        Name = "组织机构管理",
                        Code = "Department",
                        SerialNumber = 0,
                        ParentId = pmenuId,
                        Icon = "fa fa-link"
                    },
                    new Menu
                    {
                        Name = "角色管理",
                        Code = "Role",
                        SerialNumber = 1,
                        ParentId = pmenuId,
                        Icon = "fa fa-link"
                    },
                   new Menu
                   {
                       Name = "用户管理",
                       Code = "User",
                       SerialNumber = 2,
                       ParentId = pmenuId,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "功能管理",
                       Code = "Department",
                       SerialNumber = 3,
                       ParentId = pmenuId,
                       Icon = "fa fa-link"
                   });
                context.SaveChanges();
            }
        }
    }
}
