using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentureManagement.Models
{
    public class OrganizationRelation
    {
        [Key]
        public int OrganizationRelationId { get; set; }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        [Required()]
        [ForeignKey("OrganizationId")]
        public int SuperiorDepartmentId { get; set; }

        /// <summary>
        /// 下级部门ID
        /// </summary>
        [Required()]
        [ForeignKey("OrganizationId")]
        public int SubordinateDepartmentId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [StringLength(50, ErrorMessage = "少于{0}个字")]
        [Display(Name = "说明")]
        public string Description { get; set; }

        public virtual Organization SuperiorDepartment { get; set; }
        public virtual Organization SubordinateDepartment { get; set; }
    }
}
