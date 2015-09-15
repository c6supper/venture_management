using System.Web.Mvc;

namespace VentureManagement.Web.Areas.Threat
{
    public class ThreatAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Threat";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Threat_default",
                "Threat/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
