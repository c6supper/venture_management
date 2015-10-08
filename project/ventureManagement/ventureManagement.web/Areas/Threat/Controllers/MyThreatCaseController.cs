using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
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
            //var threatCases =
            //    _threatCaseService.FindList(
            //        t => (t.ThreatCaseOwnerId == _currentUser.UserId || t.ThreatCaseConfirmerId == _currentUser.UserId ||
            //             t.ThreatCaseReporterId == _currentUser.UserId || t.ThreatCaseReviewerId == _currentUser.UserId) && 
            //             t.ThreatCaseStatus != ThreatCase.STATUS_VERTIFYOK,
            //        "ThreatCaseId", false).ToArray();
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

            return View(threatCases);
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
