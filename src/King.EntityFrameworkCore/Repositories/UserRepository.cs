using System;
using System.Collections.Generic;
using System.Text;
using King.Domain.Entities;
using King.Domain.IRepositories;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace King.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// 用户管理仓储实现
    /// </summary>
    public class UserRepository : KingRepositoryBase<User>, IUserRepository
    {
        public UserRepository(KingDBContext dbContent) : base(dbContent)
        { }
        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        public Task<User> CheckUser(string userName, string password)
        {
            return Task.FromResult(_dbContext.Set<User>().FirstOrDefault(it => it.UserName == userName && it.Password == password));
        }
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<User> GetWithRoles(Guid id)
        {
            var user = _dbContext.Users.Include(u => u.UserRoles).FirstOrDefault(it => it.Id == id);
            return Task.FromResult<User>(user);
        }
    }
}
