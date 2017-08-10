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
        public MainController(IMemoryCache memoryCache, KingDBContext dbContent, ILogger<MainController> logger) : base(memoryCache, dbContent, logger)
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
        public IActionResult GetFixedProducts(int pageIndex = 1, int pageSize = 10)
        {
            var fixProds = dbContent.FixedProducts.Where(fp => fp.IsDel == 0 && fp.DataState == 0).OrderByDescending(fp => fp.IsHot);
            return Json(new { status = 0, msg = "获取数据成功", fixedProducts = fixProds.Skip((pageIndex - 1) * pageSize).Take(pageSize) });
        }
        [HttpGet]
        public IActionResult GetMaxHotFProduct()
        {
            var fixProd = dbContent.FixedProducts.OrderByDescending(fp => fp.IsHot).FirstOrDefault(fp => fp.IsDel == 0 && fp.DataState == 0);
            return Json(new { status = 0, msg = "获取数据成功", fixedProduct = fixProd });
        }

        [HttpGet]
        public IActionResult GetFixedProduct(Guid proId)
        {
            var fixPro = dbContent.FixedProducts.FirstOrDefault(fp => fp.Id == proId);
            if (fixPro != null)
            {
                return Json(new { status = 0, msg = "获取数据成功", fixedProduct = fixPro, currentAmount = staff.CurrentAmount });
            }
            else
            {
                return Json(new { status = -1, msg = "未能找到对应产品" });
            }
        }
        [HttpGet]
        public IActionResult GetCurrentDeposits(int pageIndex = 1, int pageSize = 10)
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
            var fixedProduct = dbContent.FixedProducts.FirstOrDefault(fp => fp.Id == productId && fp.IsDel == 0 && fp.DataState == 0);
            if (fixedProduct == null)
            {
                throw new Exception("产品不存在");
            }
            var fixedDeposit = new Domain.WagesEnities.FixedDeposit()
            {
                Id = Guid.NewGuid(),
                AIRate = fixedProduct.AIRate,
                Amount = amount * -1,
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
                Remarks = string.Format("产品 [{0}] 期限[{1}] 年化[{2}]", fixedProduct.Name, fixedProduct.TimeLimitUnit == 0 ? fixedProduct.TimeLimit + "天" : fixedProduct.TimeLimit + "个月", string.Format( "{0:#,##0.00}",fixedProduct.AIRate) + "%"),
                StaffId = staff.Id
            };
            dbContent.CurrentDeposits.Add(currentDeposit);
            dbContent.SaveChanges();
            return Json(new { status = 0, msg = "转存成功" });
        }

        [HttpPost]
        public IActionResult CreateQRCode(decimal amount)
        {
            if (amount <= 0)
            {
                return Json(new { status = -1, msg = "金额必须大于0" });
            }
            var paymentQR = new Domain.WagesEnities.PaymentQRTmp()
            {
                Id = Guid.NewGuid(),
                Amount = amount,
                IsPay = false,
                CreateTime = DateTime.Now,
                StaffId = staff.Id
            };
            dbContent.PaymentQRTmps.Add(paymentQR);
            dbContent.SaveChanges();
            return Json(new { status = 0, msg = "生成付款码成功", qrcode = paymentQR.Id });
        }

        //public IActionResult DoPayment(Guid qrcode)
        //{
        //    var paymentQR = dbContent.PaymentQRTmps.FirstOrDefault(pq => pq.Id == qrcode && pq.IsPay == false && (DateTime.Now - pq.CreateTime).Minutes <= 5);
        //    if (paymentQR == null)
        //    {
        //        return Json(new { status = -1, msg = "" });
        //    }
        //    paymentQR.IsPay = true;
        //    paymentQR.ToStaffId = staff.Id;
        //    dbContent.PaymentQRTmps.Update(paymentQR);

        //    //payer
        //    var payerStaff = dbContent.Staffs.FirstOrDefault(sf => sf.Id == paymentQR.StaffId);
        //    payerStaff.CurrentAmount -= paymentQR.Amount;
        //    dbContent.Staffs.Update(payerStaff);

        //    dbContent.CurrentDeposits.Add(new Domain.WagesEnities.CurrentDeposit()
        //    {
        //        Id = Guid.NewGuid(),
        //        Amount = paymentQR.Amount,
        //        CreateTime = DateTime.Now,
        //        MType = 5,
        //        StaffId = payerStaff.Id,
        //        Remarks = string.Format("转账给{0}", staff.Name)
        //    });

        //}

    }

    public class TotalAmountModel
    {
        public decimal GeneralInterestRate { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
