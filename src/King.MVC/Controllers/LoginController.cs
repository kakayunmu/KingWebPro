using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using King.MVC.Models;
using Microsoft.Extensions.Logging;
using King.Application.UserApp;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace King.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserAppService _userAppService;
        public LoginController(
            ILogger<LoginController> logger,
            IUserAppService userAppService)
        {
            _logger = logger;
            _userAppService = userAppService;
        }
        // GET: /<controller>/
        public IActionResult Index(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userAppService.CheckUser(model.UserName, model.Password);
                if (user != null)
                {
                    var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Email,user.EMail),
                        new Claim(ClaimTypes.MobilePhone,user.MobileNumber)
                    };
                    var Identity = new ClaimsIdentity("Forms");
                    Identity.AddClaims(claims);
                    var principal = new ClaimsPrincipal(Identity);
                    DateTime? expiresUtc = null;
                    if (model.Rememberme)
                    {
                        expiresUtc = DateTime.UtcNow.AddDays(7);
                    }
                    await HttpContext.Authentication.SignInAsync("UserAuth", principal, new AuthenticationProperties { IsPersistent = true, ExpiresUtc = expiresUtc });
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError("Password", "用户名或密码错误");
            }
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
