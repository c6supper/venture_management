﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class ProjectService : BaseService<VMProject>, InterfaceProjectService
    {
        private readonly List<int> _currentOrgList;
        public ProjectService(List<int> currentOrgList)
            : base(RepositoryFactory.ProjectRepository)
        {
            _currentOrgList = currentOrgList;
            CurrentRepository.EntityFilterEvent += ProjectFilterEvent;
        }

        public ProjectService()
            : base(RepositoryFactory.ProjectRepository)
        {
        }

        private object ProjectFilterEvent(object sender, FileterEventArgs e)
        {
            var vmps = e.EventArg as IQueryable<VMProject>;
            Debug.Assert(vmps != null, "vmps != null");

            var filteredVmProject = new List<VMProject>();
            foreach (var orgId in _currentOrgList)
            {
                filteredVmProject.AddRange(vmps.Where(vmp => vmp.OrganizationId == orgId));
            }

            return filteredVmProject.AsQueryable();
        }

        public bool Exist(string project, int superProjectId)
        {
            return
                CurrentRepository.FindList(u => u.ProjectName == project, "ProjectName", false)
                .ToArray().SelectMany(prj => prj.ProjectRelation)
                    .Any(prjr => prjr.SuperProjectId == superProjectId);
        }

        public VMProject Find(string project, int superProjectId)
        {
            return CurrentRepository.FindList(u => u.ProjectName == project, string.Empty, false)
                .ToArray()
                .FirstOrDefault(prj => prj.ProjectRelation
                    .Any(prjr => prjr.SuperProjectId == superProjectId));
        }

        public VMProject Find(int projectId)
        {
            return CurrentRepository.Find(org => org.ProjectId == projectId);
        }

        public IQueryable<VMProject> FindList(Expression<Func<VMProject, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }

        public IQueryable<VMProject> FindPageList(int pageIndex, int pageSize, out int totalRecord,
            Expression<Func<VMProject, bool>> whereLamdba)
        {
            return CurrentRepository.FindPageList(pageIndex, pageSize, out totalRecord, whereLamdba, "projectName", false);
        }
    }
}