using System;
using System.Collections.Generic;
using System.Text;
using King.Application.DepartmentApp.Dtos;
using System.Threading.Tasks;
using King.Domain.IRepositories;

namespace King.Application.DepartmentApp
{
    public interface IDepartmentAppService
    {
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns>部门集合</returns>
        Task<List<DepartmentDto>> GetAll();
        /// <summary>
        /// 通过父部门获取子部门
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <param name="startPage">当前页码</param>
        /// <param name="pageSize">每页数据行数</param>
        /// <returns>子级部门集合</returns>
        Task<PageData<DepartmentDto>> GetDepartmentByParent(Guid parentId, int startPage, int pageSize);
        /// <summary>
        /// 插入或更新部门
        /// </summary>
        /// <param name="dot"></param>
        /// <returns></returns>
        Task<DepartmentDto> InsertOrUpdate(DepartmentDto dot);
        /// <summary>
        /// 批量删除部门
        /// </summary>
        /// <param name="ids">部门Ids</param>
        /// <returns></returns>
        Task DeleteBatch(List<Guid> ids);
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns>是否成功</returns>
        Task Delete(Guid id);
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        Task<DepartmentDto> Get(Guid id);
        /// <summary>
        /// 部门集合转树形部门集合
        /// </summary>
        /// <param name="departments">部门集合</param>
        /// <returns>树形部门集合</returns>
        Task<List<DepartmentTreeDto>> ConvertL2T(List<DepartmentDto> departments);
    }
}
