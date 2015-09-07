using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ventureManagement.Models;

namespace VentureManagement.Models
{
    public class OrganizationRoleRelation
    {
        [Key]
        public int OrganizationRoleRelationId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required()]
        public int OrganizationId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Required()]
        public int RoleId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [StringLength(50, ErrorMessage = "少于{0}个字")]
        [Display(Name = "说明")]
        public string Description { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Role Role { get; set; }
    }
}
