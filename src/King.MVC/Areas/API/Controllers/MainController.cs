using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using King.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Areas.API.Controllers
{
    [Produces("application/json")]
    public class MainController : BaseController
    {
        private KingDBContext content;
        public MainController(KingDBContext content, IMemoryCache memoryCache) : base(memoryCache)
        {
            this.content = content;
        }

        [HttpGet]
        public IActionResult GetTotalAmount()
        {
            var setting = content.Settings.FirstOrDefault();
            return Json(new
            {
                status = 0,
                msg = "获取数据成功",
                totalAmountData = new TotalAmountModel()
                {
                    GeneralInterestRate = setting.GeneralInterestRate,
                    CurrentAmount = staff.CurrentAmount,
                    FixedAmount = staff.FixedAmount,
                    TotalAmount = staff.CurrentAmount + staff.FixedAmount
                }
            });
        }
        [HttpGet]
        public IActionResult GetFixedProducts()
        {
            var fixProds = content.FixedProducts.Where(fp => fp.IsDel == 0 && fp.DataState == 0);
            return Json(new { status = 0, msg = "获取数据成功", fixedProducts = fixProds });
        }
        [HttpGet]
        public IActionResult GetCurrentDeposits(int pageIndex, int pageSize)
        {
            var currentDeposits = content.CurrentDeposits.Where(cd => cd.StaffId == staff.Id).OrderByDescending(cd => cd.CreateTime);
            return Json(new
            {
                status = 0,
                msg = "获取数据成功",
                totalCount = currentDeposits.Count(),
                currentDeposits = currentDeposits.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            });
        }

        
    }

    public class TotalAmountModel
    {
        public decimal GeneralInterestRate { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
