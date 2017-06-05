using King.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.Application.UserApp;
using Microsoft.Extensions.Logging;

namespace King.MVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserAppService _userAppService;
        private readonly ILogger _logger;

        public UserController(IUserAppService userAppService, ILogger<UserController> logger)
        {
            _userAppService = userAppService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUserByDepartent([FromBody]QueryParam qparam)
        {
            Guid department = Guid.Parse(qparam.SearchFelds.Find(f => f.Field == "departmentId").Val);
            var result = await _userAppService.GetUserByDepartment(department, qparam.StartPage, qparam.PageSize);
            return Json(result);
        }
    }
}
