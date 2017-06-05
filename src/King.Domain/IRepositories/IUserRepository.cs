using System;
using System.Collections.Generic;
using System.Text;
using King.Domain.Entities;
using System.Threading.Tasks;

namespace King.Domain.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        Task<User> CheckUser(string userName, string password);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        Task<User> GetWithRoles(Guid id);
    }
}
