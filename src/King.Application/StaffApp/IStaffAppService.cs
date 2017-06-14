using King.Application.StaffApp.Dtos;
using King.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace King.Application.StaffApp
{
    public interface IStaffAppService
    {
        /// <summary>
        /// 检查员工是否存在
        /// </summary>
        /// <param name="IDNumber"></param>
        /// <returns></returns>
        Task<bool> CheckStaff(string IDNumber);
        /// <summary>
        /// 新建或编辑员工
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<StaffDto> InsertOrUpdate(StaffDto dto);
        /// <summary>
        /// 批量删除员工
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteBatch(List<Guid> ids);
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 获取员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<StaffDto> Get(Guid id);
        /// <summary>
        /// 获取员工列表
        /// </summary>
        /// <param name="startPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageData<StaffDto>> GetAllStaff(int startPage, int pageSize);

    }
}
