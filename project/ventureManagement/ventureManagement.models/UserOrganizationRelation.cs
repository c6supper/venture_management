using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentureManagement.Models
{
    public class UserOrganizationRelation
    {
        [Key]
        public int UserOrganizationRelationId { get; set; }

        [Required,ForeignKey("User")]
        public int UserId { get; set; }

        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        public virtual User User { get; set; }
        public virtual Organization Organization { get; set; } 
    }
}