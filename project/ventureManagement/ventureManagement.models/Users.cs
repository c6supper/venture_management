using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentureManagement.Models
{
    /// <summary>
    /// 用户模型
    /// <remarks>
    /// 创建：2014.02.02<br />
    /// 修改：2014.02.16
    /// </remarks>
    /// </summary>
    public class User
    {
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{2}到{1}个字符")]
        public string UserName { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "实名")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required] [Display(Name = "密码")] [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "密码最短{1}个字符")]
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// mobile
        /// </summary>
        [Required]
        [Display(Name = "手机号码")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        public DateTime RegistrationTime { get; set; }

        /// <summary>
        /// 上次登陆时间
        /// </summary>
        [Display(Name = "上次登陆时间")]
        public Nullable<DateTime> LoginTime { get; set; }

        /// <summary>
        /// 上次登陆IP
        /// </summary>
        // ReSharper disable once InconsistentNaming
        [Display(Name = "上次登陆的IP")]
        public string LoginIP { get; set; }


        [InverseProperty("ThreatCaseReporter")]
        public virtual ICollection<ThreatCase> ReportedThreatCases { get; set; }

        [InverseProperty("ThreatCaseOwner")]
        public virtual ICollection<ThreatCase> OwnedThreatCases { get; set; }

        [InverseProperty("ThreatCaseReviewer")]
        public virtual ICollection<ThreatCase> ReviewedThreatCases { get; set; }

        [InverseProperty("ThreatCaseConfirmer")]
        public virtual ICollection<ThreatCase> ConfirmedThreatCases { get; set; }

        /// <summary>
        /// 用户状态<br />
        /// </summary>
        private string _status = STATUS_INVALID;

        [Display(Name = "用户状态")]
        public string Status
        {
            get
            {
                switch (_status)
                {
                    case STATUS_INVALID:
                    case STATUS_VALID:
                    case STATUS_LOCKED:
                    case STATUS_UNAUTH:
                        return _status;
                    default:
                        _status = STATUS_INVALID;
                        return _status;
                }
            }
            set
            {
                switch (value)
                {
                    case STATUS_INVALID:
                    case STATUS_VALID:
                    case STATUS_LOCKED:
                    case STATUS_UNAUTH:
                        _status = value;
                        break;
                    default:
                        _status = STATUS_INVALID;
                        break;
                }
            }
        }

        public virtual ICollection<UserRoleRelation> UserRoleRelations { get; set; }
        public virtual ICollection<UserOrganizationRelation> UserOrganizationRelations { get; set; }

        // ReSharper disable InconsistentNaming
        public const string USER_ADMIN = "master";
        public const string STATUS_INVALID= "未验证";
        public const string STATUS_UNAUTH = "管理员未确认";
        public const string STATUS_LOCKED = "锁定";
        public const string STATUS_VALID = "已验证";
    }
}
