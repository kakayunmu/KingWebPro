using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    public class WagesTemplate : Entity
    {
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string StaffName { get; set; }
        /// <summary>
        /// 员工手机号
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNumber { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 批次ID
        /// </summary>
        public Guid GroupId { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
