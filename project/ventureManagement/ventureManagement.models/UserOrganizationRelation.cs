using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentureManagement.Models
{
    public class UserOrganizationRelation
    {
        [Key]
        public int UserOrganizationRelationId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; } 
    }
}