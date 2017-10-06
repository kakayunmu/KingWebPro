using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using King.MVC.Models;
using King.Domain.IRepositories;
using King.Domain.WagesEnities;
using Microsoft.Extensions.Caching.Memory;
using King.Utility.Extended;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    public class WithdrawalsApplyController : Controller
    {
        private King.EntityFrameworkCore.KingDBContext content;
        private IMemoryCache memoryCache;
        public WithdrawalsApplyController(EntityFrameworkCore.KingDBContext content,IMemoryCache memoryCache)
        {
            this.content = content;
            this.memoryCache = memoryCache;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetWithdrawalsApplys([FromBody]QueryParam qparam)
        {
            var result = content.WithdrawalsApplys
                .Join(content.Staffs,
                wa => wa.StaffId,
                sf => sf.Id,
                (wa, sf) => new WithdrawalsApply_Staff()
                {
                    Id = wa.Id,
                    StaffId = wa.StaffId,
                    Amount = wa.Amount,
                    ApplyState = wa.ApplyState,
                    ApplyTime = wa.ApplyTime,
                    Auditor = wa.Auditor,
                    AuditorTime = wa.AuditorTime,
                    StaffName = sf.Name,
                    AlipayAccount=sf.AlipayAccount,
                    StaffMobileNumber = sf.MobileNumber
                })
                .OrderBy(wa => wa.ApplyState)
                .OrderByDescending(wa => wa.ApplyTime);


            return Json(new PageData<WithdrawalsApply_Staff>()
            {
                Total = result.Count(),
                Rows = result.Skip(qparam.StartPage - 1).Take(qparam.PageSize)
            });
        }

        public IActionResult DoAudit(string[] ids, int applyState)
        {
            var user = content.Users.FirstOrDefault(u=>u.Id== Guid.Parse(User.GetClaimVal(ClaimTypes.NameIdentifier)));
            var was = content.WithdrawalsApplys.Where(wa => ids.Contains(wa.Id.ToString()));
            foreach (var item in was)
            {
                item.ApplyState = applyState;
                item.Auditor = user.Name;
                item.AuditorTime = DateTime.Now;
                if (applyState == 1)
                {
                    item.PayState = 2;
                    content.CurrentDeposits.Add(new CurrentDeposit()
                    {
                        Amount = item.Amount,
                        CreateTime = DateTime.Now,
                        Id = Guid.NewGuid(),
                        JsonObj = Newtonsoft.Json.JsonConvert.SerializeObject(item),
                        MType = 4,
                        Remarks = "支付宝提现",
                        StaffId = item.StaffId
                    });                   
                }
                else if(applyState==2)
                {
                    item.PayState = 3;
                    
                    var targetStaff = content.Staffs.FirstOrDefault(sf => sf.Id == item.StaffId);
                    targetStaff.CurrentAmount += item.Amount;                   
                    content.Staffs.Update(targetStaff);
                    //更新缓存
                    string accessToken = memoryCache.Get<string>(targetStaff.Id);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        memoryCache.Set(targetStaff.Id, accessToken, new TimeSpan(4, 0, 0));
                        memoryCache.Set(accessToken, targetStaff, new TimeSpan(4, 0, 0));
                    }
                }
            }
            content.WithdrawalsApplys.UpdateRange(was);
            content.SaveChanges();
            return Json(new { status = 0, msg = "操作成功" });
        }
        public class WithdrawalsApply_Staff : WithdrawalsApply
        {
            public string StaffName { get; set; }
            public string StaffMobileNumber { get; set; }
            public string AlipayAccount { get; set; }
        }
    }

}
