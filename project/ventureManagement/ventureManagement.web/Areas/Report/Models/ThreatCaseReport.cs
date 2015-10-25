using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VentureManagement.Models;
// ReSharper disable InconsistentNaming

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
        [Display(Name = ThreatCase.THREATCASE_LEVEL_ORDINARY)]
        public int ThreatCaseLevelGeneral { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = ThreatCase.THREATCASE_LEVEL_MINOR)]
        public int ThreatCaseLevelLarger { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = ThreatCase.THREATCASE_LEVEL_MAJOR)]
        public int ThreatCaseLevelMajor { get; set; }

        public const string THREATCASE_DEPARTMENTNAME = "施工单位";
        public const string THREATCASE_PROJECTNAME = "施工项目";
        public const string THREATCASE_LOCATION = "施工地点";
        public const string THREATCASE_OWNER = "施工责任人";

        public const string THREATCASELEVEL_GENERAL = ThreatCase.THREATCASE_LEVEL_ORDINARY;
        public const string THREATCASELEVEL_LARGER = ThreatCase.THREATCASE_LEVEL_MINOR;
        public const string THREATCASELEVEL_MAJOR = ThreatCase.THREATCASE_LEVEL_MAJOR;
    }

    public class ProjectThreatCaseReport
    {
        //[Key]
        //public int ThreatCaseReportId { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "项目")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = ThreatCase.THREATCASE_LEVEL_ORDINARY)]
        public int ThreatCaseLevelGeneral { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = ThreatCase.THREATCASE_LEVEL_MINOR)]
        public int ThreatCaseLevelLarger { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = ThreatCase.THREATCASE_LEVEL_MAJOR)]
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
        [Display(Name = ThreatCase.THREATCASE_LEVEL_ORDINARY)]
        public int ThreatCaseLevelGeneral { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = ThreatCase.THREATCASE_LEVEL_MINOR)]
        public int ThreatCaseLevelLarger { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = ThreatCase.THREATCASE_LEVEL_MAJOR)]
        public int ThreatCaseLevelMajor { get; set; }
    }

    public class ThreatCaseStatistics
    {
        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患大类")]
        public string ThreatCaseCategory { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患小类")]
        public string ThreatCaseType { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "发生频次")]
        public int Frequency { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患预警等级")]
        public string ThreatCaseLevel { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "建议")]
        public string Suggest { get; set; }

//        public const string THREATCASELEVEL_MAJOR = ThreatCase.THREATCASE_LEVEL_MAJOR;
    }
}