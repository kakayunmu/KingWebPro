using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pomelo.AspNetCore.TimedJob;
using Microsoft.Extensions.Logging;
using King.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace King.MVC.Jobs
{
    public class FixedJob : Job
    {
        private ILogger<FixedJob> logger;
        private KingDBContext dbContext;
        private IMemoryCache memoryCache;
        public FixedJob(ILogger<FixedJob> logger, KingDBContext dbContext, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.memoryCache = memoryCache;
        }
        [Invoke(Begin = "2017-08-09 00:00:00", Interval = 1000 * 60 * 10, SkipWhileExecuting = true)]
        public void Run()
        {
            logger.LogDebug("执行了FixedJob");
            string sqlStr = "INSERT INTO fixedinterests SELECT UUID(), a.AIRate * 1.0 / 365 / 100 * Amount, NOW(), a.Id, 0 FROM fixeddeposits a LEFT JOIN (SELECT * FROM fixedinterests WHERE DATEDIFF(CreateTime, NOW()) = 0 ) b ON a.id = b.FixedDepositId WHERE b.id IS NULL AND a.DataState = 0 AND DATEDIFF(NOW(), a.DumpTime) > 0 AND DATE_FORMAT(a.DumpTime, '%H%i%s') < DATE_FORMAT(NOW(), '%H%i%s')";
            dbContext.Database.ExecuteSqlCommand(sqlStr);
            string sqlStr2 = "UPDATE fixeddeposits a INNER JOIN (SELECT FixedDepositId, SUM(Amounts) AS Amounts FROM fixedinterest WHERE Settled = 0 GROUP BY FixedDepositId ) b ON a.id = b.FixedDepositId SET a.CumulativeAmount = a.CumulativeAmount + b.Amounts WHERE a.DataState = 0";
            dbContext.Database.ExecuteSqlCommand(sqlStr2);
            dbContext.Database.ExecuteSqlCommand("UPDATE fixedinterest SET Settled = 1 WHERE Settled = 0");
            var endFixedDiposits = dbContext.FixedDeposits.Where(fd => fd.DataState == 0 && fd.ExpireTime < DateTime.Now);
            foreach (var item in endFixedDiposits)
            {
                DoEndFD(item);
            }
        }

        public void DoEndFD(Domain.WagesEnities.FixedDeposit fd)
        {
            fd.DataState = 1;
            dbContext.FixedDeposits.Update(fd);
            var staff = dbContext.Staffs.FirstOrDefault(sf => sf.Id == fd.StaffId);
            staff.CurrentAmount = staff.CurrentAmount + fd.Amount + fd.CumulativeAmount;
            staff.FixedAmount = staff.FixedAmount - fd.Amount;
            dbContext.Staffs.Update(staff);
            var accessToken = memoryCache.Get<string>(staff.Id);
            if (!string.IsNullOrEmpty(accessToken))
            {
                memoryCache.Set(accessToken, staff);
            }
            dbContext.CurrentDeposits.Add(new Domain.WagesEnities.CurrentDeposit()
            {
                Amount = fd.CumulativeAmount,
                CreateTime = DateTime.Now,
                Id = Guid.NewGuid(),
                MType = 6,
                Remarks = "",
                StaffId = fd.StaffId,
                JsonObj = Newtonsoft.Json.JsonConvert.SerializeObject(fd)
            });
            dbContext.SaveChanges();

        }
    }
}
