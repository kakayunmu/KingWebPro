using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using King.Application.FixedProductApp;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using King.MVC.Models;
using King.Application.FixedProductApp.Dtos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    [Authorize]
    public class FixedProductController : Controller
    {
        private readonly IFixedProductAppService _fpAppService;
        private readonly ILogger<FixedProductController> _logger;

        public FixedProductController(IFixedProductAppService fixedProductAppService, ILogger<FixedProductController> logger)
        {
            _fpAppService = fixedProductAppService;
            _logger = logger;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetFP([FromBody]QueryParam qparam)
        {
            var result = await _fpAppService.GetFixedProductByPage(qparam.StartPage, qparam.PageSize);
            return Json(result);
        }

        public async Task<IActionResult> Edit(FixedProductDto dto)
        {
            try
            {
                dto.CreateTime = DateTime.Now;
                await _fpAppService.InsertOrUpdate(dto);
                return Json(new
                {
                    Result = "Success",
                    Message = "保存数据成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new
                {
                    Result = "Error",
                    Message = "保存数据发生异常,保存失败"
                });
            }
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
                await _fpAppService.DeleteBatch(idsGuidList);
                return Json(new { result = "Success", message = "删除数据成功" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
    }
}
