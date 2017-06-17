using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    public class WagesTemplate2StaffCompare:WagesTemplate
    {
        /// <summary>
        /// 是否有员工对应 1 匹配成功 2不存在 3 存在错误
        /// </summary>
        public int IsMapping { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
