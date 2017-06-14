using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    /// <summary>
    /// 固定存款产品
    /// </summary>
    public class FixedProduct:Entity
    {
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
    }
}
