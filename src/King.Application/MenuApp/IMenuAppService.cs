using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using King.Application.MenuApp.Dtos;
using King.Domain.IRepositories;
using System.Linq.Expressions;
using King.Domain.Entities;

namespace King.Application.MenuApp
{
    public interface IMenuAppService
    {
        Task<List<MenuDto>> GetAllList(Expression<Func<Menu, bool>> expression=null);
        Task<PageData<MenuDto>> GetMenusByParent(Guid parentId, int startPage, int pageSize);
        Task<bool> InsertOrUpdate(MenuDto dto);
        Task DeleteBatch(List<Guid> ids);
        Task Delete(Guid id);
        Task<MenuDto> Get(Guid id);
        Task<List<MenuDto>> GetMenusByUser(Guid userId);

        Task<List<MenuTreeDto>> ConvertL2T(List<MenuDto> menus,string url=null);
    }
}
