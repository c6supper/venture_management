using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "施工地点")]
        public string ProjectLocation { get; set; }

        [Required]
        [Display(Name = "施工项目名")]
        public string ProjectName { get; set; }

        [Required]
        public int  OrganizationId { get; set; }

        [Required]
        public int UserId { get; set; }

        public string Description { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<ProjectRelation> ProjectRelation { get; set; }

        [InverseProperty("Project")]
        public virtual ICollection<ThreatCase> ThreatCases { get; set; }

        // ReSharper disable once InconsistentNaming
        public const string PROJECT_ROOT = "工程项目列表";

        // ReSharper disable once InconsistentNaming
        public const int INVALID_PROJECT = -1;
    }
}
