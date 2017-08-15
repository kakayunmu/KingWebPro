using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using King.Application.StaffApp;
using King.MVC.Models;
using King.Application.StaffApp.Dtos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IStaffAppService _staffAppService;
        public StaffController(ILogger<StaffController> logger, IStaffAppService staffAppService)
        {
            _logger = logger;
            _staffAppService = staffAppService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetStaff([FromBody]QueryParam qparam)
        {
            var result = await _staffAppService.GetAllStaff(qparam.StartPage, qparam.PageSize);
            return Json(result);
        }

        public async Task<IActionResult> Edit(StaffDto dto)
        {
            try
            {
                if (dto.Id == Guid.Empty)
                {
                    dto.CreateTime = DateTime.Now;
                    dto.Password = Utility.Security.Encryption.Md5WithSalt("KingWeb", "123456");
                    dto.HeadImg = "img/user3-160x160.jpg";
                    dto.CurrentAmount = 0;
                    dto.FixedAmount = 0;
                    dto.IsDel = 0;
                }
                else
                {
                    var old = await _staffAppService.Get(dto.Id);
                    dto.CreateTime = DateTime.Now;
                    dto.Password = old.Password;
                    dto.HeadImg = old.HeadImg;
                    dto.CurrentAmount = old.CurrentAmount;
                    dto.FixedAmount = old.FixedAmount;
                    dto.IsDel = old.IsDel;                     
                }
                await _staffAppService.InsertOrUpdate(dto);
                return Json(new { Result = "Success", Message = "保存数据成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new
                {
                    Result = "Error",
                    Message = "保存数据失败，系统发生异常"
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
                await _staffAppService.DeleteBatch(idsGuidList);
                return Json(new { result = "Success", message = "删除数据成功" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
    }
}
