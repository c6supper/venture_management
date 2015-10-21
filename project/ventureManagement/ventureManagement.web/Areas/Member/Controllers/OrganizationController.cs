using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.UI;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.Models;
using VentureManagement.Web.Attributes;


namespace VentureManagement.Web.Areas.Member.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_ORGANIZATION_WRITE + "," + Role.PERIMISSION_ORGANIZATION_READ)]
    public class OrganizationController : MemberBaseController
    {
        //
        // GET: /Member/Organization/
        public ActionResult Index()
        {
            return View(GetOrganization());
        }

        private Node RecursiveAddNode(Organization org)
        {
            var node = new Node
            {
                Text = org.OrganizationName,
                AttributesObject = new
                {
                    organizationId = org.OrganizationId,
                    organizationName = org.OrganizationName,
                    Description = org.Description
                }
            };
            node.CustomAttributes.Add(new ConfigItem("organizationId", "0", ParameterMode.Raw));
            node.CustomAttributes.Add(new ConfigItem("organizationName", "1", ParameterMode.Raw));

            foreach (var childOrgr in _orgrService.FindList(r => r.SuperiorDepartmentId == org.OrganizationId,
                "OrganizationId",false).ToArray())
            {
                node.Children.Add(RecursiveAddNode(childOrgr.SubordinateDepartment));
            }

            if (node.Children.Count > 0)
            {
                node.Icon = Icon.ApplicationAdd;
                node.Expanded = true;
            }
            else
            {
                node.Leaf = true;
            }

            return node;
        }

        public ActionResult GetAllOrganizations(int start, int limit, int page, string query)
        {
            var orgController = new OrganizationController();
            var orgs = orgController.GetOrganizations(start, limit, page, query);
            return this.Store(orgs.Data, orgs.TotalRecords);
        }

        public Paging<Organization> GetOrganizations(int start, int limit, int page,string filter)
        {
            var pageIndex = start / limit + ((start % limit > 0) ? 1 : 0) + 1;
            var count = 0;
            List<Organization> orgs;
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                orgs =
                    _orgSerivce.FindPageList(pageIndex, limit, out count,
                        org => org.OrganizationName.StartsWith(filter.ToLower())).ToList();
            }
            else
            {
                orgs =
                    _orgSerivce.FindPageList(pageIndex, limit, out count,
                        org => true).ToList();
            }

            return new Paging<Organization>(orgs, count);
        }

        private Node GetOrganization()
        {
            var orgs = Session["Organization"] as List<Organization>;
            var root = new Node();

            if (orgs == null) return root;
            foreach (var org in orgs)
            {
                root.Children.Add(RecursiveAddNode(org));
            }

            return root;
        }

        [AccessDeniedAuthorize(Roles = Role.PERIMISSION_ORGANIZATION_WRITE)]
        public ActionResult CreateOrganization(int superiorDepartmentId, string subordinateDepartment,string description)
        {
            string infoMessage = "创建成功";

            if (_orgSerivce.Exist(subordinateDepartment,superiorDepartmentId))
            {
                infoMessage = "部门已存在";
            }
            else
            {
                try
                {
                    var sub = _orgSerivce.Add(new Organization
                    {
                        OrganizationName = subordinateDepartment,
                        Description = description
                    });

                    var super = _orgSerivce.Find(superiorDepartmentId);
                    _orgrService.Add(new OrganizationRelation
                    {
                        SuperiorDepartmentId = super.OrganizationId,
                        SubordinateDepartmentId = sub.OrganizationId,
                        Description = description
                    });

                    UpdateCurrentOrgList();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message);
                }
            }
            X.Msg.Alert("提示",infoMessage).Show();

            return this.Direct();
        }

        [AccessDeniedAuthorize(Roles = Role.PERIMISSION_ORGANIZATION_WRITE)]
        public ActionResult DeleteOrganization(int? organizationId)
        {

            if (organizationId == null)
            {
                X.Msg.Alert("提示", "请先选择您要删除的安全部门.").Show();
                return this.Direct();
            }

            var org = _orgSerivce.Find((int)organizationId);

            if (org == null) return this.RedirectToAction("Index");

            if (org.AsSuperOrganizationRelations.Any())
            {
                X.Msg.Alert("提示", "该部门含有子部门，无法删除.").Show();
                return this.Direct();
            }

            if (org.Projects.Any())
            {
                X.Msg.Alert("提示", "该部门拥有施工项目，无法删除.").Show();
                return this.Direct();
            }

            if (!_orgSerivce.Delete(org))
            {
                X.Msg.Alert("提示", "删除部门失败，请稍候重试.").Show();
                return this.Direct();
            }

            X.Msg.Confirm("提示", "删除成功.", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "document.location.reload();",
                    Text = "确定"
                }
            }).Show();

            return this.Direct();
        }

        public ActionResult GetOrganization(int? organizationId)
        {
            return this.Json(organizationId == null ? null : _orgSerivce.Find((int)organizationId));
        }
    }
}
