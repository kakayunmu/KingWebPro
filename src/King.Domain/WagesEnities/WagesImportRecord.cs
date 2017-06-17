using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    /// <summary>
    /// 导入记录
    /// </summary>
    public class WagesImportRecord : Entity
    {
        /// <summary>
        /// 导入批次ID
        /// </summary>
        public Guid GrpupId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid CreateBy { get; set; }
    }
}
