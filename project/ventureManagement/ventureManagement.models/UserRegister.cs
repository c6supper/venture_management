using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentureManagement.Models
{
    public class UserRegister : User
    {
        /// 确认密码
        /// </summary>
        [Display(Name = "确认密码", Description = "再次输入密码。")]
        [Compare("Password", ErrorMessage = "必填")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}
