using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ventureManagement.IDAL;
using ventureManagement.models;

namespace ventureManagement.DAL
{
    class RoleRepository : BaseRepository<Role>, InterfaceRoleRepository
    {
    }
}
