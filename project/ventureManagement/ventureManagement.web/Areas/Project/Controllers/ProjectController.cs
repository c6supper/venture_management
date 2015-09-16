using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Areas.Project.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_PROJECT_WRITE + "," + Role.PERIMISSION_PROJECT_READ)]
    public class ProjectController : Controller
    {
        //
        // GET: /Project/Project/

        private readonly InterfaceProjectService _projectService = new ProjectService();
        private readonly InterfaceProjectRelationService _projectRelationService = new ProjectRelationService();

        public ActionResult Index()
        {
            Debug.Print(Url.Action("GetData"));
            return View(GetProject());
        }

        private Node RecursiveAddNode(VentureManagement.Models.Project project)
        {
            var node = new Node
            {
                Text = project.ProjectName,
                AttributesObject = new
                {
                    projectId = project.ProjectId,
                    projectName = project.ProjectName,
                    Description = project.Description
                }
            };
            node.CustomAttributes.Add(new ConfigItem("projectId", "0", ParameterMode.Raw));
            node.CustomAttributes.Add(new ConfigItem("projectName", "1", ParameterMode.Raw));

            foreach (var subProject in _projectRelationService.FindList(project.ProjectName).ToArray())
            {
                node.Children.Add(RecursiveAddNode(subProject.SubProject));
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

        private Node GetProject()
        {
            var node = new Node
            {
                Text = string.Empty
            };

            foreach (var project in _projectService.FindList(prj => prj.ProjectRelation.Count <= 0, string.Empty, false))
            {
                node.Children.Add(RecursiveAddNode(project));
            }

            return node;
        }

        public ActionResult CreateProject(int? superProjectId, string subProject, string description)
        {
            string infoMessage = "创建成功";

            if (_projectService.Exist(subProject, superProjectId))
            {
                infoMessage = "工程已存在";
            }
            else
            {
                var sub = _projectService.Add(new VentureManagement.Models.Project
                {
                    ProjectName = subProject,
                    Description = description
                });

                if (superProjectId != null)
                {
                    var super = _projectService.Find((int) superProjectId);
                    _projectRelationService.Add(new ProjectRelation
                    {
                        SuperProjectId = super.ProjectId,
                        SubProjectId = sub.ProjectId,
                        SuperProject = super,
                        SubProject = sub,
                        Description = description
                    });
                }

                return RedirectToAction("Index");
            }
            X.Msg.Alert("提示", infoMessage).Show();

            return this.Direct();
        }

        public ActionResult DeleteProject(int projectId)
        {
            return this.Direct();
        }
    }
}
