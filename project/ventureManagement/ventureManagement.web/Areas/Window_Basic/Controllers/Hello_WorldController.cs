﻿using System.Web.Mvc;

namespace ventureManagement.web.Areas.Window_Basic.Controllers
{
	public class Hello_WorldController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public DirectResult Button2_Click()
		{
			X.GetCmp<Window>("Window1").Show();
			return this.Direct();
		}
	}
}