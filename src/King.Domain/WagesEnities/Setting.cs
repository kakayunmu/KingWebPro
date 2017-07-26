using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    public class Setting : Entity
    {
        /// <summary>
        /// 取款需审核金额
        /// </summary>
        public decimal MaxWithdrawals { get; set; }
        /// <summary>
        /// 普通利率
        /// </summary>
        public decimal GeneralInterestRate { get; set; }
        /// <summary>
        /// 每日个人累计最大提现金额
        /// </summary>
        public decimal MaxPersonalAmount { get; set; }
        /// <summary>
        /// 每日公司累计最大提现金额
        /// </summary>
        public decimal MaxAllAmount { get; set; }
        /// <summary>
        /// 资金池额度预警额度
        /// </summary>
        public decimal PoolAmountRemind { get; set; }
        /// <summary>
        /// 预警手机号多个逗号分隔
        /// </summary>
        public string RemindMobiles { get; set; }
    }
}
