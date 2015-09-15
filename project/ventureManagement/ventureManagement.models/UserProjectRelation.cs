using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentureManagement.Models
{
    public class UserProjectRelation
    {
        [Key]
        public int UserProjectRelationId { get; set; }

        [Required]
        public int UserRoleRelationId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public virtual UserRoleRelation UserRoleRelation { get; set; }
        public virtual Project Project { get; set; }
    }
}
