using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using King.EntityFrameworkCore;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Areas.API.Controllers
{
    [Produces("application/json")]
    public class ProfileController : BaseController
    {
        private KingDBContext content;
        public ProfileController(KingDBContext content, IMemoryCache memoryCache) : base(memoryCache)
        {
            this.content = content;
        }

        [HttpGet]
        public IActionResult GetStaffInfo()
        {
            return Json(new { status = 0, msg = "获取数据成功", staff = staff });
        }

    }
}
