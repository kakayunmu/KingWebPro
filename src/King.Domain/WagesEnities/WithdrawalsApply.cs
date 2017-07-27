using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    /// <summary>
    /// 提现申请
    /// </summary>
    public class WithdrawalsApply : Entity
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid StaffId { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 提现申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// 审核状态 0 未审核 1 审核通过 2 审核不通过
        /// </summary>
        public int ApplyState { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string Auditor { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditorTime { get; set; }
    }
}
