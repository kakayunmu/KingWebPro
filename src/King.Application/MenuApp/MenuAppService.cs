using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using King.Application.MenuApp.Dtos;
using King.Domain.IRepositories;
using AutoMapper;
using System.Linq;
using King.Domain.Entities;
using System.Linq.Expressions;

namespace King.Application.MenuApp
{
    public class MenuAppService : IMenuAppService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUserRepository _userRepository;

        public MenuAppService(IMenuRepository menuRepository, IUserRepository userRepository)
        {
            _menuRepository = menuRepository;
            _userRepository = userRepository;
        }

        public Task<List<MenuTreeDto>> ConvertL2T(List<MenuDto> menus, string url = null)
        {
            var retLi = new List<MenuTreeDto>();
            foreach (var item in menus.FindAll(it => it.ParentId == Guid.Empty))
            {
                bool ownActive = item.Url!=null && url.IndexOf(item.Url) != -1;
                bool active = false;
                retLi.Add(new MenuTreeDto()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Icon = item.Icon,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    Remarks = item.Remarks,
                    SerialNumber = item.SerialNumber,
                    Type = item.Type,
                    Url = item.Url,
                    Childs = MenuRec(item.Id, menus, url, out active),
                    Active = ownActive ? ownActive : active
                });
            }
            return Task.FromResult(retLi);
        }

        private List<MenuTreeDto> MenuRec(Guid pId, List<MenuDto> menus, string url, out bool active)
        {
            var childs = menus.FindAll(m => m.ParentId == pId);
            List<MenuTreeDto> retLi = new List<MenuTreeDto>();
            bool childsActiv = false;
            foreach (var item in childs)
            {
                bool ownActive = item.Url !=null && url.IndexOf(item.Url) != -1;
                if (ownActive)
                    childsActiv = ownActive;
                retLi.Add(new MenuTreeDto()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Icon = item.Icon,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    Remarks = item.Remarks,
                    SerialNumber = item.SerialNumber,
                    Type = item.Type,
                    Url = item.Url,
                    Childs = MenuRec(item.Id, menus, url, out active),
                    Active = ownActive ? ownActive : active
                });
            }
            active = childsActiv;
            return retLi;
        }

        public async Task Delete(Guid id)
        {
            await _menuRepository.Delete(id);
            //递归删除所有子类
            var childs = await _menuRepository.GetAllList(m => m.ParentId == id);
            foreach (var item in childs)
            {
                await (Delete(item.Id));
            }
        }

        public async Task DeleteBatch(List<Guid> ids)
        {
            foreach (var item in ids)
            {
                await Delete(item);
            }
        }

        public async Task<MenuDto> Get(Guid id)
        {
            return Mapper.Map<MenuDto>(await _menuRepository.Get(id));
        }

        public async Task<List<MenuDto>> GetAllList(Expression<Func<Menu, bool>> expression = null)
        {
            var menus = await _menuRepository.GetAllList(expression);
            return Mapper.Map<List<MenuDto>>(menus);
        }

        public async Task<PageData<MenuDto>> GetMenusByParent(Guid parentId, int startPage, int pageSize)
        {
            var menus = await _menuRepository.LoadPageList(startPage, pageSize, it => it.ParentId == parentId, it => it.SerialNumber);
            var dataList = Mapper.Map<List<MenuDto>>(menus.Rows.ToList());
            return new PageData<MenuDto>()
            {
                Total = menus.Total,
                Rows = dataList.AsQueryable<MenuDto>()
            };
        }

        public async Task<List<MenuDto>> GetMenusByUser(Guid userId)
        {
            List<MenuDto> result = new List<MenuDto>();
            var allMenus = await _menuRepository.GetAllList(it => it.Type == 0);
            allMenus.OrderBy(it => it.SerialNumber);
            // if (userId == Guid.Empty)
            return Mapper.Map<List<MenuDto>>(allMenus);

        }

        public async Task<bool> InsertOrUpdate(MenuDto dto)
        {
            var menu = await _menuRepository.InsertOrUpdate(Mapper.Map<Menu>(dto));
            return menu == null ? false : true;
        }
    }
}
