using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        [DefaultValue(0)]
        public long RoleValue { get; set; }

        public virtual ICollection<UserRoleRelation> UserRoleRelations { get; set; }

        public static string[] RoleValueToPermissions(long roleValue)
        {
            List<string> permissions = new List<string>();
            var permissionStringsIndex = 0;
            int[] value = { Convert.ToInt32(roleValue >> 8), Convert.ToInt32(roleValue) };

            var bitValue = new BitArray(value);

            foreach (bool bit in bitValue)
            {
                if(bit)
                    permissions.Add(PerimissionStrings[permissionStringsIndex]);

                permissionStringsIndex++;

                if (permissionStringsIndex >= PerimissionStrings.Count())
                    return permissions.ToArray();
            }

            return permissions.ToArray();
        }

        // ReSharper disable InconsistentNaming
        //beware of the 31 bit and 63bit is always 0 because of the BitArray only takes int
        public static readonly string[] PerimissionStrings =
        {
            PERIMISSION_ORGANIZATION ,
            PERIMISSION_USER,
            PERIMISSION_PERMISSION
        };
        public const string PERIMISSION_ORGANIZATION = "组织结构管理";
        public const string PERIMISSION_USER = "用户管理";
        public const string PERIMISSION_PERMISSION = "用户权限管理";
        public const string PERIMISSION_UNKOWN = "权限错误";

        public const string ROLE_ADMIN = "系统管理员";
    }
}
