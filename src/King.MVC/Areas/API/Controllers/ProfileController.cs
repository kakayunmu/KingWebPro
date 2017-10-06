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
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Areas.API.Controllers
{
    [Produces("application/json")]
    public class ProfileController : BaseController
    {
        public ProfileController(IMemoryCache memoryCache, KingDBContext dbContent, ILogger<ProfileController> logger) : base(memoryCache, dbContent, logger)
        {
        }

        //获取用户信息
        [HttpGet]
        public IActionResult GetStaffInfo()
        {
            return Json(new { status = 0, msg = "获取数据成功", staff = staff });
        }
        //生成用户二维码
        [HttpGet]
        public IActionResult GetStaffQR([FromServices]IHostingEnvironment env)
        {
            string fileName = Path.Combine("qrcode", string.Format("{0}_{1}.png", "in", staff.Id));
            if (!System.IO.File.Exists(Path.Combine(env.WebRootPath, fileName)))
            {
                if (!Directory.Exists(Path.Combine(env.WebRootPath, "qrcode")))
                {
                    Directory.CreateDirectory(Path.Combine(env.WebRootPath, "qrcode"));
                }
                var height = 500;
                var widht = 500;
                var margin = 0;
                var Options = new ZXing.Common.EncodingOptions()
                {
                    Height = height,
                    Width = widht,
                    Margin = margin
                };
                Options.Hints.Add( ZXing.EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
                var qrCodeWriter = new ZXing.BarcodeWriterPixelData()
                {
                    Format = ZXing.BarcodeFormat.QR_CODE,
                    Options =Options
                };
               

                var pixelData = qrCodeWriter.Write("{\"type\":\"in\",\"data\":\""+ staff.Id + "\"}");
                using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                {
                    using (var ms = new FileStream(Path.Combine(env.WebRootPath,fileName), FileMode.OpenOrCreate))
                    {
                        var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                                                         System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        try
                        {
                            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                        }
                        finally
                        {
                            bitmap.UnlockBits(bitmapData);
                        }
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Flush();
                    }
                }
            }
            return Json(new { status=0,msg="获取数据成功",qrurl= string.Format("qrcode/{0}_{1}.png", "in", staff.Id), headImg = staff.HeadImg});
        }
        //获取固定记录
        [HttpGet]
        public IActionResult GetFixedDeposit(int pageIndex = 1, int pageSize = 10)
        {

            var totalFDAmount = dbContent.FixedDeposits.Where(fd => fd.DataState == 0 && fd.StaffId == staff.Id).Sum(fd => fd.Amount);
            var totalFIAmount = dbContent.FixedDeposits.Where(fd =>  fd.StaffId == staff.Id).Sum(fd => fd.CumulativeAmount);
            var fixedDeposits = dbContent.FixedDeposits.Where(fd => fd.StaffId == staff.Id)
                .Select(fd => new
                {
                    Id = fd.Id,
                    AIRate = fd.AIRate,
                    Amount = fd.Amount,
                    DataState = fd.DataState,
                    ExpireTime = fd.ExpireTime,
                    Name = fd.Name,
                    FIAmount = fd.CumulativeAmount
                })
                .OrderBy(fd => fd.DataState)
                .OrderByDescending(fd => fd.ExpireTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return Json(new
            {
                status = 0,
                msg = "获取数据成功",
                data = fixedDeposits.ToList().Select(fd => new
                {
                    Id = fd.Id,
                    AIRate = fd.AIRate,
                    Amount = fd.Amount,
                    DataState = fd.DataState,
                    DumpTime = fd.ExpireTime,
                    Name = fd.Name,
                    FIAmount = fd.FIAmount,
                    RemDay = (fd.ExpireTime - DateTime.Now).Days
                }),
                totalFDAmount = totalFDAmount,
                totalFIAmount = totalFIAmount
            });

        }
        ///获取定存产品明细
        [HttpGet]
        public IActionResult GetFiexedDepositInfo(Guid fdId)
        {
            var retObj = dbContent.FixedDeposits.FirstOrDefault(fd => fd.Id == fdId && fd.StaffId == staff.Id);
            return Json(new { status = 0, msg = "获取数据成功", data = retObj });
        }
        //按照类别获取活期记录
        [HttpGet]
        public IActionResult GetCurrentDeposit(int mType, int pageIndex = 1, int pageSize = 10)
        {

            var currentDeposit = dbContent.CurrentDeposits.Where(cd => cd.StaffId == staff.Id && (mType == 0 ? true : cd.MType == mType))
                .OrderByDescending(cd => cd.CreateTime);
            var retData = currentDeposit.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return Json(new { status = 0, msg = "获取数据成功", data = retData });
        }
        //修改头像
        [HttpPost]
        public IActionResult ChangeHeadImg([FromServices]IHostingEnvironment env)
        {

            var files = Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                string filePath = Path.Combine("upload", Guid.NewGuid().ToString() + ".jpg");
                logger.LogDebug("文件名称" + files[0].FileName);
                logger.LogDebug("路径" + Path.Combine(env.WebRootPath, filePath));
                using (var fileStream = new FileStream(Path.Combine(env.WebRootPath, filePath), FileMode.CreateNew))
                {
                    files[0].CopyTo(fileStream);
                }


                staff.HeadImg = filePath;
                dbContent.Staffs.Update(staff);
                dbContent.SaveChanges();
                return Json(new { status = 0, msg = "头像上传成功", url = filePath });
            }
            else
            {
                logger.LogDebug("未能获取到文件");
            }
            return Json(new { status = -1, msg = "头像上传失败" });
        }
        //修改手机号
        [HttpPost]
        public IActionResult ModifyStaffMobile(string vcode, string mobile)
        {
            var mobileVCode = memoryCache.Get<BasicsController.MobileVCode>(staff.MobileNumber);
            if (mobileVCode != null && mobileVCode.vcode == vcode)
            {
                staff.MobileNumber = mobile;
                dbContent.Staffs.Update(staff);
                dbContent.SaveChanges();
                return Json(new { status = 0, msg = "修改手机号码成功" });
            }
            else
            {
                return Json(new { status = -1, msg = "验证码不正确" });
            }
        }
        //修改姓名
        [HttpPost]
        public IActionResult ModifyStaffName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new { status = -1, msg = "姓名不能为空" });
            }
            staff.Name = name;
            dbContent.Update(staff);
            dbContent.SaveChanges();
            return Json(new { status = 0, msg = "修改姓名成功" });
        }
        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModifyStaffPassword(string oldPwd, string newPwd)
        {
            if (staff.Password == Utility.Security.Encryption.Md5WithSalt("KingWeb", oldPwd))
            {
                staff.Password = Utility.Security.Encryption.Md5WithSalt("KingWeb", newPwd);
                dbContent.Update(staff);
                dbContent.SaveChanges();
                return Json(new { status = 0, msg = "修改密码成功" });
            }
            else
            {
                return Json(new { status = -1, msg = "旧密码不正确" });
            }
        }
        [HttpPost]
        public IActionResult ModifyStaffPaymentPwd(string vcode, string pwd)
        {
            var mobileVCode = memoryCache.Get<BasicsController.MobileVCode>(staff.MobileNumber);
            if (string.IsNullOrEmpty(pwd))
            {
                return Json(new { status = -1, msg = "支付密码不能为空" });
            }
            Regex reg = new Regex(@"^\d{6}$");
            if (!reg.IsMatch(pwd))
            {
                return Json(new { status = -1, msg = "支付密码必须为6位数字组成" });
            }
            if (mobileVCode != null && mobileVCode.vcode == vcode)
            {
                staff.PaymentPwd = Utility.Security.Encryption.Md5WithSalt("KingWeb", pwd);
                dbContent.Update(staff);
                dbContent.SaveChanges();
                return Json(new { status = 0, msg = "设置支付密码成功" });
            }
            else
            {
                return Json(new { status = -1, msg = "验证码不正确" });
            }
        }
        /// <summary>
        /// 设置支付宝账号
        /// </summary>
        /// <param name="vcode">验证码</param>
        /// <param name="alipayAccount">支付宝账号</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModifyStaffAlipayAccount(string vcode, string alipayAccount)
        {
            if (string.IsNullOrEmpty(alipayAccount))
            {
                return Json(new { status = -1, msg = "支付宝账号不能为空" });
            }
            var mobileVCode = memoryCache.Get<BasicsController.MobileVCode>(staff.MobileNumber);
            if (mobileVCode != null && mobileVCode.vcode == vcode)
            {
                staff.AlipayAccount = alipayAccount;
                dbContent.Update(staff);
                dbContent.SaveChanges();
                return Json(new { status = 0, msg = "设置支付宝账号成功" });
            }
            else
            {
                return Json(new { status = -1, msg = "验证码不正确" });
            }
        }
        //提现记录
        [HttpGet]
        public IActionResult GetWithdrawalsApplys(int pageIndex = 1, int pageSize = 15)
        {
            var withdrawalsApplys = dbContent.WithdrawalsApplys.Where(wa => wa.StaffId == staff.Id)
                .OrderByDescending(wa => wa.ApplyTime);
            return Json(new { status = 0, msg = "获取数据成功", data = withdrawalsApplys.Skip((pageIndex - 1) * pageSize).Take(pageSize) });
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult OutLogin()
        {
            memoryCache.Remove(accessToken);
            memoryCache.Remove(staff.Id);
            return Json(new { status = 0, msg = "退出登录成功" });
        }


    }
}
