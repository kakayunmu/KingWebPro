using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using King.MVC.Models;
using Microsoft.Extensions.Caching.Memory;
using King.Domain.IRepositories;

namespace King.MVC.Controllers
{
    public class WagesRecordController : Controller
    {
        private EntityFrameworkCore.KingDBContext content;
        private IMemoryCache memoryCache;

        public WagesRecordController(EntityFrameworkCore.KingDBContext content, IMemoryCache memoryCache)
        {
            this.content = content;
            this.memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetWagesRecords([FromBody]QueryParam qparam)
        {
            var result = content.CurrentDeposits.Where(cd => cd.MType == 1).Join(
                content.Staffs, cd => cd.StaffId, sf => sf.Id, (cd, sf) => new WRModel
                {
                    StaffId = cd.StaffId,
                    StaffName = sf.Name,
                    StaffMobileNumber = sf.MobileNumber,
                    Amount = cd.Amount,
                    Remark = cd.Remarks,
                    CreateTime = cd.CreateTime

                }).OrderByDescending(cd => cd.CreateTime);

            return Json(new PageData<WRModel>()
            {
                Total = result.Count(),
                Rows = result.Skip(qparam.StartPage - 1).Take(qparam.PageSize)
            });

        }

        public class WRModel
        {
            public Guid StaffId { get; set; }
            public string StaffName { get; set; }
            public string StaffMobileNumber { get; set; }
            public decimal Amount { get; set; }
            public string Remark { get; set; }
            public DateTime CreateTime { get; set; }
        }
    }
}