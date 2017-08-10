using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    /// <summary>
    /// 固定记录
    /// </summary>
    public class FixedDeposit : Entity
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
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 期限
        /// </summary>
        public int TimeLimit { get; set; }
        /// <summary>
        /// 期限单位 0天 1月
        /// </summary>
        public int TimeLimitUnit { get; set; }
        /// <summary>
        /// 年化利率
        /// </summary>
        public decimal AIRate { get; set; }
        /// <summary>
        /// 0 正常 1已到期 2提前转出
        /// </summary>
        public int DataState { get; set; }
        /// <summary>
        /// 转存时间
        /// </summary>
        public DateTime DumpTime { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 转出时间
        /// </summary>
        public DateTime TurnOutTime { get; set; }
        /// <summary>
        /// 累计收益金额
        /// </summary>
        public decimal CumulativeAmount { get; set; }

        public ICollection<FixedInterest> FixedInterests { get; set; }
    }
}
