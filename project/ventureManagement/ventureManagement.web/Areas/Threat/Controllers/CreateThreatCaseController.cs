using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.Models;
using VentureManagement.Web.Areas.Project.Controllers;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_CREATETHREATCASE_WRITE + "," + Role.PERIMISSION_CREATETHREATCASE_READ)]
    public class CreateThreatCaseController : ThreatBaseController
    {
        //
        // GET: /Threat/CreateThreatCase/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllProjects(int start, int limit, int page, string query)
        {
            var projectController = new ProjectController();
            var projects = projectController.GetProjects(start, limit, page, query);
            return this.Store(projects.Data, projects.TotalRecords);
        }

        public ActionResult GetThreatCategory()
        {
            return this.Store(_correctionTemplate.Category);
        }

        public ActionResult GetThreatType(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                var threatCorrectionCategory = _correctionTemplate.Category.FirstOrDefault();
                if (threatCorrectionCategory != null)
                    return this.Store(threatCorrectionCategory.Type);
            }

            foreach (var cat in _correctionTemplate.Category.Where(cat => cat.CategoryName == category))
            {
                return this.Store(cat.Type);
            }

            return this.Store("隐患数据库破坏，请联系系统管理员。");
        }

        [ValidateInput(true)]
		public ActionResult CreateThreatCase(List<FieldsGroupModel> groups1, List<FieldsGroupModel> groups2)
		{
			StringBuilder sb = new StringBuilder(255);

			sb.Append("<h1>Checked Items</h1>");
			sb.Append("<h2>CheckboxGroups</h2>");
			sb.Append("<blockquote>");

			groups1.ForEach(delegate(FieldsGroupModel group)
			{
				int count = 0;

				group.CheckedItems.ForEach(delegate(CheckedFieldModel checkbox)
				{
					if (count == 0)
					{
						sb.AppendFormat("<h3>{0}</h3>", group.FieldLabel);
						sb.Append("<blockquote>");
					}
					sb.AppendFormat("{0}<br />", checkbox.BoxLabel);
					count++;
				});

				if (count > 0)
				{
					sb.Append("</blockquote>");
				}
			});

			sb.Append("</blockquote>");

			sb.Append("<h2>RadioGroups</h2>");
			sb.Append("<blockquote>");

			groups2.ForEach(delegate(FieldsGroupModel group)
			{
			    int count = 0;

			    group.CheckedItems.ForEach(delegate(CheckedFieldModel radio)
			    {
			        if (count == 0)
			        {
			            sb.AppendFormat("<h3>{0}</h3>", group.FieldLabel);
			            sb.Append("<blockquote>");
			        }
			        sb.AppendFormat("{0}<br />", radio.BoxLabel);
			        count++;
			    });

			    if (count > 0)
			    {
			        sb.Append("</blockquote>");
			    }
			});

			sb.Append("</blockquote>");

			this.GetCmp<Label>("Label1").Html = sb.ToString();

			return this.Direct();
		}
    }
    public class CheckedFieldModel
    {
        public string BoxLabel { get; set; }
    }
    public class FieldsGroupModel
    {
        public FieldsGroupModel()
        {
            this.CheckedItems = new List<CheckedFieldModel>();
        }

        public string FieldLabel { get; set; }

        public List<CheckedFieldModel> CheckedItems { get; set; }
    }
}
