using System;
using System.Collections.Generic;
using System.Text;
using King.Domain.Entities;
using System.Threading.Tasks;

namespace King.Domain.IRepositories
{
    public interface IRoleRepository : IRepository<Role>
    {
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
        Task<bool> UpdateRoleMenu(Guid roleId, List<RoleMenu> roleMenus);
    }
}
