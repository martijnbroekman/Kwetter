using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.Helpers.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetId(this ClaimsPrincipal principal)
        {
            if (int.TryParse(principal.FindFirst("sub")?.Value, out int userId))
            {
                return userId;
            }
            throw new ArgumentNullException("sub");
        }
    }
}
