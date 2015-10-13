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
                Sms sms = null;
                var updatedThreatCase = _threatCaseService.Find(threatCase.ThreatCaseId);
                updatedThreatCase.ThreatCaseCorrection = threatCase.ThreatCaseCorrection;
                updatedThreatCase.ThreatCaseCorrectionValue = threatCase.ThreatCaseCorrectionValue;
                updatedThreatCase.ThreatCaseStatus = threatCase.ThreatCaseStatus;

                if (updatedThreatCase.ThreatCaseStatus.Equals(ThreatCase.STATUS_FINISH))
                    updatedThreatCase.ThreatCaseCorrectionTime = DateTime.Now;

                if (_threatCaseService.Update(updatedThreatCase))
                {
                    try
                    {
                        string message = null;
                        switch (updatedThreatCase.ThreatCaseStatus)
                        {
                            case ThreatCase.STATUS_WAITACKNOWLEDGE:
                                message = "施工单位" + updatedThreatCase.Project.Organization.OrganizationName +
                                          "在施工场所" + updatedThreatCase.Project.ProjectLocation +
                                          "存在" + updatedThreatCase.ThreatCaseCategory + "," + updatedThreatCase.ThreatCaseType +
                                          "类安全隐患,请于" + updatedThreatCase.ThreatCaseLimitTime.ToString("yyyy年MM月dd日") + "之前完成整改.";
                                sms = Common.SmsHelper.SendSms(updatedThreatCase.ThreatCaseOwnerId, message);
                                break;
                            case ThreatCase.STATUS_WAITCONFIRM:
                                message = "施工单位" + updatedThreatCase.Project.Organization.OrganizationName +
                                    "在施工场所" + updatedThreatCase.Project.ProjectLocation + "发现疑似隐患,请尽快登陆系统确认.";
                                sms = Common.SmsHelper.SendSms(updatedThreatCase.ThreatCaseConfirmerId, message);
                                break;

                            case ThreatCase.STATUS_VERTIFYERR:
                                message = "施工单位" + updatedThreatCase.Project.Organization.OrganizationName +
                                    "在施工场所" + updatedThreatCase.Project.ProjectLocation + "隐患整改未通过验收,请尽快登陆系统确认.";
                                sms = Common.SmsHelper.SendSms(updatedThreatCase.ThreatCaseOwnerId, message);
                                break;
                            case ThreatCase.STATUS_FINISH:
                                message = "施工单位" + updatedThreatCase.Project.Organization.OrganizationName +
                                    "在施工场所" + updatedThreatCase.Project.ProjectLocation + "隐患整改完毕,请尽快登陆系统确认.";
                                sms = Common.SmsHelper.SendSms(updatedThreatCase.ThreatCaseReviewerId, message);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.Message);
                    }
                    string infoMessage = "隐患修改成功。";
                    if (sms != null)
                    {
                        if (sms.Status != 0)
                        {
                            infoMessage += "短信预警失败,";
                            if (!string.IsNullOrEmpty(sms.BlockWord))
                                infoMessage += "存在运营商非法关键字'" + sms.BlockWord + ",";

                            infoMessage += "'请联系项目责任人:" + updatedThreatCase.ThreatCaseOwner.DisplayName + ",联系方式:" +
                                updatedThreatCase.ThreatCaseOwner.Mobile;
                        }
                    }

                    X.Msg.Confirm("提示", infoMessage, new MessageBoxButtonsConfig
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
                X.Msg.Alert("", "隐患修改失败,请检查参数.").Show();
                Debug.Print(ex.Message);
            }

            return this.FormPanel();
        }
    }
}
