using System;
using System.Collections.Generic;
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
            CurrentRepository.EntityFilterEvent += ThreatCaseFilterEvent;
        }

        public ThreatCaseService()
            : base(RepositoryFactory.ThreatCaseRepository)
        {
        }

        private object ThreatCaseFilterEvent(object sender, FileterEventArgs e)
        {
            var tcs = e.EventArg as IQueryable<ThreatCase>;

            return _currentOrgList.Aggregate(tcs, (current, orgId) =>
                current.Where(tc => tc.Project.OrganizationId == orgId).Concat(current).Distinct());
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
    }
}