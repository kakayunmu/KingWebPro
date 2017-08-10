using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    public class PaymentQRTmp:Entity
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid StaffId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 收款人
        /// </summary>
        public Guid ToStaffId { get; set; }
    }
}
