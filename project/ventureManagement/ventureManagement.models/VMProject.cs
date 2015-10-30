using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

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

        [Required]
        [Display(Name = "施工项目启动时间")]
        public DateTime ProjectStartTime { get; set; }

        [Required]
        [Display(Name = "施工项目结束时间")]
        public DateTime ProjectFinishTime { get; set; }

        public string Description { get; set; }

        [Required, DefaultValue(STATUS_CONSTRUCTING)]
        [Display(Name = "项目状态")]
        public string ProjectStatus { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }

        [InverseProperty("SubProject")]
        public virtual ICollection<ProjectRelation> AsSubProjectRelation { get; set; }

        [InverseProperty("Project")]
        public virtual ICollection<ThreatCase> ThreatCases { get; set; }

        // ReSharper disable once InconsistentNaming
        public const string PROJECT_ROOT = "工程项目列表";

        // ReSharper disable once InconsistentNaming
        public const int INVALID_PROJECT = -1;

        public const string STATUS_CONSTRUCTING = "施工中";
        public const string STATUS_FINISHED = "施工完毕";
        public const string STATUS_PLANNING = "计划中";
    }
}
