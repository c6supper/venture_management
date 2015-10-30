using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentureManagement.Models
{
    public class ProjectRelation
    {
        [Key]
        public int ProjectRelationId { get; set; }

        [Required]
        public int SubProjectId { get; set; }

        public int SuperProjectId { get; set; }

        [StringLength(30, ErrorMessage = "不超过{1}个字符")]
        public string Description { get; set; }

        [ForeignKey("SubProjectId")]
        public virtual VMProject SubProject { get; set; }
    }
}
