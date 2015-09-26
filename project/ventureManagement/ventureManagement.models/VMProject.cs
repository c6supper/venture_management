using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentureManagement.Models
{
    // ReSharper disable once InconsistentNaming
    public class VMProject
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        public string ProjectLocation { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public int  OrganizationId { get; set; }

        public string Description { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual ICollection<ProjectRelation> ProjectRelation { get; set; }

        // ReSharper disable once InconsistentNaming
        public const string PROJECT_ROOT = "工程项目列表";

        // ReSharper disable once InconsistentNaming
        public const int INVALID_PROJECT = -1;
    }
}
