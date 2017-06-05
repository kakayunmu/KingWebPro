using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    [Authorize]
    public class HomeController : KingControllerBase
    {
        public IActionResult Index()
        {
           // User.Identity.
            //var query = from u in User.Claims
            //            where u.Type == System.Security.Claims.ClaimTypes.NameIdentifier
            //            select u.Value;
            //string a = query.FirstOrDefault();
            return View();
        }
    }
}
