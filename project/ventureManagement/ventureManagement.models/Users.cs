using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ventureManagement.Models
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
        [Display(Name = "显示名")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
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
        /// 用户状态<br />
        /// 0正常，1锁定，2未通过邮件验证，3未通过管理员确认
        /// </summary>
        public int Status { get; set; }

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
        public string LoginIP { get; set; }

        /// <summary>
        /// 用户状态文字说明
        /// </summary>
        /// <returns></returns>
        public string StatusToString()
        {
            switch (Status)
            {
                case 0:
                    return "正常";
                case 1:
                    return "已锁定";
                case 2:
                    return "未通过邮件验证";
                case 3:
                    return "未通过管理员确认";
                default:
                    return "未知";
            }
        }


        public virtual ICollection<UserRoleRelation> UserRoleRelations { get; set; }
    }
}
