using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentureManagement.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        public string ProjectLocation { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public int  OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
