using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.Helpers.Extensions
{
    public static class HtmlHelpers
    {
        public static string MakeActive(this IUrlHelper urlHelper, string controller, string action)
        {
            string controllerName = urlHelper.ActionContext.RouteData.Values["controller"].ToString();
            string actionName = urlHelper.ActionContext.RouteData.Values["action"].ToString();

            if (!controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase) || !actionName.Equals(action, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return "active";
        }

        public static string MakeActive(this IUrlHelper urlHelper, bool active)
        {
            if (!active)
            {
                return null;
            }

            return "active";
        }
    }
}
