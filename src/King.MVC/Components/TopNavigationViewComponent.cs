using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.Application.UserApp;
using King.Utility.Extended;
using System.Security.Claims;

namespace King.MVC.Components
{
    [ViewComponent(Name = "TopNavigation")]
    public class TopNavigationViewComponent : ViewComponent
    {
        private readonly IUserAppService _userAppService;
        public TopNavigationViewComponent(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userAppService.Get(Guid.Parse(User.GetClaimVal(ClaimTypes.NameIdentifier)));
            user.HeadImg = string.IsNullOrEmpty(user.HeadImg) ? "img/user9-560x560.jpg" : user.HeadImg;
            return View(user);
        }

    }
}
