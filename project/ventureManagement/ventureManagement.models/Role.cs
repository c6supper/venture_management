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
        [Display(Name = "权限角色")]
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
            var permissions = new List<string>();
            var permissionStringsIndex = 0;
            int[] value = { (int)roleValue, (int)(roleValue >> 32) };

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
            int[] value = { (int)RoleValue, (int)(RoleValue >> 32) };

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
            RoleValue = (Convert.ToInt64(intByBitArray[1]) << 32) + intByBitArray[0];
        }

        // ReSharper disable InconsistentNaming
        //beware of the 31 bit and 63bit is always 0 because of the BitArray only takes int
        //31 63 should be PERIMISSION_UNKOWN
        public static readonly string[] PermissionStrings =
        {
            //PERIMISSION_ALARM_WRITE,
            PERIMISSION_ALARM,
            //PERIMISSION_FILEMANAGE_WRITE,
            PERIMISSION_FILEMANAGE,
            PERIMISSION_ORGANIZATION_WRITE,
            PERIMISSION_ORGANIZATION_READ,
            PERIMISSION_PERMISSION_WRITE,
            PERIMISSION_PERMISSION_READ,
            PERIMISSION_USER_WRITE,
            PERIMISSION_USER_READ,
            PERIMISSION_PROJECT_WRITE,
            PERIMISSION_PROJECT_READ,
            //PERIMISSION_CREATETHREATCASE_WRITE,
            PERIMISSION_CREATETHREATCASE,
            PERIMISSION_MYTHREATCASE,
            PERIMISSION_THREATCASE,
            //PERIMISSION_THREATCASE_READ,
            PERIMISSION_THREATCORRECTIONTEMPLATE,
            //PERIMISSION_THREATCORRECTIONTEMPLATE_READ,

            PERIMISSION_UNKOWN
        };
        
        //public const string PERIMISSION_ALARM_WRITE = "文档查询(写)";
        public const string PERIMISSION_ALARM = "文档查询";
        //public const string PERIMISSION_FILEMANAGE_WRITE = "文档上传(写)";
        public const string PERIMISSION_FILEMANAGE = "文档上传";
        public const string PERIMISSION_ORGANIZATION_WRITE = "安全部管理(写)";
        public const string PERIMISSION_ORGANIZATION_READ = "安全部管理(只读)";
        public const string PERIMISSION_PERMISSION_WRITE = "权限管理(写)";
        public const string PERIMISSION_PERMISSION_READ = "权限管理(只读)";
        public const string PERIMISSION_USER_WRITE = "用户管理(写)";
        public const string PERIMISSION_USER_READ = "用户管理(只读)"; 
        public const string PERIMISSION_PROJECT_WRITE = "施工项目管理(写)";
        public const string PERIMISSION_PROJECT_READ = "施工项目管理(只读)";
        public const string PERIMISSION_CREATETHREATCASE = "隐患排查预警申报";
        public const string PERIMISSION_MYTHREATCASE = "待处理的隐患排查预警";
        //public const string PERIMISSION_CREATETHREATCASE_READ = "隐患预警申报(只读)";
        public const string PERIMISSION_THREATCASE = "隐患排查预警库";
        //public const string PERIMISSION_THREATCASE_READ = "隐患排查预警库(只读)";
        public const string PERIMISSION_THREATCORRECTIONTEMPLATE = "隐患排查预警整改库模板";
        //public const string PERIMISSION_THREATCORRECTIONTEMPLATE_READ = "隐患排查预警整改库模板(只读)";

        public const string PERIMISSION_UNKOWN = "权限错误";

        public const string ROLE_ADMIN = "系统管理员";
        public const string ROLE_GROUP = "集团安全管理人员";
        public const string ROLE_BRANCH = "分局安全管理人员";
        public const string ROLE_PROJECT_INSPECTOR = "项目部安全巡检员";
        public const string ROLE_PROJECT_LEADER = "项目负责人";
    }
}
