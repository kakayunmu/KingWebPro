using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using King.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.IO;
using Microsoft.Extensions.Logging;

namespace King.MVC.Areas.API.Controllers
{
    [Produces("application/json")]
    public class BasicsController : BaseController
    {
        public BasicsController(IMemoryCache memoryCache, KingDBContext dbContent, ILogger<BasicsController> logger) : base(memoryCache, dbContent,logger)
        {
        }

        [HttpPost]
        public IActionResult Login(string mobile, string pwd)
        {
            logger.LogDebug(string.Format("用户{0}登录ing",mobile));
            var ret = dbContent.Staffs.FirstOrDefault(f => f.MobileNumber == mobile && f.Password == Utility.Security.Encryption.Md5WithSalt("KingWeb", pwd));
            if (ret != null)
            {
                string accessToken = Guid.NewGuid().ToString();
                memoryCache.Set(accessToken, ret, new TimeSpan(4, 0, 0));
                memoryCache.Set(ret.Id, accessToken, new TimeSpan(4, 0, 0));
                ret.RefToken = Guid.NewGuid().ToString();
                dbContent.Staffs.Update(ret);
                dbContent.SaveChanges();
                logger.LogDebug(string.Format("用户{0}登录成功", mobile));
                return Json(new { status = 0, msg = "登录成功", accessToken = accessToken, staff = ret });

            }
            else
            {
                return Json(new { status = -1, msg = "密码不正确" });
            }

        }

        [HttpPost]
        public IActionResult LoginWithVCode(string mobile, string vcode)
        {
            var mvcode = memoryCache.Get<MobileVCode>(mobile);
            if (mvcode != null && mvcode.vcode == vcode)
            {
                var ret = dbContent.Staffs.FirstOrDefault(f => f.MobileNumber == mobile);
                if (ret != null)
                {
                    string accessToken = Guid.NewGuid().ToString();
                    memoryCache.Set(accessToken, ret, new TimeSpan(4, 0, 0));
                    memoryCache.Set(ret.Id, accessToken);
                    ret.RefToken = Guid.NewGuid().ToString();
                    dbContent.Staffs.Update(ret);
                    dbContent.SaveChanges();
                    return Json(new { status = 0, msg = "登录成功", accessToken = accessToken, staff = ret });
                }
            }
            else
            {
                return Json(new { status = -1, msg = "验证码无效" });
            }
            return null;
        }
        [HttpGet]
        public IActionResult LoginOut()
        {
            memoryCache.Remove(accessToken);
            return Json(new { status = 0, msg = "退出登录成功" });
        }

        [HttpPost]
        public IActionResult RefAccessToken(string refToken)
        {
            string accessToken = Guid.NewGuid().ToString();
            var staff = dbContent.Staffs.FirstOrDefault(f => f.RefToken == refToken);
            if (staff != null)
            {
                memoryCache.Set(accessToken,staff);
                memoryCache.Set(staff.Id, accessToken);
                return Json(new { status = 0, msg = "刷新Token成功", accessToken = accessToken, staff = staff });
            }
            else
            {
                return Json(new { status = -1, msg = "refToken无效" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendLoginVcode(string mobile)
        {
            var staff = dbContent.Staffs.FirstOrDefault(f => f.MobileNumber == mobile);
            if (staff == null)
            {
                return Json(new { status = -1, msg = "该手机号尚未登记为员工" });
            }
            else
            {
                var mobileVcode = memoryCache.Get<MobileVCode>(mobile);
                if (mobileVcode != null)
                {
                    return Json(new { status = -1, msg = "发送验证频率太频繁！发送失败" });
                }
                else
                {
                    int rcode = new Random().Next(1000, 9999);
                    Dictionary<string, string> smsParame = new Dictionary<string, string>();
                    smsParame.Add("smscode", rcode.ToString());
                    RequestParame rp = new RequestParame()
                    {
                        Mobile = mobile,
                        SMSParam = smsParame,
                        Config = new ALYSMSConfig()
                        {
                            AccessId = "LTAIQ0WptV3WGYUC",
                            AccessKey = "XFVIKRAYc7hIsxeebioEXl2QQwpTay",
                            RegionEndpoint = "https://1227674209450822.mns.cn-hangzhou.aliyuncs.com/",                            
                            TopicName = "sms.topic-cn-hangzhou",
                            SignName = "员工平台",
                            TemplateCode = "SMS_48690112"
                        }
                    };
                    HttpWebResponse response = await King.Utility.HttpUtil.HttpWebHelper.PostWithJson("http://47.93.53.76:8088/api/SMS/PostSendSMS", rp) as HttpWebResponse;

                    string retString = "";
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(stream);
                        retString = sr.ReadToEnd();
                    }
                    var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<RetClass>(retString);
                    if (ret.Status == 0)
                    {
                        memoryCache.Set<MobileVCode>(mobile, new MobileVCode() { mobile = mobile, vcode = rcode.ToString() }, new TimeSpan(0, 1, 0));
                    }

                    #region 测试代码
                    //int rcode = 1234;
                    //memoryCache.Set<MobileVCode>(mobile, new MobileVCode() { mobile = mobile, vcode = rcode.ToString() }, new TimeSpan(0, 1, 0));
                    //var ret = new { Status = 0, Msg = "验证码发送成功" };
                    #endregion
                    return Json(new { status = ret.Status, msg = ret.Msg });
                }
            }
        }
        [HttpPost]
        public IActionResult FindPwd(string mobile, string vCode, string newPwd)
        {
            var mobileVCode = memoryCache.Get<MobileVCode>(mobile);
            if (mobileVCode != null && mobileVCode.vcode == vCode)
            {
                var staff = dbContent.Staffs.FirstOrDefault(f => f.MobileNumber == mobile);
                if (staff != null)
                {
                    staff.Password = Utility.Security.Encryption.Md5WithSalt("KingWeb", newPwd);
                    dbContent.Staffs.Update(staff);
                    dbContent.SaveChanges();
                    return Json(new { status = 0, msg = "找回密码成功" });
                }
                else
                {
                    return Json(new { status = -1, msg = "手机号尚未注册" });
                }
            }
            else
            {
                return Json(new { status = -1, msg = "验证码不正确" });
            }
        }

        [HttpPost]
        public IActionResult ChangePwd(string newPwd)
        {
            staff.Password = Utility.Security.Encryption.Md5WithSalt("KingWeb", newPwd);
            dbContent.Staffs.Update(staff);
            memoryCache.Remove(accessToken);
            dbContent.SaveChanges();
            return Json(new { status = 0, msg = "密码修改成功" });
        }
        public class MobileVCode
        {
            public string mobile { get; set; }
            public string vcode { get; set; }
        }


        public class RequestParame
        {
            public ALYSMSConfig Config { get; set; }
            public string Mobile { get; set; }
            public Dictionary<string, string> SMSParam { get; set; }
        }
        public class ALYSMSConfig
        {
            public string AccessId
            {
                get; set;
            }
            public string AccessKey
            {
                get; set;
            }
            public string RegionEndpoint
            {
                get; set;
            }
            public string TopicName
            {
                get; set;
            }
            public string SignName
            {
                get; set;
            }
            public string TemplateCode
            {
                get; set;
            }
        }

        public class RetClass
        {
            public int Status { get; set; }
            public string Msg { get; set; }
        }

    }
}