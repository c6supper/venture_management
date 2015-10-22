﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.Models;
using VentureManagement.Web.Areas.Member.Controllers;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Areas.Project.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_PROJECT_WRITE + "," + Role.PERIMISSION_PROJECT_READ)]
    public class ProjectController : ProjectBaseController
    {
        //
        // GET: /Project/Project/
        public ActionResult Index()
        {
            return View();
        }

        public StoreResult GetNodes(string node)
        {
            return this.Store(GetProject());
        }

        public ActionResult UpdateProject(string id, string field, string newValue, string oldValue)
        {
            X.Msg.Alert("提示", "请检查参数是否正确，项目名/备注/部门名/施工地点/负责人不能为空").Show();
            return this.Direct();

            return this.RemoteTree(newValue + "_echo");
            //return this.RemoteTree(false, "Renaming is disabled");
        }

        public ActionResult GetAllProjects(int start, int limit, int page, string query)
        {
            var projects = GetProjects(start, limit, page, query);
            return this.Store(projects.Data, projects.TotalRecords);
        }

        public Paging<VMProject> GetProjects(int start, int limit, int page, string filter)
        {
            var pageIndex = start / limit + ((start % limit > 0) ? 1 : 0) + 1;
            var count = 0;
            List<VMProject> projects;
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                projects =
                    _projectService.FindPageList(pageIndex, limit, out count,
                        prj => prj.ProjectName.StartsWith(filter.ToLower())).ToList();
            }
            else
            {
                projects =
                    _projectService.FindPageList(pageIndex, limit, out count,
                        prj => true).ToList();
            }

            return new Paging<VMProject>(projects, count);
        }

        private Node RecursiveAddNode(VMProject project)
        {
            var node = new Node
            {
                Text = project.ProjectName,
                AttributesObject = new
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    Description = project.Description,
                    ProjectLocation = project.ProjectLocation,
                    OrganizationName = project.Organization.OrganizationName,
                    UserId = project.User.UserId,
                    UserName = project.User.UserName,
                    DisplayName = project.User.DisplayName,
                    ProjectStatus = project.ProjectStatus
                }
            };

            foreach (var subProject in _projectRelationService.FindList(r => r.SuperProjectId == project.ProjectId,"ProjectRelationId",false).ToArray())
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

        private NodeCollection GetProject()
        {
            var nodes = new NodeCollection(false);
            //var node = new Node
            //{
            //    Text = VMProject.PROJECT_ROOT,
            //    NodeID = VMProject.PROJECT_ROOT
            //};

            foreach (var prj in _projectRelationService.FindList(r=>r.SuperProjectId == VMProject.INVALID_PROJECT,"ProjectRelationId",false)
                .ToArray().Select(r=>r.SubProject))
            {
                nodes.Add(RecursiveAddNode(prj));
            }
            //if (node.Children.Count <= 0)
            //    node.EmptyChildren = true;

            return nodes;
        }

        [AccessDeniedAuthorize(Roles = Role.PERIMISSION_PROJECT_WRITE)]
        public ActionResult CreateProject(int? superProjectId, string subProject, string description,
            string projectLocation,int? organizationid,int? userId)
        {
            var infoMessage = "创建成功";

            if(superProjectId == null)
                superProjectId = VMProject.INVALID_PROJECT;

            if (organizationid == null || userId == null)
            {
                X.Msg.Alert("提示", "请检查参数是否正确，项目名/备注/部门名/施工地点/负责人不能为空").Show();
                return this.Direct();
            }

            var project = new VMProject
            {
                ProjectName = subProject,
                Description = description,
                ProjectLocation = projectLocation,
                OrganizationId = (int)organizationid,
                UserId = (int)userId,
                ProjectStatus = VMProject.STATUS_CONSTRUCTING
            };

            ModelState.Clear();
            if (!TryValidateModel(project))
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();
                X.Msg.Alert("提示", "请检查参数是否正确，项目名/备注/部门名/项目地点不能为空").Show();
                return this.Direct();
            }

            if (_projectService.Exist(subProject, (int)superProjectId))
            {
                infoMessage = "工程已存在";
            }
            else
            {
                var sub = _projectService.Add(project);

                var super = _projectService.Find((int)superProjectId);
                _projectRelationService.Add(new ProjectRelation
                {
                    SuperProjectId = (int)superProjectId,
                    SubProjectId = sub.ProjectId,
                    Description = description
                });
            }

            X.Msg.Confirm("提示", infoMessage, new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "document.location.reload();",
                    Text = "确定"
                }
            }).Show();

            return this.Direct();
        }

        [AccessDeniedAuthorize(Roles = Role.PERIMISSION_PROJECT_WRITE)]
        public ActionResult DeleteProject(int? projectId)
        {
            if (projectId == null)
            {
                X.Msg.Alert("提示", "请先选择您要删除的施工项目.").Show();
                return this.Direct();
            }

            var project = _projectService.Find((int)projectId);

            if (project == null) return this.RedirectToAction("Index");

            if (project.ThreatCases.Any())
            {
                X.Msg.Alert("提示", "系统含有该施工项目的隐患申报记录，无法删除该工程.").Show();
                return this.Direct();
            }

            if (_projectRelationService.FindList(pjr => pjr.SuperProjectId == (int)projectId,"ProjectRelationId",false).Any())
            {
                X.Msg.Alert("提示", "该工程有拆分子工程，请先删除拆分子工程.").Show();
                return this.Direct();
            }

            if (!_projectService.Delete((int) projectId))
            {
                X.Msg.Alert("提示", "删除工程失败，请稍候重试.").Show();
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
    }
}
