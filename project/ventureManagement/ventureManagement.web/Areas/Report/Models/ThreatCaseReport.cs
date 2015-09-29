using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentureManagement.Web.Areas.Report.Models
{
    public class ThreatCaseReport
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

       // [Required(ErrorMessage = "必填")]
       // [Display(Name = "隐患上报时间")]
       // public DateTime ThreatCaseReportTime { get; set; }

    }
}