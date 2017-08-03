using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using King.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Areas.API.Controllers
{
    [Produces("application/json")]
    public class MainController : BaseController
    {
        public MainController(IMemoryCache memoryCache, KingDBContext dbContent, ILogger<MainController> logger) : base(memoryCache, dbContent,logger)
        {
        }

        [HttpGet]
        public IActionResult GetTotalAmount()
        {
            var setting = dbContent.Settings.FirstOrDefault();
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
            var fixProds = dbContent.FixedProducts.Where(fp => fp.IsDel == 0 && fp.DataState == 0);
            return Json(new { status = 0, msg = "获取数据成功", fixedProducts = fixProds });
        }
        [HttpGet]
        public IActionResult GetCurrentDeposits(int pageIndex=1, int pageSize=0)
        {
            var currentDeposits = dbContent.CurrentDeposits.Where(cd => cd.StaffId == staff.Id).OrderByDescending(cd => cd.CreateTime);
            return Json(new
            {
                status = 0,
                msg = "获取数据成功",
                totalCount = currentDeposits.Count(),
                currentDeposits = currentDeposits.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            });
        }
        //转存
        [HttpPost]
        public IActionResult DumpFixed(Guid productId, decimal amount)
        {
            logger.LogDebug(string.Format("用户{0}转存固定{1}元 产品Id{2}", staff.Name, amount, productId));
            if (amount <= 0)
            {
                throw new Exception("转存金额不正确");
            }
            if (staff.CurrentAmount < amount)
            {
                throw new Exception("余额不足");
            }
            staff.CurrentAmount = staff.CurrentAmount - amount;
            staff.FixedAmount = staff.FixedAmount + amount;
            dbContent.Staffs.Update(staff);
            var fixedProduct = dbContent.FixedProducts.FirstOrDefault(fp => fp.Id == productId);
            var fixedDeposit = new Domain.WagesEnities.FixedDeposit()
            {
                Id = Guid.NewGuid(),
                AIRate = fixedProduct.AIRate,
                Amount = amount,
                DataState = 0,
                DumpTime = DateTime.Now,
                ExpireTime = fixedProduct.TimeLimitUnit == 0 ? DateTime.Now.AddDays(fixedProduct.TimeLimit) : DateTime.Now.AddMonths(fixedProduct.TimeLimit),
                Name = fixedProduct.Name,
                StaffId = staff.Id,
                TimeLimit = fixedProduct.TimeLimit,
                TimeLimitUnit = fixedProduct.TimeLimitUnit

            };
            dbContent.FixedDeposits.Add(fixedDeposit);
            var currentDeposit = new Domain.WagesEnities.CurrentDeposit()
            {
                Id = Guid.NewGuid(),
                Amount = amount,
                CreateTime = DateTime.Now,
                MType = 7,
                Remarks = string.Format("产品 [{0}]", fixedProduct.Name),
                StaffId = staff.Id
            };
            dbContent.CurrentDeposits.Add(currentDeposit);
            dbContent.SaveChanges();
            return Json(new { status = 0, msg = "转存成功" });
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
