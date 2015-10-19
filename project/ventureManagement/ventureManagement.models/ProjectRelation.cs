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
        [ForeignKey("SubProject"), Column(Order = 0)]
        public int SubProjectId { get; set; }

        public int SuperProjectId { get; set; }

        public string Description { get; set; }

        public virtual VMProject SubProject { get; set; }
    }
}
