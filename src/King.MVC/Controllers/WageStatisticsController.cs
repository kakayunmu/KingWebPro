using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using King.EntityFrameworkCore;
using King.MVC.Models;
using King.Domain.IRepositories;
using King.Domain.WagesEnities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    public class WageStatisticsController : Controller
    {
        private EntityFrameworkCore.KingDBContext content;
        private IMemoryCache memoryCache;
        public WageStatisticsController(EntityFrameworkCore.KingDBContext content, IMemoryCache memoryCache)
        {
            this.content = content;
            this.memoryCache = memoryCache;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetWageStatistics([FromBody]QueryParam qparam)
        {
            string where = " Where 1=1 ";
            var staffName = qparam.SearchFelds.Find(sp => sp.Field == "staffName");
            if (staffName != null)
            {
                where += " AND staffName like '%"+staffName.Val+"%' ";
            }
            var stime = qparam.SearchFelds.Find(sp => sp.Field == "stime");
            if (stime != null)
            {
                where += " AND currDate= '" + stime.Val+"' ";
            }
            Int64 total = await content.GetModel<Int64>("SELECT COUNT(*) from wageStatistics"+where);
            var rows = await content.GetList<WageStatistics>("SELECT * from wageStatistics "+where+" order by currDate desc LIMIT " + (qparam.StartPage - 1) * qparam.PageSize + "," + qparam.PageSize);
            return Json(new PageData2<WageStatistics>()
            {
                Total = total,
                Rows = rows 
            });
        }
    }
}
