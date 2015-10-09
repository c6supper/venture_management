using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_THREATCASE)]
    public class ThreatCaseController : ThreatBaseController
    {
        public ActionResult Index()
        {
            return View(_threatCaseService.FindList(t=>true,"ThreatCaseId",false).ToArray());
        }

        public ActionResult Detail(int threatCaseId)
        {
            return View(_threatCaseService.Find(threatCaseId));
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
            var threatCases = _threatCaseService.FindPageList(pageIndex, limit, out count, u=>true).ToList();

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

        [AccessDeniedAuthorize(Roles = Role.PERIMISSION_ORGANIZATION_WRITE)]
        public ActionResult UpdateThreatCases(StoreDataHandler handler)
        {
            //var threatCases = handler.BatchObjectData<ThreatCase>();

            //var store = this.GetCmp<Store>("ThreatCaseGridStore");

            //foreach (var createdThreatCase in threatCases.Created)
            //{
            //    if (!TryValidateModel(createdThreatCase))
            //    {
            //        var record = store.GetById(createdThreatCase.ThreatCaseId);
            //        X.Msg.Alert("", ThreatCase.VALIDATION_MESSAGE).Show();
            //        record.Reject();
            //        return this.Direct();
            //    }

            //    var user = _threatCaseService.Find(createdThreatCase.ThreatCaseName);

            //    if (user != null)
            //    {
            //        var record = store.GetById(createdThreatCase.ThreatCaseId);
            //        X.Msg.Alert("", "用户名冲突，请重试").Show();
            //        record.Destroy();
            //        return this.Direct();
            //    }

            //    user = new ThreatCase();
            //    user = createdThreatCase;
            //    user.RegistrationTime = DateTime.Now;
            //    user.Password = Common.Utility.DesEncrypt(user.ThreatCaseName);

            //    // ReSharper disable once InvertIf
            //    try
            //    {
            //        if ((user = _threatCaseService.Add(user)) != null)
            //        {
            //            var record = store.Find("ThreatCaseName", createdThreatCase.ThreatCaseName);
            //            record.Set("ThreatCaseId", user.ThreatCaseId);
            //            record.Commit();
            //        }
            //    }
            //    catch (System.Exception ex)
            //    {
            //        Debug.Print(ex.Message);
            //    }

            //}

            //foreach (var updatedThreatCase in threatCases.Updated)
            //{
            //    var user = _threatCaseService.Find(updatedThreatCase.ThreatCaseId);

            //    if (user.ThreatCaseName != updatedThreatCase.ThreatCaseName)
            //    {
            //        var record = store.GetById(updatedThreatCase.ThreatCaseId);
            //        record.Reject();
            //        X.Msg.Alert("", "用户名不能更改").Show();
            //        return this.Direct();
            //    }

            //    if (string.IsNullOrEmpty(updatedThreatCase.DisplayName) ||
            //        string.IsNullOrEmpty(updatedThreatCase.Email) ||
            //        string.IsNullOrEmpty(updatedThreatCase.Mobile))
            //    {
            //        var record = store.GetById(updatedThreatCase.ThreatCaseId);
            //        X.Msg.Alert("", "实名/邮箱/手机号不能为空，请重试").Show();
            //        record.Reject();
            //        return this.Direct();
            //    }

            //    user.DisplayName = updatedThreatCase.DisplayName;
            //    user.Email = updatedThreatCase.Email;
            //    user.Mobile = updatedThreatCase.Mobile;
            //    user.Status = updatedThreatCase.Status;

            //    if (_threatCaseService.Update(user))
            //    {
            //        var record = store.GetById(updatedThreatCase.ThreatCaseId);
            //        record.Commit();
            //    }
            //}

            //foreach (var deletedThreatCase in threatCases.Deleted)
            //{
            //    if (null == _threatCaseService.Find(deletedThreatCase.ThreatCaseName))
            //    {
            //        store.CommitRemoving(deletedThreatCase.ThreatCaseId);
            //        continue;
            //    }

            //    if (_userRoleRelationService.DeleteByThreatCase(deletedThreatCase.ThreatCaseName))
            //    {
            //        if (_threatCaseService.Delete(_threatCaseService.Find(deletedThreatCase.ThreatCaseName)))
            //        {
            //            store.CommitRemoving(deletedThreatCase.ThreatCaseId);
            //        }
            //    }
            //}

            return this.Direct();
        }

    }
}
