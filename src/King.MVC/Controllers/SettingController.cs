using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace King.MVC.Controllers
{
    [Authorize]
    public class SettingController : KingControllerBase
    {
        private EntityFrameworkCore.KingDBContext content;
        private readonly ILogger _logger;
        public SettingController(EntityFrameworkCore.KingDBContext content, ILogger<SettingController> logger)
        {
            this.content = content;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            var setting = content.Settings.FirstOrDefault();
            return View(setting);
        }

        public IActionResult SaveSetting(Domain.WagesEnities.Setting setting)
        {
            Guid id = Guid.NewGuid();
            if (setting.Id == Guid.Empty)
            {
                setting.Id = id;
                content.Settings.Add(setting);
            }
            else
            {
                id = setting.Id;
                content.Settings.Update(setting);
            }
            content.SaveChanges();
            return Json(new { status = 0, msg = "保存设置成功", dataId = id });
        }
    }
}