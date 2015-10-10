using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentureManagement.Web.Areas.Report.Models
{
    public class ProjectThreatCaseReport
    {
        //[Key]
        //public int ThreatCaseReportId { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "项目")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "一般事故隐患")]
        public int ThreatCaseLevelGeneral { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "较大事故隐患")]
        public int ThreatCaseLevelLarger { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "重大事故隐患")]
        public int ThreatCaseLevelMajor { get; set; }
    }

    public class DepartmentThreatCaseReport
    {
        //[Key]
        //public int ThreatCaseReportId { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "部门")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "一般事故隐患")]
        public int ThreatCaseLevelGeneral { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "较大事故隐患")]
        public int ThreatCaseLevelLarger { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "重大事故隐患")]
        public int ThreatCaseLevelMajor { get; set; }
    }
}