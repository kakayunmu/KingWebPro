using System;
using System.Collections.Generic;
using System.Text;
using King.Application.RoleApp.Dtos;
using King.EntityFrameworkCore.Repositories;
using King.Domain.IRepositories;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using King.Domain.Entities;

namespace King.Application.RoleApp
{
    public class RoleAppService : IRoleAppService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleAppService(IRoleRepository roleRepositiry)
        {
            _roleRepository = roleRepositiry;
        }
        public async Task Delete(Guid id)
        {
            await _roleRepository.Delete(id);
        }

        public async Task DeleteBatch(List<Guid> ids)
        {
            await _roleRepository.Delete(it => ids.Contains(it.Id));
        }

        public async Task<RoleDto> Get(Guid id)
        {
            return Mapper.Map<RoleDto>(await _roleRepository.Get(id));
        }

        public async Task<List<RoleDto>> GetAllList()
        {
            var roleList = await _roleRepository.GetAllList();
            return Mapper.Map<List<RoleDto>>(roleList);
        }

        public async Task<List<Guid>> GetAllMenuListByRole(Guid roleId)
        {
            return await _roleRepository.GetAllMenuListByRole(roleId);
        }

        public async Task<PageData<RoleDto>> GetAllPageList(int startPage, int pageSize)
        {
            var roles = await _roleRepository.LoadPageList(startPage, pageSize, null, it => it.Code);
            return new PageData<RoleDto>()
            {
                Total = roles.Total,
                Rows = Mapper.Map<List<RoleDto>>(roles.Rows).AsQueryable<RoleDto>()
            };
        }

        public async Task<RoleDto> InsertOrUpdate(RoleDto dto)
        {
            return Mapper.Map<RoleDto>(await _roleRepository.InsertOrUpdate(Mapper.Map<Role>(dto)));

        }

        public async Task<bool> UpdateRoleMenu(Guid roleId, List<RoleMenuDto> roleMenus)
        {
            return await _roleRepository.UpdateRoleMenu(roleId, Mapper.Map<List<RoleMenu>>(roleMenus));
        }
    }
}
