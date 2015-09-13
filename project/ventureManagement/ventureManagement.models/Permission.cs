using System.ComponentModel.DataAnnotations;

namespace VentureManagement.Models
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        public bool MemberOrganizationRead { get; set; }
        public bool MemberOrganizationWrite { get; set; }
        public bool MemberUserWrite { get; set; }
        public bool MemberUserRead { get; set; }
    }
}