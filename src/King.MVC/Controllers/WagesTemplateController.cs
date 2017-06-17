using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using King.Application.WagesTemplateApp;
using Microsoft.Extensions.Logging;
using King.Utility.Extended;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    [Authorize]
    public class WagesTemplateController : Controller
    {

        private readonly IWagesTemplateAppService _wtAppService;
        private readonly ILogger<WagesTemplateController> _logger;
        public WagesTemplateController(IWagesTemplateAppService wtAppService, ILogger<WagesTemplateController> logger)
        {
            _wtAppService = wtAppService;
            _logger = logger;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UpLoadWagesFile()
        {
            var file = Request.Form.Files[0];
            string fname = file.FileName;
            string localPath = Directory.GetCurrentDirectory();
            FileInfo fileInfo = new FileInfo(string.Format("{0}/upload/{1}_{2}", localPath, Guid.NewGuid().ToString(), fname));
            try
            {
                using (FileStream fs = new FileStream(fileInfo.ToString(), FileMode.Create))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var groupId = await _wtAppService.BulkInsert(fileInfo);
                return Json(new
                {
                    Result = "Success",
                    Message = "上传文件成功",
                    Data = groupId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new
                {
                    Result = "Error",
                    Message = "删除文件失败"
                });
            }
        }

        public async Task<IActionResult> GetImportData(Guid groupId)
        {
            try
            {
                var list = await _wtAppService.GetAllByGroupId(groupId);
                return Json(new
                {
                    Result = "Success",
                    Message = "获取数据成功",
                    data = list
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new
                {
                    Result = "Error",
                    Message = "获取数据失败"
                });
            }
        }

        public async Task<IActionResult> DoCompare(Guid groupId)
        {
            try
            {
                var list = await _wtAppService.Compare(groupId);
                return Json(new
                {
                    Result = "Success",
                    Message = "获取数据成功",
                    data = list,
                    Flg = list.Find(it => it.IsMapping == 3) == null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new
                {
                    Result = "Error",
                    Message = "获取数据失败"
                });
            }
        }

        public async Task<IActionResult> WagesImport(Guid groupId)
        {
            try
            {
                await _wtAppService.WagesImport(groupId, Guid.Parse(User.GetClaimVal(ClaimTypes.NameIdentifier)));
                return Json(new
                {
                    Result = "Success",
                    Message = "导入工资成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new
                {
                    Result = "Error",
                    Message = "导入工资失败，系统发生异常",
                });
            }
        }
    }
}
