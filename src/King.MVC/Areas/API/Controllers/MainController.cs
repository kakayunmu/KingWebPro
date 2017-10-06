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

        #region 获取总额
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
        #endregion

        #region 获取固定产品
        [HttpGet]
        public IActionResult GetFixedProducts(int pageIndex = 1, int pageSize = 10)
        {
            var fixProds = dbContent.FixedProducts.Where(fp => fp.IsDel == 0 && fp.DataState == 0).OrderByDescending(fp => fp.IsHot);
            return Json(new { status = 0, msg = "获取数据成功", fixedProducts = fixProds.Skip((pageIndex - 1) * pageSize).Take(pageSize) });
        }
        #endregion

        #region 获取最热产品
        [HttpGet]
        public IActionResult GetMaxHotFProduct()
        {
            var fixProd = dbContent.FixedProducts.OrderByDescending(fp => fp.IsHot).FirstOrDefault(fp => fp.IsDel == 0 && fp.DataState == 0);
            return Json(new { status = 0, msg = "获取数据成功", fixedProduct = fixProd });
        }
        #endregion

        #region 获取固定产品详情
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
        #endregion

        #region 获取活期交易记录
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
        #endregion

        #region 转存
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
                Remarks = string.Format("产品 [{0}] 期限[{1}] 年化[{2}]", fixedProduct.Name, fixedProduct.TimeLimitUnit == 0 ? fixedProduct.TimeLimit + "天" : fixedProduct.TimeLimit + "个月", string.Format("{0:#,##0.00}", fixedProduct.AIRate) + "%"),
                StaffId = staff.Id
            };
            dbContent.CurrentDeposits.Add(currentDeposit);
            dbContent.SaveChanges();
            return Json(new { status = 0, msg = "转存成功" });
        }
        #endregion

        #region 获取定存记录
        [HttpGet]
        public IActionResult GetFixeDeposits(int pageIndex, int pageSize)
        {
            var fixedDeposits = dbContent.FixedDeposits.Where(fd => fd.StaffId == staff.Id && fd.DataState == 0).OrderByDescending(fd => fd.DumpTime);
            return Json(new
            {
                status = 0,
                msg = "获取数据成功",
                fixedDeposits = fixedDeposits.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            });
        }
        #endregion

        #region 暂时无用了
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

        [HttpPost]
        public IActionResult DoPayment(Guid qrcode)
        {
            var paymentQR = dbContent.PaymentQRTmps.FirstOrDefault(pq => pq.Id == qrcode && pq.IsPay == false && (DateTime.Now - pq.CreateTime).Minutes <= 5);
            if (paymentQR == null)
            {
                return Json(new { status = -1, msg = "无效/失效的二维码" });
            }
            paymentQR.IsPay = true;
            paymentQR.ToStaffId = staff.Id;
            dbContent.PaymentQRTmps.Update(paymentQR);

            //payer
            var payerStaff = dbContent.Staffs.FirstOrDefault(sf => sf.Id == paymentQR.StaffId);
            payerStaff.CurrentAmount -= paymentQR.Amount;
            dbContent.Staffs.Update(payerStaff);

            dbContent.CurrentDeposits.Add(new Domain.WagesEnities.CurrentDeposit()
            {
                Id = Guid.NewGuid(),
                Amount = paymentQR.Amount * -1,
                CreateTime = DateTime.Now,
                MType = 5,
                StaffId = payerStaff.Id,
                Remarks = string.Format("转账给{0}", staff.Name)
            });

            //geter
            staff.CurrentAmount += paymentQR.Amount;
            dbContent.Staffs.Update(staff);

            dbContent.CurrentDeposits.Add(new Domain.WagesEnities.CurrentDeposit()
            {
                Id = Guid.NewGuid(),
                Amount = paymentQR.Amount,
                CreateTime = DateTime.Now,
                MType = 5,
                StaffId = staff.Id,
                Remarks = string.Format("收到 {0} 转账", payerStaff.Name)
            });
            dbContent.SaveChanges();

            //更新缓存
            string accessToken = memoryCache.Get<string>(payerStaff.Id);
            if (!string.IsNullOrEmpty(accessToken))
            {
                memoryCache.Set(payerStaff.Id, accessToken, new TimeSpan(4, 0, 0));
                memoryCache.Set(accessToken, payerStaff, new TimeSpan(4, 0, 0));
            }
            accessToken = memoryCache.Get<string>(staff.Id);
            if (!string.IsNullOrEmpty(accessToken))
            {
                memoryCache.Set(staff.Id, accessToken, new TimeSpan(4, 0, 0));
                memoryCache.Set(accessToken, staff, new TimeSpan(4, 0, 0));
            }

            return Json(new { status = 0, msg = "操作成功" });
        }
        #endregion

        #region 提现
        [HttpPost]
        public IActionResult Withdrawals(string payPwd, decimal amount)
        {
            var payPwdMd5 = Utility.Security.Encryption.Md5WithSalt("KingWeb", payPwd);
            if (amount < 1)
            {
                return Json(new { status = -1, msg = "最小提现金额为1元" });
            }
            if (staff.PaymentPwd != payPwdMd5)
            {
                return Json(new { status = -1, msg = "交易密码不正确！" });
            }
            if (staff.CurrentAmount < amount)
            {
                return Json(new { status = -1, msg = "余额不足" });
            }
            staff.CurrentAmount -= amount;
            dbContent.Staffs.Update(staff);
            dbContent.WithdrawalsApplys.Add(new Domain.WagesEnities.WithdrawalsApply()
            {
                Amount = amount,
                ApplyState = 0,
                ApplyTime = DateTime.Now,
                Id = Guid.NewGuid(),
                PayState = 0,
                StaffId = staff.Id
            });
            dbContent.SaveChanges();
            return Json(new { status = 0, msg = "提现已发送申请" });
        }
        #endregion

        #region 固存转活期
        [HttpPost]
        public IActionResult FixedToCurrent(Guid fdId)
        {
            var fixedDeposit = dbContent.FixedDeposits.FirstOrDefault(fd => fd.Id == fdId);
            if (fixedDeposit == null)
            {
                return Json(new { status = -1, msg = "未找到定存记录" });
            }
            var setting = dbContent.Settings.FirstOrDefault();
            decimal shouyi = setting.GeneralInterestRate / 100 / 12 / 30 * fixedDeposit.Amount * (DateTime.Now - fixedDeposit.DumpTime).Days;
            fixedDeposit.DataState = 2;
            fixedDeposit.Remarks = string.Format("固存转活期 {0}天 年化{1} 实际收益 {2}元", (DateTime.Now - fixedDeposit.DumpTime).Days, string.Format("{0:#,##0.00}",setting.GeneralInterestRate),string.Format("{0:#,##0.00}", shouyi));
            fixedDeposit.CumulativeAmount = shouyi;
            dbContent.FixedDeposits.Update(fixedDeposit);

            staff.FixedAmount -= fixedDeposit.Amount;
            staff.CurrentAmount += fixedDeposit.Amount + fixedDeposit.CumulativeAmount;
            dbContent.Staffs.Update(staff);

            dbContent.CurrentDeposits.Add(new Domain.WagesEnities.CurrentDeposit()
            {
                Id = Guid.NewGuid(),
                Amount = fixedDeposit.CumulativeAmount,
                CreateTime = DateTime.Now,
                MType = 6,
                StaffId = staff.Id,
                Remarks = fixedDeposit.Remarks,
                JsonObj = Newtonsoft.Json.JsonConvert.SerializeObject(fixedDeposit)
            });

            dbContent.SaveChanges();

            //刷新缓存
            var accessToken = memoryCache.Get<string>(staff.Id);
            if (!string.IsNullOrEmpty(accessToken))
            {
                memoryCache.Set(staff.Id, accessToken, new TimeSpan(4, 0, 0));
                memoryCache.Set(accessToken, staff, new TimeSpan(4, 0, 0));
            }

            return Json(new { status=0,msg="处理成功"});
        }
        #endregion

        #region 扫一扫转账
        [HttpPost]
        public IActionResult ScanPay(Guid targetStaffId, decimal payMoney)
        {
            if (payMoney <= 0)
            {
                return Json(new { status = -1, msg = "金额不能小于零" });
            }
            if (staff.CurrentAmount < payMoney)
            {
                return Json(new { status = -1, msg = "余额不足" });
            }
            var targetStaff = dbContent.Staffs.FirstOrDefault(sf => sf.Id == targetStaffId);//目标员工
            if (targetStaff == null)
            {
                return Json(new { status = -1, msg = "未找到转账对象" });
            }
            targetStaff.CurrentAmount += payMoney;
            dbContent.Update(targetStaff);

            staff.CurrentAmount -= payMoney;
            dbContent.Update(staff);

            dbContent.CurrentDeposits.Add(new Domain.WagesEnities.CurrentDeposit()
            {
                Id = Guid.NewGuid(),
                Amount = payMoney * -1,
                CreateTime = DateTime.Now,
                MType = 5,
                StaffId = staff.Id,
                Remarks = string.Format("转账给{0}", targetStaff.Name)
            });

            dbContent.CurrentDeposits.Add(new Domain.WagesEnities.CurrentDeposit()
            {
                Id = Guid.NewGuid(),
                Amount = payMoney,
                CreateTime = DateTime.Now,
                MType = 5,
                StaffId = targetStaff.Id,
                Remarks = string.Format("收到 {0} 转账", staff.Name)
            });
            dbContent.SaveChanges();

            //更新缓存
            string accessToken = memoryCache.Get<string>(targetStaff.Id);
            if (!string.IsNullOrEmpty(accessToken))
            {
                memoryCache.Set(targetStaff.Id, accessToken, new TimeSpan(4, 0, 0));
                memoryCache.Set(accessToken, targetStaff, new TimeSpan(4, 0, 0));
            }
            accessToken = memoryCache.Get<string>(staff.Id);
            if (!string.IsNullOrEmpty(accessToken))
            {
                memoryCache.Set(staff.Id, accessToken, new TimeSpan(4, 0, 0));
                memoryCache.Set(accessToken, staff, new TimeSpan(4, 0, 0));
            }

            return Json(new { status = 0, msg = "操作成功" });
        }
        #endregion

    }

    #region 返回值实体类
    public class TotalAmountModel
    {
        public decimal GeneralInterestRate { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
    #endregion
}
