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
        [Display(Name = "项目")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "部门")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "地点")]
        public string ThreatCaseLocation { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "施工方责任人")]
        public string ThreatCaseOwner { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "一般事故隐患")]
        public int ThreatCaseLevelGeneral { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "较大事故隐患")]
        public int ThreatCaseLevelLarger { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "重大事故隐患")]
        public int ThreatCaseLevelMajor { get; set; }

        public const string THREATCASE_DEPARTMENTNAME = "施工单位";
        public const string THREATCASE_PROJECTNAME = "施工项目";
        public const string THREATCASE_LOCATION = "施工地点";
        public const string THREATCASE_OWNER = "施工责任人";

        public const string THREATCASELEVEL_GENERAL = "一般事故隐患";
        public const string THREATCASELEVEL_LARGER = "较大事故隐患";
        public const string THREATCASELEVEL_MAJOR = "重大事故隐患";
    }

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