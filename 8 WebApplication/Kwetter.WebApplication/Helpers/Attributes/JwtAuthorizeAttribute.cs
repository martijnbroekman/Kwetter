using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Kwetter.WebApplication.Helpers.Attributes
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public JwtAuthorizeAttribute()
        {
            AuthenticationSchemes = "Identity.Application," + JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
