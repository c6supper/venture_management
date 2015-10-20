using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceProjectRelationService : InterfaceBaseService<ProjectRelation>
    {
        IQueryable<ProjectRelation> FindList(Expression<Func<ProjectRelation, bool>> whereLamdba, string orderName, bool isAsc);
    }
}