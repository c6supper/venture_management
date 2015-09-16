using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public int SuperProjectId { get; set; }

        public string Description { get; set; }

        public virtual Project SubProject { get; set; }
        public virtual Project SuperProject { get; set; }
    }
}
