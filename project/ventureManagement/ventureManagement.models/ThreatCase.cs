using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable InconsistentNaming

namespace VentureManagement.Models
{
    //工程名、施工方、责任人、隐患点、发现隐患的时间、隐患整改完成的时间、隐患大类、隐患小类、隐患描述、
    //严重性等级、可能性等级、风险指数、隐患级别、资格资质证件使用情况、现有安全设施情况、整改措施、整改投入、
    //整改效果评估（通过或不通过）、复查人

    public class ThreatCase
    {
        [Key]
        public int ThreatCaseId { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "施工项目ID")]
        public int ProjectId { get; set;}

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患点")]
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
        [Display(Name = "可选择采取的措施")]
        public string ThreatCaseSuggestion { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患原因分析")]
        public string ThreatCaseCause { get; set; }

        [Display(Name = "整改措施")]
        public string ThreatCaseCorrection { get; set; }

        [Display(Name = "整改投入")]
        public int ThreatCaseCorrectionValue { get; set; }

        [Display(Name = "整改效果评估")]
        public string ThreatCaseCorrectionResult { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "隐患上报人")]
        [ForeignKey("ThreatCaseReporter")]
        public int ThreatCaseReporterId { get; set; }
        [Required(ErrorMessage = "必填")]
        [Display(Name = "施工方责任人")]
        [ForeignKey("ThreatCaseOwner")]
        public int ThreatCaseOwnerId { get; set; }
        [Required(ErrorMessage = "必填")]
        [Display(Name = "确认人")]
        [ForeignKey("ThreatCaseConfirmer")]
        public int ThreatCaseConfirmerId { get; set; }
        [Required(ErrorMessage = "必填")]
        [Display(Name = "复查人")]
        [ForeignKey("ThreatCaseReviewer"), Column(Order = 3)]
        public int ThreatCaseReviewerId { get; set; }

        [Display(Name = "状态")]
        public string ThreatCaseStatus { get; set; }

        [InverseProperty("ThreatCase")]
        public virtual ICollection<ThreatCaseAttachment> ThreatCaseAttachments { get; set; }

        [Required]
        [Display(Name = "隐患分级")]
        public string ThreatCaseLevel { get; set; }

        public virtual VMProject Project { get; set; }

        public virtual User ThreatCaseReporter { get; set; }
        public virtual User ThreatCaseOwner { get; set; }
        public virtual User ThreatCaseReviewer { get; set; }
        public virtual User ThreatCaseConfirmer { get; set; }

        // ReSharper disable once InconsistentNaming
        public const string VALIDATION_MESSAGE = "\"工程名/隐患点/发现隐患时间\"不能为空";

        public const string STATUS_WAITCONFIRM = "等待审核";
        public const string STATUS_WAITACKNOWLEDGE = "等待施工方确认";
        public const string STATUS_CORRECTING = "整改中";
        public const string STATUS_FINISH = "整改完毕";
        public const string STATUS_VERTIFYOK = "整改通过";
        public const string STATUS_VERTIFYERR = "整改未通过";
        public const string STATUS_INVALID = "无效隐患";

        public static string[] GetAllStatusByCurrentStatus(string status)
        {
            switch (status)
            {
                case STATUS_WAITCONFIRM:
                    return new string[] { status,STATUS_INVALID, STATUS_WAITACKNOWLEDGE };
                case STATUS_WAITACKNOWLEDGE:
                    return new string[] { status,STATUS_CORRECTING };
                case STATUS_CORRECTING:
                    return new string[] { status,STATUS_FINISH };
                case STATUS_FINISH:
                    return new string[] { status,STATUS_VERTIFYOK, STATUS_VERTIFYERR };
                case STATUS_VERTIFYERR:
                    return new string[] { status,STATUS_CORRECTING };

                default:
                    return new string[] { status };
            }
        }

        public static string ConvertDisplay2Value(string display)
        {
            switch (display)
            {
                case STATUS_WAITCONFIRM:
                    return "STATUS_WAITCONFIRM";
                case STATUS_WAITACKNOWLEDGE:
                    return "STATUS_WAITACKNOWLEDGE";
                case STATUS_CORRECTING:
                    return "STATUS_CORRECTING";
                case STATUS_FINISH:
                    return "STATUS_FINISH";
                case STATUS_VERTIFYOK:
                    return "STATUS_VERTIFYOK";
                case STATUS_VERTIFYERR:
                    return "STATUS_VERTIFYERR";
                case STATUS_INVALID:
                    return "STATUS_INVALID";
            }
            return String.Empty;
        }

        public static string[] GetAllThreatCaseStatus()
        {
            return new string[] { STATUS_WAITCONFIRM, STATUS_WAITACKNOWLEDGE, STATUS_CORRECTING, STATUS_FINISH, STATUS_VERTIFYOK, STATUS_VERTIFYERR };
        }

        public const string THREATCASE_LEVEL_MAJOR = "重大隐患";
        public const string THREATCASE_LEVEL_MINOR = "一般隐患B类";
        public const string THREATCASE_LEVEL_ORDINARY = "一般隐患A类";
    }
}