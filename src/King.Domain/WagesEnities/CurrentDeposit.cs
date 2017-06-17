﻿using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    /// <summary>
    /// 活期记录
    /// </summary>
    public class CurrentDeposit : Entity
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
        /// 类型  1工资、2充值、3消费、4提现、5转出
        /// </summary>
        public int MType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
