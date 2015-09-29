using System;
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
    public class ThreatCaseService : BaseService<ThreatCase>, InterfaceThreatCaseService
    {
        private readonly List<int> _currentOrgList;
        public ThreatCaseService(List<int> currentOrgList)
            : base(RepositoryFactory.ThreatCaseRepository)
        {
            _currentOrgList = currentOrgList;
            if (currentOrgList != null)
            {
                CurrentRepository.EntityFilterEvent += ThreatCaseFilterEvent;                
            }
        }

        public ThreatCaseService()
            : base(RepositoryFactory.ThreatCaseRepository)
        {
        }

        private object ThreatCaseFilterEvent(object sender, FileterEventArgs e)
        {
            var tcs = e.EventArg as IQueryable<ThreatCase>;
            Debug.Assert(tcs != null, "tcs != null");

            var filteredThreatCase = new List<ThreatCase>();
            foreach (var orgId in _currentOrgList)
            {
                filteredThreatCase.AddRange(tcs.Where(tc => tc.Project.OrganizationId == orgId));
            }

            return filteredThreatCase.AsQueryable();
        }

        public bool Exist(int threatCaseId) { return CurrentRepository.Exist(t => t.ThreatCaseId == threatCaseId); }

        public ThreatCase Find(int threatCaseId) { return CurrentRepository.Find(t => t.ThreatCaseId == threatCaseId); }

        public int Count()
        {
            return CurrentRepository.Count(t => true);
        }

        public IQueryable<ThreatCase> FindPageList(int pageIndex, int pageSize, out int totalRecord,
            Expression<Func<ThreatCase, bool>> whereLamdba)
        {
            return CurrentRepository.FindPageList(pageIndex, pageSize, out totalRecord, whereLamdba, "ThreatCaseFoundTime", false);
        }

        public IQueryable<ThreatCase> FindList(Expression<Func<ThreatCase, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }
    }
}