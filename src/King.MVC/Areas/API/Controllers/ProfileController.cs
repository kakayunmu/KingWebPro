using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using King.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Areas.API.Controllers
{
    [Produces("application/json")]
    public class ProfileController : BaseController
    {
        public ProfileController(IMemoryCache memoryCache, KingDBContext dbContent, Logger<ProfileController> logger) : base(memoryCache, dbContent,logger)
        {
        }

        //获取用户信息
        [HttpGet]
        public IActionResult GetStaffInfo()
        {
            return Json(new { status = 0, msg = "获取数据成功", staff = staff });
        }
        //获取固定记录
        [HttpGet]
        public IActionResult GetFixedDeposit()
        {
            var fixedDeposits = dbContent.FixedDeposits.Where(fd => fd.StaffId == staff.Id);
            return Json(new { status = 0, msg = "获取数据成功", data = fixedDeposits });

        }
        public IActionResult GetCurrentDeposit(int mType, int pageIndex = 1, int pageSize = 10)
        {

            var currentDeposit = dbContent.CurrentDeposits.Where(cd => cd.StaffId == staff.Id && (mType == 0 ? true : cd.MType == mType));
            var retData = currentDeposit.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return Json(new { status = 0, msg = "获取数据成功", data = retData });
        }

    }
}
