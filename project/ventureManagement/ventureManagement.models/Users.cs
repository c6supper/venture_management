using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{2}到{1}个字符")]
        public string UserName { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{2}到{2}个字符")]
        [Display(Name = "昵称")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必填")] [Display(Name = "密码")] [DataType(DataType.Password)] 
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// mobile
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "手机号码")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegistrationTime { get; set; }

        /// <summary>
        /// 上次登陆时间
        /// </summary>
        public Nullable<DateTime> LoginTime { get; set; }

        /// <summary>
        /// 上次登陆IP
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string LoginIP { get; set; }


        /// <summary>
        /// 用户状态<br />
        /// </summary>
        private string _status = STATUS_INVALID;

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

        // ReSharper disable once InconsistentNaming
        public const string USER_ADMIN = "master";
        public const string STATUS_INVALID= "未验证";
        public const string STATUS_UNAUTH = "管理员未确认";
        public const string STATUS_LOCKED = "锁定";
        public const string STATUS_VALID = "已验证";
    }
}
