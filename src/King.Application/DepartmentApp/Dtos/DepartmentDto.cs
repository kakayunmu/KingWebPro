using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace King.Application.DepartmentApp.Dtos
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "部门名称不能为空。")]
        public string Name { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "部门编码不能为空。")]
        public string Code { get; set; }
        /// <summary>
        /// 负债人
        /// </summary>
        public string Manager { get; set; }
        /// <summary>
        /// 负债人联系电话
        /// </summary>
        public string ContactNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Reamrks { get; set; }
        /// <summary>
        /// 父部门ID
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public int IsDeleted { get; set; }
    }
}
