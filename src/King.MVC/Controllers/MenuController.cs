using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using King.Application.MenuApp;
using Microsoft.Extensions.Logging;
using King.MVC.Models;
using King.Application.MenuApp.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace King.MVC.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IMenuAppService _menuAppService;
        private readonly ILogger _logger;
        public MenuController(IMenuAppService menuAppService, ILogger<MenuController> logger)
        {
            _logger = logger;
            _menuAppService = menuAppService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetMenuTreeData()
        {
            var menus = await _menuAppService.GetAllList();
            List<TreeModel> treeModels = new List<TreeModel>();
            foreach (var menu in menus)
            {
                treeModels.Add(new TreeModel()
                {
                    Id = menu.Id.ToString(),
                    Text = menu.Name,
                    Parent = menu.ParentId == Guid.Empty ? "#" : menu.ParentId.ToString()
                });
            }
            return Json(treeModels);
        }

        [HttpPost]
        public async Task<IActionResult> GetMneusByParent([FromBody]QueryParam qparam)
        {
            Guid parentId = Guid.Parse(qparam.SearchFelds.Find(f => f.Field == "parentId").Val);
            var result = await _menuAppService.GetMenusByParent(parentId, qparam.StartPage, qparam.PageSize);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MenuDto menu)
        {
            if (await _menuAppService.InsertOrUpdate(menu))
            {
                return Json(new { result = "Success", message = "数据保存成功" });
            }
            else
            {
                return Json(new { result = "Error", message = "保存数据失败" });
            }

        }

        public async Task<IActionResult> Get(string id)
        {
            var menu = await _menuAppService.Get(Guid.Parse(id));
            return Json(menu);
        }

        public async Task<IActionResult> DeleteMuti(string ids)
        {
            try
            {
                var idsArray = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var idsGuidList = new List<Guid>();
                foreach (var item in idsArray)
                {
                    idsGuidList.Add(Guid.Parse(item));
                }
                await _menuAppService.DeleteBatch(idsGuidList);
                return Json(new { result = "Success", message = "删除数据成功" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
    }
}