using System.Net;
using System.Web.Mvc;

namespace VentureManagement.Web.Attributes
{
    public class AccessDeniedAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.Forbidden); 
            }
        }
    }
}