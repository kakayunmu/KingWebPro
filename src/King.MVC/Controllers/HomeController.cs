using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using King.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    [Authorize]
    public class HomeController : KingControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private King.EntityFrameworkCore.KingDBContext _dbContext;
        public HomeController(ILogger<HomeController> logger, King.EntityFrameworkCore.KingDBContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetChart1Data()
        {
            var totalObj = await _dbContext.GetModel<TotalObj>("SELECT  SUM(CurrentAmount) as totalCA,SUM(FixedAmount) as totalFA  from staffs where IsDel=0");
            return Json(new { status = 0, msg = "获取数据成功", data = totalObj });
        }
        [HttpPost]
        public async Task<IActionResult> GetChart2Data()
        {
            var retData = await _dbContext.GetList<TXQS>("SELECT DATE_FORMAT(ApplyTime,'%Y-%m-%d') as applyTime, SUM(Amount) as totalAmount from withdrawalsapplys where ApplyTime>DATE_ADD(NOW(),INTERVAL  -60 DAY) group BY DATE_FORMAT(ApplyTime,'%Y-%m-%d')");
            var retData2 =await _dbContext.GetList<TXQS2>("SELECT DATE_FORMAT(ApplyTime, '%Y-%m-%d') AS applyTime, COUNT(1) AS totalCount FROM withdrawalsapplys WHERE ApplyTime > DATE_ADD(NOW(), INTERVAL -60 DAY) GROUP BY DATE_FORMAT(ApplyTime, '%Y-%m-%d')");
            return Json(new { status = 0, msg = "获取数据成功", data = retData, data2 = retData2 });
        }
        [HttpPost]
        public async Task<IActionResult> GetChart3Data()
        {
            List<Chart3Ret> ret = new List<Chart3Ret>();
            var retData = await _dbContext.GetList<Chart3>("SELECT tab.diffMonth, SUM(tab.Amount) AS totalAmount FROM (SELECT Amount, TIMESTAMPDIFF(MONTH, NOW(), ExpireTime) AS diffMonth FROM fixeddeposits WHERE DataState = 0 ) tab GROUP BY tab.diffMonth");
            for (int i = 0; i < 12; i++)
            {
                var time = DateTime.Now.AddMonths(i);
                ret.Add(new Chart3Ret()
                {
                    DiffMonth = time.ToString("yyyy-MM"),
                    TotalAmount = retData.Where(rd => rd.DiffMonth >= i).Sum(rd => rd.TotalAmount)
                });
            }
            return Json(new { status = 0, msg = "获取数据成功", data = ret });
        }
        [HttpPost]
        public async Task<IActionResult> GetChart4Data()
        {
            var retData =await _dbContext.GetList<Chart4>("SELECT DATE_FORMAT(ExpireTime, '%Y-%m') AS expireMonth, SUM(CumulativeAmount) AS totalcumulativeAmount FROM fixeddeposits WHERE DataState IN (1, 2) AND TIMESTAMPDIFF(MONTH, NOW(), ExpireTime) <= 12 GROUP BY DATE_FORMAT(ExpireTime, '%Y-%m')");
            var retData2 =await _dbContext.GetList<Chart4_2>("SELECT DATE_FORMAT(CreateTime, '%Y-%m') AS createTimeMonth, SUM(Amounts) AS totalAmount FROM currentinterests WHERE Settled = 1 AND TIMESTAMPDIFF(MONTH, NOW(), CreateTime) <= 12 GROUP BY DATE_FORMAT(CreateTime, '%Y-%m')");
            return Json(new { status = 0, msg = "获取数据成功", data = retData, data2 = retData2 });
        }

        #region entity
        public class TotalObj
        {
            public decimal TotalCA { get; set; }
            public decimal TotalFA { get; set; }
        }

        public class TXQS
        {
            public string ApplyTime { get; set; }
            public decimal TotalAmount { get; set; }
        }
        public class TXQS2
        {
            public string ApplyTime { get; set; }
            public int TotalCount { get; set; }
        }
        public class Chart3
        {
            public int DiffMonth { get; set; }
            public decimal TotalAmount { get; set; }
        }
        public class Chart3Ret
        {
            public string DiffMonth { get; set; }
            public decimal TotalAmount { get; set; }
        }
        public class Chart4
        {
            public string ExpireMonth { get; set; }
            public decimal TotalcumulativeAmount { get; set; }
        }
        public class Chart4_2
        {
            public string CreateTimeMonth { get; set; }
            public decimal TotalAmount { get; set; }
        }
        #endregion
    }
}
