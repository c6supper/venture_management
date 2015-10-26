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
        public string AttachmentUrl { get; set; }

        public virtual ThreatCase ThreatCase { get; set; }
    }
}
