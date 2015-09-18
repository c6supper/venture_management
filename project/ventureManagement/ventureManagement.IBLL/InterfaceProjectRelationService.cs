using System.Linq;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceProjectRelationService : InterfaceBaseService<ProjectRelation>
    {
        IQueryable<ProjectRelation> FindList(string projectName);
        bool Exist(string superiorProject, string subordinateProject);
    }
}