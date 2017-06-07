using King.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.Application.UserApp;
using Microsoft.Extensions.Logging;
using King.Application.RoleApp;
using King.Application.UserApp.Dtos;
using King.Utility.Extended;
using System.Security.Claims;

namespace King.MVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;
        private readonly ILogger _logger;

        public UserController(IUserAppService userAppService, IRoleAppService roleAppService, ILogger<UserController> logger)
        {
            _userAppService = userAppService;
            _roleAppService = roleAppService;
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


        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleAppService.GetAllList();
            return Json(roles.Select(it => new { it.Id, it.Name }));
        }

        public async Task<IActionResult> Edit(UserDto dto, string[] roles)
        {
            try
            {
                if (dto.Id == Guid.Empty)
                {
                    dto.Id = Guid.NewGuid();
                }
                var userRoles = new List<UserRoleDto>();
                foreach (var role in roles)
                {
                    userRoles.Add(new UserRoleDto()
                    {
                        UserId = dto.Id,
                        RoleId = Guid.Parse(role)
                    });
                }
                dto.UserRoles = userRoles;
                dto.CreateTime = DateTime.Now;
                dto.CreateUserId = Guid.Parse(User.GetClaimVal(ClaimTypes.NameIdentifier));
                var user = await _userAppService.InsertOrUpdate(dto);
                return Json(new { Result = "Success", message = "保存数据成功" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", message = ex.Message });
            }
        }

        public async Task<IActionResult> Get(Guid id)
        {
            return Json(await _userAppService.Get(id));
        }

    }
}
