using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentureManagement.Models
{
    public class ThreatCaseAttachment
    {
        [Key]
        public int ThreatCaseAttachmentId { get; set; }

        [Required]
        public int ThreatCaseId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "不超过{1}个字符")]
        public string AttachmentUrl { get; set; }

        [StringLength(30, ErrorMessage = "不超过{1}个字符")]
        public string AttachmentDisplayName { get; set; }

        public virtual ThreatCase ThreatCase { get; set; }
    }
}
