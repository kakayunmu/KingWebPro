using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using King.Application.RoleApp;
using King.MVC.Models;
using King.Application.RoleApp.Dtos;

namespace King.MVC.Controllers
{
    [Authorize]
    public class RoleController : KingControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRoleAppService _roleAppService;
        public RoleController(ILogger<RoleController> logger, IRoleAppService roleAppService)
        {
            _logger = logger;
            _roleAppService = roleAppService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody]QueryParam qparam)
        {
            return Json(await _roleAppService.GetAllPageList(qparam.StartPage, qparam.PageSize));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleDto role)
        {
            try
            {
                await _roleAppService.InsertOrUpdate(role);
                return Json(new { result = "Success", message = "保存数据成功" });
            }
            catch (Exception ex)
            {

                return Json(new { result = "Error", msg = ex.Message });
            }

        }
        [HttpPost]
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
                await _roleAppService.DeleteBatch(idsGuidList);
                return Json(new { result = "Success", message = "删除数据成功" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Get(Guid id)
        {
            return Json(await _roleAppService.Get(id));
        }
        [HttpPost]
        public async Task<IActionResult> SaveMenu(Guid roleId, List<RoleMenuDto> permissions)
        {
            try
            {
                bool ret = await _roleAppService.UpdateRoleMenu(roleId, permissions);
                if (ret)
                {
                    return Json(new { result = "Success", message = "保存成功" });
                }
                else
                {
                    return Json(new { result = "Error", message = "保存失败" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetMenusByRole(Guid roleId)
        {
            var dtos = await _roleAppService.GetAllMenuListByRole(roleId);
            return Json(dtos);
        }
    }
}