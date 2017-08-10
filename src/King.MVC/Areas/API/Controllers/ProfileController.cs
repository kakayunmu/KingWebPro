﻿using System;
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
        //获取固定记录
        [HttpGet]
        public IActionResult GetFixedDeposit(int pageIndex = 1, int pageSize = 10)
        {
            var fixedDeposits = dbContent.FixedDeposits.Where(fd => fd.StaffId == staff.Id).OrderByDescending(fd => fd.DumpTime);
            return Json(new { status = 0, msg = "获取数据成功", data = fixedDeposits.Skip((pageIndex - 1) * pageSize).Take(pageSize) });

        }
        [HttpGet]
        public IActionResult GetCurrentDeposit(int mType, int pageIndex = 1, int pageSize = 10)
        {

            var currentDeposit = dbContent.CurrentDeposits.Where(cd => cd.StaffId == staff.Id && (mType == 0 ? true : cd.MType == mType))
                .OrderByDescending(cd => cd.CreateTime);
            var retData = currentDeposit.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return Json(new { status = 0, msg = "获取数据成功", data = retData });
        }
        //修改头像
        //public IActionResult ChangeHeadImg()
        //{
        //   var files= Request.Form.Files;
        //    if (files!=null&&files.Count > 0)
        //    {

        //    }
        //}
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
                return Json(new { status=0,msg="设置支付宝账号成功"});
            }
            else
            {
                return Json(new { status=-1,msg="验证码不正确"});
            }
        }
    }
}
