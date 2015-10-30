using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.DAL
{
    public class ThreatCaseRepository : BaseRepository<ThreatCase>, InterfaceThreatCaseRepository
    {
        public ThreatCaseRepository()
            :base()
        {
            RegisterProxyIncludePath("Project.Organization");
        }
    }
}