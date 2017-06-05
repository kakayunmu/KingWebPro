using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using King.Domain.Entities;
using King.Application.MenuApp.Dtos;
using King.Application.DepartmentApp.Dtos;
using King.Application.RoleApp.Dtos;
using King.Application.UserApp.Dtos;

namespace King.Application
{
    public class KingMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Menu, MenuDto>();
                cfg.CreateMap<MenuDto, Menu>();
                cfg.CreateMap<Department, DepartmentDto>();
                cfg.CreateMap<DepartmentDto, Department>();
                cfg.CreateMap<Role, RoleDto>();
                cfg.CreateMap<RoleDto, Role>();
                cfg.CreateMap<RoleMenu, RoleMenuDto>();
                cfg.CreateMap<RoleMenuDto, RoleMenu>();
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();
            });
        }
    }
}
