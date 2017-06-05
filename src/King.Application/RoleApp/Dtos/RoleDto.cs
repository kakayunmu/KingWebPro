using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace King.Application.RoleApp.Dtos
{
    public class RoleDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        [Required(ErrorMessage = "角色编码不能为空。")]
        public string Code { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空。")]
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
    }
}
