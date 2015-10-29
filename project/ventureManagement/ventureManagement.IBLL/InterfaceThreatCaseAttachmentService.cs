using System;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceThreatCaseAttachmentService : InterfaceBaseService<ThreatCaseAttachment>
    {
        IQueryable<ThreatCaseAttachment> FindList(Expression<Func<ThreatCaseAttachment, bool>> whereLamdba, string orderName, bool isAsc);
    }
}