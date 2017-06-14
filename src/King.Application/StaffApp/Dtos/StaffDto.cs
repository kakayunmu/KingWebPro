using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace King.Application.StaffApp.Dtos
{
    public class StaffDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        [MaxLength(length: 10)]
        public string Name { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        [StringLength(18, ErrorMessage = "身份证必须为18位")]
        public string IDNumber { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [StringLength(11, ErrorMessage = "手机号长度必须为11位")]
        public string MobileNumber { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg { get; set; }
        /// <summary>
        /// 登录密码 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 活期总额
        /// </summary>
        public decimal CurrentAmount { get; set; }
        /// <summary>
        /// 固定总额
        /// </summary>
        public decimal FixedAmount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
        public int IsDel { get; set; }
    }
}
