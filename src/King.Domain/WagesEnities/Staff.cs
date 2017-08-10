using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace King.Domain.WagesEnities
{
    /// <summary>
    /// 公司员工
    /// </summary>
    public class Staff : Entity
    {
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
        [StringLength(maximumLength:200)]
        public string HeadImg { get; set; }
        /// <summary>
        /// 登录密码 
        /// </summary>
        [StringLength(maximumLength:50)]
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
        /// 删除标识 0 正常 1已删除
        /// </summary>
        public int IsDel { get; set; }
        /// <summary>
        /// 刷新token
        /// </summary>
        [StringLength(maximumLength: 36)]
        public string RefToken { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PaymentPwd { get; set; }
        /// <summary>
        /// 支付宝账号
        /// </summary>
        public string AlipayAccount { get; set; }

    }
}
