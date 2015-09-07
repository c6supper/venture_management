using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentureManagement.Models
{
    /// <summary>
    /// 用户角色关系
    /// <remarks>
    /// 创建：2014.02.16
    /// </remarks>
    /// </summary>
    public class UserRoleRelation
    {
        [Key]
        public int UserRelationId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
