using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ventureManagement.Models;
using VentureManagement.IDAL;

namespace VentureManagement.DAL
{
    internal class UserRoleRelationRepository : BaseRepository<UserRoleRelation>, InterfaceUserRoleRelatioRepository
    {
    }
}
