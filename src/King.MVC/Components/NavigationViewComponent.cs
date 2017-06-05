using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.Application.MenuApp;
using King.Application.UserApp;

namespace King.MVC.Components
{
    [ViewComponent(Name = "Navigation")]
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IMenuAppService _menuAppService;
        private readonly IUserAppService _userAppService;

        public NavigationViewComponent(IMenuAppService menuAppService, IUserAppService userAppService)
        {
            _menuAppService = menuAppService;
            _userAppService = userAppService;
        }

        public IViewComponentResult Invoke()
        {
            var menus = _menuAppService.GetAllList();
            return View(_menuAppService.ConvertL2T(menus.Result));
        }

    }
}
