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

            foreach (var childOrgr in _orgrService.FindList(org.OrganizationName).ToArray())
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

        [AllowAnonymous]
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

        public ActionResult CreateOrganization(int superiorDepartmentId, string subordinateDepartment,string description)
        {
            string infoMessage = "创建成功";

            if (_orgSerivce.Exist(subordinateDepartment,superiorDepartmentId))
            {
                infoMessage = "部门已存在";
            }
            else
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
                    SuperiorDepartment = super,
                    SubordinateDepartment = sub,
                    Description = description
                });

                return RedirectToAction("Index");
            }
            X.Msg.Alert("提示",infoMessage).Show();

            return this.Direct();
        }

        public ActionResult DeleteOrganization(int organizationId)
        {
            return this.Direct();
        }

    }
}
