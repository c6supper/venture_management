using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Areas.Project.Controllers;
using VentureManagement.Web.Attributes;
using PartialViewResult = Ext.Net.MVC.PartialViewResult;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_CREATETHREATCASE_WRITE + "," + Role.PERIMISSION_CREATETHREATCASE_READ)]
    public class CreateThreatCaseController : ThreatBaseController
    {
        //
        // GET: /Threat/CreateThreatCase/
        public ActionResult Index()
        {
            return View(new ThreatCase());
        }

        public ActionResult SelectProject(ThreatCase threatCase)
        {
            InterfaceProjectService projectService = new ProjectService();
            var project = projectService.Find(threatCase.ProjectId);
            threatCase.ThreatCaseOwnerId = project.UserId;
            threatCase.ThreatCaseOwner = new User
            {
                UserId = project.UserId,
                Mobile = project.User.Mobile,
                Email = project.User.Email,
                DisplayName = project.User.DisplayName,
            };
            threatCase.Project = new VMProject()
            {
                ProjectLocation = project.ProjectLocation,
                OrganizationId = project.OrganizationId,
                Organization = new Organization()
                {
                    OrganizationName = project.Organization.OrganizationName
                }
            };

            return this.Direct(threatCase);
        }

        public ActionResult GetAllProjects(int start, int limit, int page, string query)
        {
            var projectController = new ProjectController();
            var projects = projectController.GetProjects(start, limit, page, query);
            return this.Store(projects.Data, projects.TotalRecords);
        }

        public ActionResult Submit(ThreatCase threatCase)
        {
            ModelState.Clear();
            if (!TryValidateModel(threatCase))
            {
                X.Msg.Alert("", "请检查输入参数，请重试").Show();
                return this.FormPanel();
            }

            try
            {
                threatCase.ThreatCaseReportTime = DateTime.Now;
                threatCase.ThreatCaseReporterId = _currentUser.UserId;
                threatCase.ThreatCaseStatus = ThreatCase.STATUS_WAITCONFIRM;
                threatCase.Project = null;
                threatCase.ThreatCaseOwner = null;
                threatCase.ThreatCaseReporter = null;
                threatCase.ThreatCaseCorrectionTime = DateTime.MaxValue;

                if (null != _threatCaseService.Add(threatCase))
                {
                    X.Msg.Confirm("提示", "隐患申报成功,等待施工方确认中.", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "document.location.href='/Threat/ThreatCasePrinter/Index?threatCaseId=" + threatCase.ThreatCaseId + "';",
                            Text = "确定"
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                X.Msg.Alert("", "隐患申报失败,请检查参数.").Show();
                Debug.Print(ex.Message);
            }

            return this.FormPanel();
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

        //[ValidateInput(true)]
        //public ActionResult CreateThreatCase(List<FieldsGroupModel> groups1, List<FieldsGroupModel> groups2)
        //{
        //    StringBuilder sb = new StringBuilder(255);

        //    sb.Append("<h1>Checked Items</h1>");
        //    sb.Append("<h2>CheckboxGroups</h2>");
        //    sb.Append("<blockquote>");

        //    groups1.ForEach(delegate(FieldsGroupModel group)
        //    {
        //        int count = 0;

        //        group.CheckedItems.ForEach(delegate(CheckedFieldModel checkbox)
        //        {
        //            if (count == 0)
        //            {
        //                sb.AppendFormat("<h3>{0}</h3>", group.FieldLabel);
        //                sb.Append("<blockquote>");
        //            }
        //            sb.AppendFormat("{0}<br />", checkbox.BoxLabel);
        //            count++;
        //        });

        //        if (count > 0)
        //        {
        //            sb.Append("</blockquote>");
        //        }
        //    });

        //    sb.Append("</blockquote>");

        //    sb.Append("<h2>RadioGroups</h2>");
        //    sb.Append("<blockquote>");

        //    groups2.ForEach(delegate(FieldsGroupModel group)
        //    {
        //        int count = 0;

        //        group.CheckedItems.ForEach(delegate(CheckedFieldModel radio)
        //        {
        //            if (count == 0)
        //            {
        //                sb.AppendFormat("<h3>{0}</h3>", group.FieldLabel);
        //                sb.Append("<blockquote>");
        //            }
        //            sb.AppendFormat("{0}<br />", radio.BoxLabel);
        //            count++;
        //        });

        //        if (count > 0)
        //        {
        //            sb.Append("</blockquote>");
        //        }
        //    });

        //    sb.Append("</blockquote>");

        //    this.GetCmp<Label>("Label1"). = sb.ToString();

        //    return this.Direct();
        //}
    }
}
