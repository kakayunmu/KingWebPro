using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using King.MVC.Models;
using King.Domain.IRepositories;
using King.Domain.WagesEnities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    public class WithdrawalsApplyController : Controller
    {
        private King.EntityFrameworkCore.KingDBContext content;
        public WithdrawalsApplyController(EntityFrameworkCore.KingDBContext content)
        {
            this.content = content;
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
            var was = content.WithdrawalsApplys.Where(wa => ids.Contains(wa.Id.ToString()));
            foreach (var item in was)
            {
                item.ApplyState = applyState;
            }
            content.WithdrawalsApplys.UpdateRange(was);
            content.SaveChanges();
            return Json(new { status = 0, msg = "操作成功" });
        }

        public class WithdrawalsApply_Staff : WithdrawalsApply
        {
            public string StaffName { get; set; }
            public string StaffMobileNumber { get; set; }
        }
    }

}
