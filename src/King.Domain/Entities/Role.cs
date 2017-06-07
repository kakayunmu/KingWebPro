using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.Entities
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class Role : Entity
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 角色拥有的菜单
        /// </summary>
        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
        /// <summary>
        /// 角色拥有的用户
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
