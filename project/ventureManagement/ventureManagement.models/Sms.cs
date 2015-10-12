using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentureManagement.Models
{
    public class Sms
    {
        [Key]
        public int SmsId { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "号码")]
        [DataType(DataType.PhoneNumber)]
        public string Address { get; set; }

        [Required]
        public int Send2UserId { get; set; }

        [Required]
        [DefaultValue(0)]
        public int  Type { get; set; }

        [Required]
        public string TaskId { get; set; }

        [Required]
        public DateTime SendDateTime { get; set; }

        public DateTime RecvDateTime { get; set; }

        [Required]
        public int Status { get; set; }

        [Required(ErrorMessage = "必填"),MaxLength(300)]
        [Display(Name = "消息内容")]
        public string Message { get; set; }

        public string DeliverStats { get; set; }

        [MaxLength(300)]
        public string BlockWord { get; set; }

        [ForeignKey("Send2UserId")]
        public virtual User Send2User { get; set; }
    }
}
