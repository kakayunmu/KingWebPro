using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace King.Utility.Extended
{
    /// <summary>
    /// MVC User 扩展
    /// </summary>
    public static class UserExtended
    {
        /// <summary>
        /// 获取当前用户票据项信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claimTypes"></param>
        /// <returns></returns>
        public static string GetClaimVal(this IPrincipal user, string claimTypes)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
                return string.Empty;
            var claim = claimsIdentity.FindFirst(claimTypes);
            return claim == null ? null : claim.Value;

        }
    }
}
