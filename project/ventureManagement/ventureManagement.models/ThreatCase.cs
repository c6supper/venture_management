using System;
using System.ComponentModel.DataAnnotations;

namespace VentureManagement.Models
{
    //工程名、施工方、责任人、隐患地点、发现隐患的时间、隐患整改完成的时间、隐患大类、隐患小类、隐患描述、
    //严重性等级、可能性等级、风险指数、隐患级别、资格资质证件使用情况、现有安全设施情况、整改措施、整改投入、
    //整改效果评估（通过或不通过）、复查人

    public class ThreatCase
    {
        [Key]
        public int ThreatCaseId { get; set; }

        [Required]
        public int ProjectId { get; set;}

        [Required]
        public string ThreatCaseLocation { get; set; }

        public DateTime ThreatCaseFoundTime { get; set; }

        public DateTime ThreatCaseCorrectionTime{ get; set; }

        public string ThreatCaseCategory { get; set; }

        public string ThreatCaseType { get; set; }

        public string ThreatCaseDescription { get; set; }

        public int ThreatCaseSeverity { get; set; }

        public int ThreatCasePassibility { get; set; }

        public int ThreatCaseRisk { get; set; }

        //上报人
        public int ThreatCaseReporterId { get; set; }
        //资质证明情况
        public string ThreatCaseCertification { get; set; }
        //责任人
        public int ThreatCaseOwnerId { get; set; }

        //整改方案建议
        public string ThreatCaseSuggestion { get; set; }

        //实际整改方案
        public string ThreatCaseCorrection { get; set; }
        //整改投入
        public int ThreatCaseCorrectionValue { get; set; }
        //整改效果评估
        public string ThreatCaseCorrectionResult { get; set; }
        //确认人
        public int ThreadCaseConfirmerId { get; set; }
        //复查人
        public int ThreatCaseRiviewerId { get; set; }

        public virtual VMProject Project { get; set; }
        public virtual User ThreatCaseReporter { get; set; }
        public virtual User ThreatCaseOwner { get; set; }
        public virtual User ThreatCaseRiviewer { get; set; }
        public virtual User ThreadCaseConfirmer { get; set; }
    }
}