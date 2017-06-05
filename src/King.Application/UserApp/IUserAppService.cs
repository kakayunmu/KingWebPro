using System;
using System.Collections.Generic;
using System.Text;
using King.Domain.Entities;
using System.Threading.Tasks;
using King.Application.UserApp.Dtos;
using King.Domain.IRepositories;

namespace King.Application.UserApp
{
    public interface IUserAppService
    {
        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<User> CheckUser(string userName, string password);
        /// <summary>
        /// 通过部门ID获取用户列表
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="startPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageData<UserDto>> GetUserByDepartment(Guid departmentId, int startPage, int pageSize);
        /// <summary>
        /// 新建或更新用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UserDto> InsertOrUpdate(UserDto dto);
        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteBatch(List<Guid> ids);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 通过ID获取用户实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDto> Get(Guid id);

    }
}
