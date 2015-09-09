using System;
using System.Collections.Generic;
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
                    organizationName = org.OrganizationName,
                    Description = org.Description
                }
            };

            foreach (var childOrgr in _orgrService.FindList(org.OrganizationName))
            {
                node.Children.Add(RecursiveAddNode(childOrgr.SubordinateDepartment));
            }

            if (node.Children.Count > 0)
            {
                node.Icon = Icon.Add;
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
    }
}
