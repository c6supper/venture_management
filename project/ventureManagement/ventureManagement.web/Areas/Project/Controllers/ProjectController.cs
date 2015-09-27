using System.Collections.Generic;
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
            return View(GetProject());
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

        public ActionResult GetAllOrganizations(int start, int limit, int page, string query)
        {
            var orgController = new OrganizationController();
            var orgs = orgController.GetOrganizations(start, limit, page, query);
            return this.Store(orgs.Data, orgs.TotalRecords);
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
                    DisplayName = project.User.DisplayName
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

        private Node GetProject()
        {
            var node = new Node
            {
                Text = VMProject.PROJECT_ROOT,
                NodeID = VMProject.PROJECT_ROOT
            };

            foreach (var prj in _projectRelationService.FindList(r=>r.SuperProjectId == VMProject.INVALID_PROJECT,"ProjectRelationId",false)
                .ToArray().Select(r=>r.SubProject))
            {
                node.Children.Add(RecursiveAddNode(prj));
            }
            if (node.Children.Count <= 0)
                node.EmptyChildren = true;

            return node;
        }

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
                UserId = (int)userId
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
                    SuperProject = super,
                    SubProject = sub,
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

        public ActionResult DeleteProject(int projectId)
        {
            return this.Direct();
        }
    }
}
