using King.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace King.MVC.Jobs
{
    public class CurrentJob : Job
    {
        private ILogger<FixedJob> logger;
        private KingDBContext dbContext;
        private IMemoryCache memoryCache;
        public CurrentJob(ILogger<FixedJob> logger, KingDBContext dbContext, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.memoryCache = memoryCache;
        }
        [Invoke(Begin = "2017-08-09 00:10:00", Interval = 1000 * 60 * 60 * 24, SkipWhileExecuting = true)]
        public void Run()
        {
            logger.LogDebug("执行了CurrentJob");
            var setting = dbContext.Settings.FirstOrDefault();
            if (setting == null)
            {
                logger.LogWarning("执行CurrentJob 时发现配置是空的，结束执行");
                return;
            }
            logger.LogDebug(setting.GeneralInterestRate + "");
            string sql = "INSERT INTO currentinterests SELECT UUID(), a.CurrentAmount * " + setting.GeneralInterestRate + " / 365/100, NOW(), a.Id, 0 FROM staffs a LEFT JOIN (SELECT * FROM currentinterests WHERE DATEDIFF(CreateTime, NOW()) = 0 ) b ON a.Id = b.StaffId WHERE a.IsDel = 0 AND CurrentAmount > 0 AND b.Id IS NULL";
            var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            try
            {
                dbContext.Database.ExecuteSqlCommand(sql);
                dbContext.Database.ExecuteSqlCommand("UPDATE staffs a INNER JOIN (SELECT * FROM currentdeposits WHERE DATEDIFF(CreateTime, NOW()) = 0 AND Settled = 0 ) b ON a.Id = b.StaffId SET a.CurrentAmount = a.CurrentAmount + b.Amounts WHERE a.IsDel = 0");
                dbContext.Database.ExecuteSqlCommand("UPDATE currentinterests SET Settled=1 where Setled=0");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError("执行CurrentJob 发生异常", ex);
                transaction.Rollback();
            }
        }
    }
}
