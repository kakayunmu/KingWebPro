using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using King.Application.DepartmentApp;
using King.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using King.Application.DepartmentApp.Dtos;
using King.Utility.Extended;
using System.Security.Claims;

namespace King.MVC.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly ILogger _logger;
        public readonly IDepartmentAppService _departmentAppService;

        public DepartmentController(IDepartmentAppService departmentAppService, ILogger<DepartmentController> logger)
        {
            _logger = logger;
            _departmentAppService = departmentAppService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetDepartmentTreeData()
        {
            var departments = await _departmentAppService.GetAll();
            List<TreeModel> treeModels = new List<TreeModel>();
            foreach (var item in departments)
            {
                treeModels.Add(new TreeModel()
                {
                    Id = item.Id.ToString(),
                    Parent = item.ParentId == Guid.Empty ? "#" : item.ParentId.ToString(),
                    Text = item.Name
                });
            }
            return Json(treeModels);
        }

        public async Task<IActionResult> GetDepartmentsByParent([FromBody]QueryParam qparam)
        {
            Guid parentId = Guid.Parse(qparam.SearchFelds.Find(f => f.Field == "parentId").Val);
            var result = await _departmentAppService.GetDepartmentByParent(parentId, qparam.StartPage, qparam.PageSize);
            return Json(result);
        }

        public async Task<IActionResult> Edit(DepartmentDto department)
        {

            department.CreateUserId = Guid.Parse(User.GetClaimVal(ClaimTypes.NameIdentifier));
            department.CreateTime = DateTime.Now;
            if (await _departmentAppService.InsertOrUpdate(department) != null)
            {
                return Json(new { result = "Success", message = "数据保存成功" });
            }
            else
            {
                return Json(new { result = "Error", message = "数据保存失败" });
            }
        }

        public async Task<IActionResult> Get(string id)
        {
            var menu = await _departmentAppService.Get(Guid.Parse(id));
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
                await _departmentAppService.DeleteBatch(idsGuidList);
                return Json(new { result = "Success", message = "删除数据成功" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
    }
}