using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VentureManagement.Models
{
    /// <summary>
    /// 角色
    /// <remarks>
    /// 创建：2014.02.02
    /// 修改：2014.02.16
    /// </remarks>
    /// </summary>
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{1}到{0}个字")]
        [Display(Name = "名称")]
        public string RoleName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(50, ErrorMessage = "少于{0}个字")]
        [Display(Name = "说明")]
        public string Description { get; set; }

        public virtual ICollection<UserRoleRelation> UserRoleRelations { get; set; }

        // ReSharper disable once InconsistentNaming
        public const string ROLE_ADMIN = "administrator";
    }
}
