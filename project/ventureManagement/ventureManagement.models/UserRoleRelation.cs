using System.ComponentModel.DataAnnotations;

namespace ventureManagement.Models
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
        public int RelationId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required()]
        public int UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Required()]
        public int RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
