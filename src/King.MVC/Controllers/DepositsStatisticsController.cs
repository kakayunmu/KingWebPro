using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using King.MVC.Models;
using King.EntityFrameworkCore;
using King.Domain.IRepositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    public class DepositsStatisticsController : Controller
    {
        private EntityFrameworkCore.KingDBContext content;
        private IMemoryCache memoryCache;

        public DepositsStatisticsController(EntityFrameworkCore.KingDBContext content, IMemoryCache memoryCache)
        {
            this.content = content;
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDepositsStatistics([FromBody]QueryParam qparam)
        {
            string where = " Where 1=1 ";
            var staffName = qparam.SearchFelds.Find(sp => sp.Field == "staffName");
            if (staffName != null)
            {
                where += " AND name like '%" + staffName.Val + "%' ";
            }
            var stime = qparam.SearchFelds.Find(sp => sp.Field == "stime");
            if (stime != null)
            {
                where += " AND CreateTime like '" + stime.Val + "%' ";
            }
            Int64 total = await content.GetModel<Int64>("SELECT COUNT(*) from depositsstatistics" + where);
            var rows = await content.GetList<DepositsStatistics>("SELECT * from depositsstatistics " + where + " order by CreateTime desc LIMIT " + (qparam.StartPage - 1) * qparam.PageSize + "," + qparam.PageSize);
            return Json(new PageData2<DepositsStatistics>()
            {
                Total = total,
                Rows = rows
            });
        }

       public class DepositsStatistics
        {
            public decimal Amount { get; set; }
            public string Remarks { get; set; }
            public string Name { get; set; }
            public DateTime CreateTime { get; set; }
        }
    }
}
