using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentureManagement.Models
{
    //工程名、施工方、责任人、隐患地点、发现隐患的时间、隐患整改完成的时间、隐患大类、隐患小类、隐患描述、
    //严重性等级、可能性等级、风险指数、隐患级别、资格资质证件使用情况、现有安全设施情况、整改措施、整改投入、
    //整改效果评估（通过或不通过）、复查人

    public class ThreatCase
    {
        [Key]
        public int ThreatCaseId { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "工程名")]
        public int ProjectId { get; set;}

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患地点")]
        public string ThreatCaseLocation { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患上报时间")]
        public DateTime ThreatCaseReportTime { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "发现隐患时间")]
        public DateTime ThreatCaseFoundTime { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "整改限期")]
        public DateTime ThreatCaseLimitTime { get; set; }

        [Display(Name = "隐患整改完成时间")]
        public DateTime ThreatCaseCorrectionTime{ get; set; }

        [Display(Name = "隐患大类")]
        [Required(ErrorMessage = "必填")]
        public string ThreatCaseCategory { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患小类")]
        public string ThreatCaseType { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患描述")]
        public string ThreatCaseDescription { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患严重等级")]
        public int ThreatCaseSeverity { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患可能性等级")]
        public int ThreatCasePassibility { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患风险指数")]
        public int ThreatCaseRisk { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "资格资质证件使用情况")]
        public string ThreatCaseCertification { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "现有安全设施情况")]
        public string ThreatCaseCurrentSecurity { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "建议整改措施")]
        public string ThreatCaseSuggestion { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "可能的原因")]
        public string ThreatCaseCause { get; set; }

        [Display(Name = "整改措施")]
        public string ThreatCaseCorrection { get; set; }

        [Display(Name = "整改投入")]
        public int ThreatCaseCorrectionValue { get; set; }

        [Display(Name = "整改效果评估")]
        public string ThreatCaseCorrectionResult { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患上报人")]
        [ForeignKey("ThreatCaseReporter"), Column(Order = 0)]
        public int ThreatCaseReporterId { get; set; }
        [Required(ErrorMessage = "必填")]
        [Display(Name = "施工方责任人")]
        [ForeignKey("ThreatCaseOwner"), Column(Order = 1)]
        public int ThreatCaseOwnerId { get; set; }
        [Display(Name = "确认人")]
        [ForeignKey("ThreatCaseConfirmer"), Column(Order = 2)]
        public int ThreatCaseConfirmerId { get; set; }
        [Display(Name = "复查人")]
        [ForeignKey("ThreatCaseRiviewer"), Column(Order = 3)]
        public int ThreatCaseRiviewerId { get; set; }

        [Display(Name = "状态")]
        public string ThreatCaseStatus { get; set; }

        [Required]
        [Display(Name = "隐患分级")]
        public string ThreatCaseLevel { get; set; }

        public virtual VMProject Project { get; set; }

        public virtual User ThreatCaseReporter { get; set; }
        public virtual User ThreatCaseOwner { get; set; }
        public virtual User ThreatCaseRiviewer { get; set; }
        public virtual User ThreatCaseConfirmer { get; set; }

        // ReSharper disable once InconsistentNaming
        public const string VALIDATION_MESSAGE = "\"工程名/隐患地点/发现隐患时间\"不能为空";

        public const string STATUS_WAITCONFIRM = "等待施工方确认";
        public const string STATUS_CORRECTING = "整改中";
        public const string STATUS_WAITVERTIFY = "等待整改确认";
        public const string STATUS_VERTIFYOK = "整改完成";
        public const string STATUS_VERTIFYERR = "整改不通过";
    }
}