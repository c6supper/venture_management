using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ventureManagement.IBLL;
using ventureManagement.Models;
using VentureManagement.DAL;
using VentureManagement.IBLL;

namespace VentureManagement.BLL
{
    public class UserRoleRelationService : BaseService<UserRoleRelation>, InterfaceRoleRelationService
    {
        public UserRoleRelationService()
            : base(RepositoryFactory.UserRoleRelationRepository)
        {
        }
    }
}
