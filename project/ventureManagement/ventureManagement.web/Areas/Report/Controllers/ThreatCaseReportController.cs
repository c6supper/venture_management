using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.Web.Areas.Report.Models;

namespace VentureManagement.Web.Areas.Report.Controllers
{
    public class ThreatCaseReportController : ThreatCaseReportBaseController
    {
        //
        // GET: /Report/ThreatCaseReport/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Generate(string type, DateTime? from, DateTime? to, string containerId)
        {
            var reportFrom = @from == null ? new DateTime(1990, 1, 1) : Convert.ToDateTime(@from);
            var reportTo = to == null ? new DateTime(3000, 1, 1) : Convert.ToDateTime(to);

            this.GetCmp<GridPanel>("ReportGridPanel").Dispose();
            this.BuildGridPanel(type, reportFrom, reportTo).AddTo(containerId);

            return this.Direct();
        }

        private Ext.Net.GridPanel BuildGridPanel(string type, DateTime from, DateTime to)
        {
            bool bDepartmentName = true, bProjectName = true, bThreatCaseLocation = true, bThreatCaseOwner = true;
            switch (type)
            {
                case ThreatCaseReport.THREATCASE_DEPARTMENTNAME:
                    bDepartmentName = false;
                    break;
                case ThreatCaseReport.THREATCASE_PROJECTNAME:
                    bProjectName = false;
                    break;
                case ThreatCaseReport.THREATCASE_LOCATION:
                    bThreatCaseLocation = false;
                    break;
                case ThreatCaseReport.THREATCASE_OWNER:
                    bThreatCaseOwner = false;
                    break;
                default:
                    break;
            }

            return new Ext.Net.GridPanel
            {
                Border = false,
                ID = "ReportGridPanel",
                Store =  
                {
                    this.BuildStore(type, from, to)
                },
                SelectionModel = 
                { 
                    new RowSelectionModel() { Mode = SelectionMode.Single }
                },
                ColumnModel =
                {
                    Columns =
                    {
                        new Column 
                        {
                            Text = ThreatCaseReport.THREATCASE_DEPARTMENTNAME, 
                            DataIndex = "DepartmentName",
                            MinWidth = 150,
                            Hidden = bDepartmentName
                        },
                        new Column 
                        {
                            Text = ThreatCaseReport.THREATCASE_PROJECTNAME, 
                            DataIndex = "ProjectName",
                            MinWidth = 150,
                            Hidden = bProjectName
                        },
                        new Column 
                        {
                            Text = ThreatCaseReport.THREATCASE_LOCATION, 
                            DataIndex = "ThreatCaseLocation",
                            MinWidth = 150,
                            Hidden = bThreatCaseLocation
                        },
                        new Column 
                        {
                            Text = ThreatCaseReport.THREATCASE_OWNER, 
                            DataIndex = "ThreatCaseOwner",
                            MinWidth = 150,
                            Hidden = bThreatCaseOwner
                        },
                        new Column
                        {
                            Text = ThreatCaseReport.THREATCASELEVEL_GENERAL,
                            DataIndex = "ThreatCaseLevelGeneral",
                            MinWidth = 150
                        },
                        new Column
                        {
                            Text = ThreatCaseReport.THREATCASELEVEL_LARGER,
                            DataIndex = "ThreatCaseLevelLarger",
                            MinWidth = 150
                        },
                        new Column
                        {
                            Text = ThreatCaseReport.THREATCASELEVEL_MAJOR,
                            DataIndex = "ThreatCaseLevelMajor",
                            MinWidth = 150,
                        }
                    }
                },
                View =
                {
                   new Ext.Net.GridView()
                   {
                        StripeRows = true,
                        TrackOver = true 
                   }
                }
            };
        }

        private Store BuildStore(string type, DateTime from, DateTime to)
        {
            Store store = new Store
            {
                Model = 
                { 
                    new Model 
                    {
                        Fields = 
                        {
                            new ModelField("DepartmentName"),
                            new ModelField("ProjectName"),
                            new ModelField("ThreatCaseLocation"),
                            new ModelField("ThreatCaseOwner"),
                            new ModelField("ThreatCaseLevelGeneral", ModelFieldType.Int),
                            new ModelField("ThreatCaseLevelLarger", ModelFieldType.Int),
                            new ModelField("ThreatCaseLevelMajor", ModelFieldType.Int)
                        }
                    }
                }
            };

            store.DataSource = FindThreatCaseReports(type, from, to);

            return store;
        }
        
        public IEnumerable FindThreatCaseReports(string type, DateTime from, DateTime to)
        {
            List<string> typeValueList = new List<string>();
            var tcDi = new Dictionary<string, ThreatCaseReport>();
            foreach (var threatcase in _threatCaseService.FindList(tc => (tc.ThreatCaseFoundTime >= from && tc.ThreatCaseFoundTime <= to),
                "ThreatCaseLevel", false).ToArray())
            {
                typeValueList.Clear();
                switch (type)
                {
                    case ThreatCaseReport.THREATCASE_DEPARTMENTNAME:
                        typeValueList.Add(threatcase.Project.Organization.OrganizationName);
                        //typeValueList.AddRange(_orgrService.GetParentOrgList(threatcase.Project.Organization.OrganizationName));
                        break;
                    case ThreatCaseReport.THREATCASE_PROJECTNAME:
                        typeValueList.Add(threatcase.Project.ProjectName);
                        //typeValueList.AddRange(_projectRelationService.GetParentProjectList(threatcase.Project.ProjectId));
                        break;
                    case ThreatCaseReport.THREATCASE_LOCATION:
                        typeValueList.Add(threatcase.ThreatCaseLocation);
                        break;
                    case ThreatCaseReport.THREATCASE_OWNER:
                        typeValueList.Add(threatcase.ThreatCaseOwner.DisplayName);
                        break;
                    default:
                        break;
                }

                foreach (var typeValue in typeValueList)
                {
                    if (!tcDi.ContainsKey(typeValue))
                    {
                        var threatCaseReport = new ThreatCaseReport()
                        {
                            DepartmentName = typeValue,//threatcase.Project.Organization.OrganizationName,
                            ProjectName = typeValue,//threatcase.Project.ProjectName,
                            ThreatCaseLocation = typeValue,//threatcase.ThreatCaseLocation,
                            ThreatCaseOwner = typeValue,//threatcase.ThreatCaseOwner.UserName,
                        };
                        tcDi[typeValue] = threatCaseReport;
                    }

                    switch (threatcase.ThreatCaseLevel)
                    {
                        case ThreatCaseReport.THREATCASELEVEL_GENERAL:
                            tcDi[typeValue].ThreatCaseLevelGeneral++;
                            break;
                        case ThreatCaseReport.THREATCASELEVEL_LARGER:
                            tcDi[typeValue].ThreatCaseLevelLarger++;
                            break;
                        case ThreatCaseReport.THREATCASELEVEL_MAJOR:
                            tcDi[typeValue].ThreatCaseLevelMajor++;
                            break;
                    }
                }
            }

            return tcDi.Values.ToList();
        }
    }
}
