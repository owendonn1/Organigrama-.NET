using System.Security.Claims;
using System;

namespace ERP_C.Helpers
{
    public static class Misc
    {
        public static int GetId(ClaimsPrincipal user)
        {
            return Int32.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
