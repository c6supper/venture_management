using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VentureManagement.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{1}到{0}个字")]
        [Display(Name = "部门名")]
        public string OrganizationName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明")]
        public string Description { get; set; }

        public virtual ICollection<OrganizationRoleRelation> OrganizationRoleRelations { get; set; }

        public virtual ICollection<OrganizationRelation> OrganizationRelations { get; set; }

        // ReSharper disable once InconsistentNaming
        public const string ORGANIZATION_STSTEM = "系统管理部门";
    }
}
