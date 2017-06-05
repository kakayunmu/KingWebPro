using King.Application.RoleApp.Dtos;
using King.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace King.Application.RoleApp
{
    public interface IRoleAppService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<List<RoleDto>> GetAllList();
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="startPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        Task<PageData<RoleDto>> GetAllPageList(int startPage, int pageSize);
        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<RoleDto> InsertOrUpdate(RoleDto dto);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        Task DeleteBatch(List<Guid> ids);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        Task Delete(Guid id);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RoleDto> Get(Guid id);
        /// <summary>
        /// 根据角色获取权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<Guid>> GetAllMenuListByRole(Guid roleId);
        /// <summary>
        /// 更新角色权限关联关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleMenus"></param>
        /// <returns></returns>
        Task<bool> UpdateRoleMenu(Guid roleId, List<RoleMenuDto> roleMenus);
    }
}
