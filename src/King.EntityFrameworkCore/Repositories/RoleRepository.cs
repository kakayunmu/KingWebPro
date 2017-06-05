using System;
using System.Collections.Generic;
using System.Text;
using King.Domain.Entities;
using King.Domain.IRepositories;
using System.Linq;
using System.Threading.Tasks;

namespace King.EntityFrameworkCore.Repositories
{
    public class RoleRepository : KingRepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(KingDBContext dbContent) : base(dbContent)
        {
        }
        /// <summary>
        /// 根据角色获取权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<List<Guid>> GetAllMenuListByRole(Guid roleId)
        {
            var roleMenus = _dbContext.Set<RoleMenu>().Where(it => it.RoleId == roleId);
            var menuIds = from t in roleMenus select t.MenuId;
            return Task.FromResult(menuIds.ToList());
        }
        /// <summary>
        /// 更新角色权限关联关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleMenus"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRoleMenu(Guid roleId, List<RoleMenu> roleMenus)
        {
            var oldDatas = _dbContext.Set<RoleMenu>().Where(it => it.RoleId == roleId).ToList();
            oldDatas.ForEach(it => _dbContext.Set<RoleMenu>().Remove(it));
            _dbContext.SaveChanges();
            await _dbContext.Set<RoleMenu>().AddRangeAsync(roleMenus);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
