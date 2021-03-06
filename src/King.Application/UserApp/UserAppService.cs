﻿using System;
using System.Collections.Generic;
using System.Text;
using King.Domain.Entities;
using King.Domain.IRepositories;
using System.Threading.Tasks;
using King.Application.UserApp.Dtos;
using AutoMapper;
using System.Linq;

namespace King.Application.UserApp
{
    /// <summary>
    /// 用户管理服务
    /// </summary>
    public class UserAppService : IUserAppService
    {
        /// <summary>
        /// 用户管理仓储接口
        /// </summary>
        private readonly IUserRepository _repository;
        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="repository"></param>
        public UserAppService(IUserRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<User> CheckUser(string userName, string password)
        {
            return await _repository.CheckUser(userName, King.Utility.Security.Encryption.Md5WithSalt("KingWeb", password));
        }

        public async Task Delete(Guid id)
        {
            var user = await _repository.Get(id);
            user.IsDeleted = 1;
            await _repository.InsertOrUpdate(user);
        }

        public async Task DeleteBatch(List<Guid> ids)
        {
            foreach (var item in ids)
            {
                await Delete(item);
            }
        }

        public async Task<UserDto> Get(Guid id)
        {
            return Mapper.Map<UserDto>(await _repository.GetWithRoles(id));
        }

        public async Task<PageData<UserDto>> GetUserByDepartment(Guid departmentId, int startPage, int pageSize)
        {
            var list = await _repository.LoadPageList(startPage, pageSize, it => it.DepartmentId == departmentId && it.IsDeleted == 0, it => it.CreateTime);
            return new PageData<UserDto>()
            {
                Total = list.Total,
                Rows = Mapper.Map<List<UserDto>>(list.Rows).AsQueryable<UserDto>()
            };
        }

        public async Task<UserDto> InsertOrUpdate(UserDto dto)
        {
            if (await Get(dto.Id) != null)
            {
                await _repository.Delete(dto.Id);
            }
            else
            {
                dto.Password = King.Utility.Security.Encryption.Md5WithSalt("KingWeb", "888888");
            }
            var user = await _repository.InsertOrUpdate(Mapper.Map<User>(dto));
            return Mapper.Map<UserDto>(user);
        }

        public async Task ResetPwd(Guid userId, string pwd)
        {
            var user = await _repository.Get(userId);
            user.Password = King.Utility.Security.Encryption.Md5WithSalt("KingWeb", pwd);
            await _repository.InsertOrUpdate(user);
        }
    }
}
