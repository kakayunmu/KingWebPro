using King.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace King.MVC.Jobs
{
    public class MonthlyTotalJob : Job
    {
        private ILogger<FixedJob> logger;
        private KingDBContext dbContext;
        private IMemoryCache memoryCache;
        public MonthlyTotalJob(ILogger<FixedJob> logger, KingDBContext dbContext, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.memoryCache = memoryCache;
        }
        [Invoke(Begin = "2017-08-09 00:00:00", Interval = 1000 * 60 * 20, SkipWhileExecuting = true)]
        public void Run()
        {
            logger.LogDebug("执行了合计活期收益任务 MonthlyTotalJob");
            try
            {
                if (DateTime.Now.Day == 1)//每月第一天
                {
                    var ret = dbContext.CurrentDeposits.FromSql("SELECT *  from currentdeposits where DATEDIFF(CreateTime,NOW())=0 AND MType=6 LIMIT 1");
                    if (ret == null)
                    {
                        string sqlStr = "INSERT INTO currentdeposits (Id, Amount, CreateTime, MType, Remarks , StaffId) SELECT UUID(), SUM(Amounts) AS Amounts, NOW(), 6, @remarks , StaffId FROM currentinterests WHERE CreateTime > @stime AND createTime < @etime GROUP BY StaffId";
                        dbContext.Database.ExecuteSqlCommand(sqlStr, new[] {
                    new MySqlParameter(){
                         ParameterName="@remarks",
                          Value=string.Format("{0}活期累计收益",DateTime.Now.ToString("yy年MM月份")),
                    },
                    new MySqlParameter(){
                        ParameterName="@stime",
                        Value=DateTime.Now.AddMonths(-1).Date,
                    },
                    new MySqlParameter()
                    {
                        ParameterName="@etime",
                        Value=DateTime.Now.Date
                    } });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("执行 MonthlyTotalJob 发生异常:{0}/n/r {1}", ex.Message, ex.StackTrace));
            }
        }
    }
}
