using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentureManagement.Models
{
    public class Alarm
    {
        [Key]
        public int AlarmId { get; set; }

        [Display(Name = "文件名")]
        //[Required(ErrorMessage = "必填")]
        //[StringLength(20, MinimumLength = 4, ErrorMessage = "{2}到{1}个字符")]
        public string FileName { get; set; }

        //[Required(ErrorMessage = "必填")]
        //[StringLength(20, MinimumLength = 2, ErrorMessage = "{2}到{2}个字符")]
        [Display(Name = "文件内容")]
        public string Content { get; set; }


        //[Required(ErrorMessage = "必填")]
        //[StringLength(20, MinimumLength = 2, ErrorMessage = "{2}到{2}个字符")]
        [Display(Name = "文件路径")]
        public string FilePath { get; set; }

        //[Required(ErrorMessage = "必填")]
        //[StringLength(20, MinimumLength = 2, ErrorMessage = "{2}到{2}个字符")]
        [Display(Name = "文件时间")]
        public DateTime FileDate { get; set; }
    }
}
