using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ventureManagement.models
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
        public string Name { get; set; }

        /// <summary>
        /// 角色类型<br />
        /// 0普通（普通注册用户），1特权（像VIP之类的类型），3管理（管理权限的类型）
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "用户组类型")]
        public int Type { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(50, ErrorMessage = "少于{0}个字")]
        [Display(Name = "说明")]
        public string Description { get; set; }

        /// <summary>
        /// 获取角色类型名称
        /// </summary>
        /// <returns></returns>
        public string TypeToString()
        {
            switch (Type)
            {
                case 0:
                    return "普通";
                case 1:
                    return "特权";
                case 2:
                    return "管理";
                default:
                    return "未知";
            }
        }

        public virtual ICollection<UserRoleRelation> UserRoleRelations { get; set; }
    }
}
