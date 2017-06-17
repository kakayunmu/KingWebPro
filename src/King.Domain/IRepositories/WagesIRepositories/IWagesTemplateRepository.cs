using King.Domain.WagesEnities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace King.Domain.IRepositories.WagesIRepositories
{
    public interface IWagesTemplateRepository : IRepository<WagesTemplate>
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        Task BulkInsert(List<WagesTemplate> dataList);
        /// <summary>
        /// 导入数据与员工关联比较
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<List<WagesTemplate2StaffCompare>> Compare(Guid groupId);
        /// <summary>
        /// 数据导入
        /// </summary>
        /// <param name="groupId">导入批次ID</param>
        /// <returns></returns>
        Task WagesImport(Guid groupId, Guid execBy);
    }
}
