using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using King.EntityFrameworkCore;

namespace King.MVC.Areas.API.Controllers
{
    [Area("API")]
    public class BaseController : Controller
    {
        protected readonly ILogger<BaseController> logger;
        protected IMemoryCache memoryCache;
        protected KingDBContext dbContent;
        public BaseController(IMemoryCache memoryCache, KingDBContext dbContent,ILogger<BaseController> logger)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.dbContent = dbContent;
        }
        protected string accessToken;
        protected Domain.WagesEnities.Staff staff;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["Action"].ToString().ToLower();
            string[] noVActions = { "loginwithvcode", "login", "refaccesstoken", "sendloginvcode", "findpwd" };
            if (!noVActions.Contains(action))
            {
                Microsoft.Extensions.Primitives.StringValues act;
                context.HttpContext.Request.Headers.TryGetValue("accessToken", out act);
                if (act.Count > 0)
                {
                    accessToken = act[0];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        staff = memoryCache.Get<Domain.WagesEnities.Staff>(accessToken);
                        if (staff != null)
                        {
                            base.OnActionExecuting(context);
                        }
                        else
                        {
                            context.Result = Json(new { status = -200, msg = "accessToken 无效" });
                        }
                    }
                    else
                    {
                        context.Result = Json(new { status = -200, msg = "accessToken 无效" });
                    }
                }
                else
                {
                    context.Result = Json(new { status = -200, msg = "accessToken 无效" });
                }
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                context.Result = Json(new { status = -1, msg = context.Exception.Message });
                context.Exception = null;
            }
            else
            {
                base.OnActionExecuted(context);
            }
        }

    }
}
