using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.Entities
{
    /// <summary>
    /// 角色与菜单对应实体
    /// </summary>
    public class RoleMenu
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 角色实体
        /// </summary>
        public Role Role { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; set; }
        /// <summary>
        /// 菜单实体
        /// </summary>
        public Menu Menu { get; set; }
    }
}
