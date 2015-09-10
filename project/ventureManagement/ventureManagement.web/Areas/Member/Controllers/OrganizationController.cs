using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.Models;


namespace VentureManagement.Web.Areas.Member.Controllers
{
    //[Authorize(Roles = "OrganizationControllerRead")]

    public class OrganizationController : Controller
    {
        readonly OrganizationService _orgSerivce = new OrganizationService();
        readonly OrganizationRelationService _orgrService = new OrganizationRelationService();
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

        private Node GetOrganization()
        {
            Organization org = Session["Organization"] as Organization;

            return org != null ? RecursiveAddNode(org) : new Node();
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

    }
}
