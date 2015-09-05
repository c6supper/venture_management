using System.Web.Mvc;

namespace ventureManagement.web.Areas.Member
{
	public class ButtonsBasicAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Member";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Member",
				"Member/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
