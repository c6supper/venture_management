using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.Models;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    public class MyThreatCaseController : ThreatBaseController
    {
        //
        // GET: /Threat/MyThreatCase/

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read(StoreRequestParameters parameters)
        {
            return this.Store(ThreatCasesPaging(parameters));
        }

        private Paging<ThreatCase> ThreatCasesPaging(StoreRequestParameters parameters)
        {
            return ThreatCasePaging(parameters.Start, parameters.Limit, parameters.SimpleSort, parameters.SimpleSortDirection, null);
        }

        private Paging<ThreatCase> ThreatCasePaging(int start, int limit, string sort, SortDirection dir, string filter)
        {
            var pageIndex = start / limit + ((start % limit > 0) ? 1 : 0) + 1;
            var count = 0;
            var threatCases = new List<ThreatCase>();

            var myConfirmedThreatCases =
                _currentUser.ConfirmedThreatCases.Where(t => t.ThreatCaseStatus == ThreatCase.STATUS_WAITCONFIRM);

            var myOwnedThreatCases =
                _currentUser.OwnedThreatCases.Where(t => t.ThreatCaseStatus == ThreatCase.STATUS_WAITACKNOWLEDGE ||
                                                         t.ThreatCaseStatus == ThreatCase.STATUS_VERTIFYERR ||
                                                         t.ThreatCaseStatus == ThreatCase.STATUS_CORRECTING);
            var myReviewedThreatCases =
                _currentUser.ReviewedThreatCases.Where(t => t.ThreatCaseStatus == ThreatCase.STATUS_FINISH);

            if (myOwnedThreatCases.Any())
                threatCases.AddRange(myOwnedThreatCases);

            if (myConfirmedThreatCases.Any())
                threatCases.AddRange(myConfirmedThreatCases);

            if (myReviewedThreatCases.Any())
                threatCases.AddRange(myReviewedThreatCases);

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                threatCases.RemoveAll(t => !t.ThreatCaseStatus.ToLower().StartsWith(filter.ToLower()));
            }

            var projectService = new ProjectService();
            foreach (var t in threatCases)
            {
                var project = projectService.Find(t.ProjectId);
                t.Project = new VMProject()
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectLocation = project.ProjectLocation,
                    OrganizationId = project.OrganizationId,
                    Description = project.Description,
                    UserId = project.UserId
                };
            }

            if (!string.IsNullOrEmpty(sort))
            {
                threatCases.Sort(delegate(ThreatCase x, ThreatCase y)
                {
                    var direction = dir == SortDirection.DESC ? -1 : 1;

                    var userA = x.GetType().GetProperty(sort).GetValue(x, null);
                    var userB = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(userA, userB) * direction;
                });
            }

            return new Paging<ThreatCase>(threatCases, count);
        }


        public ActionResult Detail(int threatCaseId)
        {
            return View(_threatCaseService.Find(threatCaseId));
        }

        public ActionResult Submit(ThreatCase threatCase)
        {
            threatCase.Project = null;
            threatCase.ThreatCaseConfirmer = null;
            threatCase.ThreatCaseReviewer = null;
            threatCase.ThreatCaseOwner = null;
            threatCase.ThreatCaseReporter = null;

            ModelState.Clear();
            if (!TryValidateModel(threatCase))
            {
                X.Msg.Alert("", "请检查输入参数，请重试").Show();
                return this.FormPanel();
            }

            try
            {
                var updatedThreatCase = _threatCaseService.Find(threatCase.ThreatCaseId);
                updatedThreatCase.ThreatCaseCorrection = threatCase.ThreatCaseCorrection;
                updatedThreatCase.ThreatCaseCorrectionValue = threatCase.ThreatCaseCorrectionValue;
                updatedThreatCase.ThreatCaseStatus = threatCase.ThreatCaseStatus;

                if (updatedThreatCase.ThreatCaseStatus.Equals(ThreatCase.STATUS_FINISH))
                    updatedThreatCase.ThreatCaseCorrectionTime = DateTime.Now;

                if (_threatCaseService.Update(updatedThreatCase))
                {
                    X.Msg.Confirm("提示", "隐患状态成功.", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "window.parent.document.location.reload();",
                            Text = "确定"
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                X.Msg.Alert("", "隐患状态修改失败,请检查参数.").Show();
                Debug.Print(ex.Message);
            }

            return this.FormPanel();
        }
    }
}
