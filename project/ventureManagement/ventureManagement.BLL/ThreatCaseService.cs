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
        private readonly HashSet<int> _orgHash;
        public ThreatCaseService(HashSet<int> orgHash)
            : base(RepositoryFactory.ThreatCaseRepository)
        {
            _orgHash = orgHash;
            if (_orgHash != null)
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
            return tcs != null ? tcs.Where(t => _orgHash.Contains(t.Project.OrganizationId)) : null;
        }

        public bool Exist(int threatCaseId) { return CurrentRepository.Exist(t => t.ThreatCaseId == threatCaseId); }

        public ThreatCase Find(int threatCaseId) { return CurrentRepository.Find(t => t.ThreatCaseId == threatCaseId); }

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