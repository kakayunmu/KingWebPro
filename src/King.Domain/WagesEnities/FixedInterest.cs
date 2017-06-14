using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    /// <summary>
    /// 固定存款每日利息记录
    /// </summary>
    public class FixedInterest : Entity
    {
        /// <summary>
        /// 存款记录ID
        /// </summary>
        public string FixedDepositId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amounts { get; set; }
        /// <summary>
        /// 产生日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否已结算 0 未结算 1 已结算
        /// </summary>
        public int Settled { get; set; }
    }
}
