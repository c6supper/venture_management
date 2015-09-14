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
                    permissions.Add(PermissionStrings[permissionStringsIndex]);

                permissionStringsIndex++;

                if (permissionStringsIndex >= PermissionStrings.Count())
                    return permissions.ToArray();
            }

            return permissions.ToArray();
        }

        public IEnumerable RoleValueToAllPermissions()
        {
            var permissions = new List<object> {RoleId,RoleName,Description};

            var permissionStringsIndex = 0;
            int[] value = { Convert.ToInt32(RoleValue), Convert.ToInt32(RoleValue >> 8) };

            var bitValue = new BitArray(value);

            foreach (bool bit in bitValue)
            {
                if (permissionStringsIndex == 31 || permissionStringsIndex == 63)
                    continue;

                permissions.Add(bit);

                if (++permissionStringsIndex >= PermissionStrings.Count())
                    break;
            }

            return permissions.ToArray();
        }

        public void PermissionsToRoleValue(BitArray bitValue)
        {
            var intByBitArray = new int[2];
            bitValue.CopyTo(intByBitArray,0);
            RoleValue = (Convert.ToInt64(intByBitArray[1]) << 8) + intByBitArray[0];
        }

        // ReSharper disable InconsistentNaming
        //beware of the 31 bit and 63bit is always 0 because of the BitArray only takes int
        //31 63 should be PERIMISSION_UNKOWN
        public static readonly string[] PermissionStrings =
        {
            PERIMISSION_ORGANIZATION_WRITE ,
            PERIMISSION_ORGANIZATION_READ,
            PERIMISSION_USER_WRITE,
            PERIMISSION_USER_READ,
            PERIMISSION_PERMISSION_WRITE,
            PERIMISSION_PERMISSION_READ,

            PERIMISSION_UNKOWN
        };

        public const string PERIMISSION_ORGANIZATION_WRITE = "组织结构管理(写)";
        public const string PERIMISSION_ORGANIZATION_READ = "组织结构管理(读)";
        public const string PERIMISSION_USER_WRITE = "用户管理(写)";
        public const string PERIMISSION_USER_READ = "用户管理(读)";
        public const string PERIMISSION_PERMISSION_WRITE = "用户权限管理(写)";
        public const string PERIMISSION_PERMISSION_READ = "用户权限管理(读)";
        public const string PERIMISSION_UNKOWN = "权限错误";

        public const string ROLE_ADMIN = "系统管理员";
    }
}
