using System;
using System.Collections.Generic;
using System.Text;

namespace King.Application.FixedProductApp.Dtos
{
    public class FixedProductDto
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public Guid Id { get; set; }
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
        /// 是否已删除 0 正常 1 已删除
        /// </summary>
        public int IsDel { get; set; }
        /// <summary>
        /// 数据状态 0 上架 1 下架
        /// </summary>
        public int DataState { get; set; }
    }
}
