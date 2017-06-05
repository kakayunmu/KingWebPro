using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.Entities
{
    /// <summary>
    /// 用户角色实体
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户实体
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 角色实体
        /// </summary>
        public Role Role { get; set; }

    }
}
