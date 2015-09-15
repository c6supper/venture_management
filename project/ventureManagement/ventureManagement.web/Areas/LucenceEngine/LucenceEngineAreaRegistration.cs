using System.Web.Mvc;

namespace VentureManagement.Web.Areas.LucenceEngine
{
    public class LucenceEngineAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "LucenceEngine";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "LucenceEngine_default",
                "LucenceEngine/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
