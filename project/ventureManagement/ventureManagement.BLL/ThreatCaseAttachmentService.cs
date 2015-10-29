using System;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class ThreatCaseAttachmentService : BaseService<ThreatCaseAttachment>, InterfaceThreatCaseAttachmentService
    {
        public ThreatCaseAttachmentService()
            : base(RepositoryFactory.ThreatCaseAttachmentRepository)
        {
        }

        public IQueryable<ThreatCaseAttachment> FindList(Expression<Func<ThreatCaseAttachment, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }
    }
}