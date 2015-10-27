using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.Web.Areas.Report.Models;

namespace VentureManagement.Web.Areas.ThreatReport.Controllers
{
    public class ThreatCaseStatisticsController : ThreatCaseReportBaseController
    {
        static Dictionary<string, ThreatCaseStatistics> tcSubDi = new Dictionary<string, ThreatCaseStatistics>();
        //
        // GET: /Report/ThreatCaseStatistics/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Submit(string threatCategory, string threatType, DateTime? startTime, DateTime? endTime, string containerId)
        {
            var reportFrom = startTime == null ? DateTime.Now.AddYears(-1) : Convert.ToDateTime(startTime);
            var reportTo = endTime == null ? DateTime.Now : Convert.ToDateTime(endTime);

            this.GetCmp<GridPanel>("statisticsReportGridPanel").Dispose();
            this.BuildGridPanel(threatCategory, threatType, reportFrom, reportTo).AddTo(containerId);

            return this.Direct();
        }

        private Ext.Net.GridPanel BuildGridPanel(string treatCategory, string threatType, DateTime from, DateTime to)
        {
            return new Ext.Net.GridPanel
            {
                Border = false,
                ID = "statisticsReportGridPanel",
                Height = 1000,
                Store =  
                {
                    this.BuildStore(treatCategory, threatType, from, to)
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
                            Text = "隐患大类", 
                            DataIndex = "ThreatCaseCategory",
                            MinWidth = 150
                        },
                        new Column 
                        {
                            Text = "发生频次", 
                            DataIndex = "Frequency",
                            MinWidth = 150
                        },
                        new Column 
                        {
                            Text = "隐患预警等级", 
                            DataIndex = "ThreatCaseLevel",
                            MinWidth = 150
                        },
                        new Column
                        {
                            Text = "建议",
                            DataIndex = "Suggest",
                            MinWidth = 540
                        }
                    }
                },
                Plugins =
                {
                    (Html.X().RowExpander()
                    .Loader(Html.X().ComponentLoader()
                        .Url(Url.Action("GetSubGrid"))
                        .Mode(LoadMode.Component)
                        .LoadMask(mask => mask.ShowMask = true)
                        .Params(new { threatCaseCategory = JRawValue.From("this.record.data['ThreatCaseCategory']") })
                        )
                    )
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

        private Store BuildStore(string treatCategory, string threatType, DateTime from, DateTime to)
        {
            Store store = new Store
            {
                Model = 
                { 
                    new Model 
                    {
                        Fields = 
                        {
                            new ModelField("ThreatCaseCategory"),
                            new ModelField("ThreatCaseType"),
                            new ModelField("Frequency", ModelFieldType.Int),
                            new ModelField("ThreatCaseLevel"),
                            new ModelField("Suggest"),
                        }
                    }
                }
            };

            store.DataSource = FindThreatCaseReports(treatCategory, threatType, from, to);

            return store;
        }

        public IEnumerable FindThreatCaseReports(string treatCategory, string threatType, DateTime from, DateTime to)
        {
            var tcDi = new Dictionary<string, ThreatCaseStatistics>();
            //var tcSubDi = new Dictionary<string, ThreatCaseStatistics>();
            tcSubDi.Clear();
            foreach (var threatcase in _threatCaseService.FindList(tc => (tc.ThreatCaseFoundTime >= from && tc.ThreatCaseFoundTime <= to && tc.ThreatCaseLevel == ThreatCaseReport.THREATCASELEVEL_GENERAL),
                "ThreatCaseFoundTime", false).ToArray())
            {
                var tcc = threatcase.ThreatCaseCategory;
                if (tcc == null) continue;
                var tct = threatcase.ThreatCaseType;
                if (tct == null) continue;

                if (tcc == treatCategory || string.IsNullOrEmpty(treatCategory))
                {
                    if (!tcDi.ContainsKey(tcc))
                    {
                        var threatCaseStatistics = new ThreatCaseStatistics()
                        {
                            ThreatCaseCategory = tcc,
                            ThreatCaseType = tct,
                            ThreatCaseLevel = "",
                            Suggest = ""//threatcase.ThreatCaseSuggestion
                        };
                        tcDi[tcc] = threatCaseStatistics;
                    }

                    if (tct == threatType || string.IsNullOrEmpty(threatType))
                    {
                        if (!tcSubDi.ContainsKey(tct))
                        {
                            var threatCaseStatistics = new ThreatCaseStatistics()
                            {
                                ThreatCaseCategory = tcc,
                                ThreatCaseType = tct,
                                ThreatCaseLevel = "",
                                Suggest = ""//threatcase.ThreatCaseSuggestion
                            };
                            tcSubDi[tct] = threatCaseStatistics;
                        }

                        tcSubDi[tct].Frequency++;
                        tcDi[tcc].Frequency++;

                        if (tcSubDi[tct].Frequency > 50)
                        {
                            tcSubDi[tct].ThreatCaseLevel = "红色预警";
                            tcSubDi[tct].Suggest = "";
                        }
                        else if (tcSubDi[tct].Frequency > 20 && tcSubDi[tct].Frequency < 50)
                        {
                            tcSubDi[tct].ThreatCaseLevel = "橙色预警";
                            tcSubDi[tct].Suggest = "";
                        }
                        else if (tcSubDi[tct].Frequency > 10 && tcSubDi[tct].Frequency < 20)
                        {
                            tcSubDi[tct].ThreatCaseLevel = "黄色预警";
                            tcSubDi[tct].Suggest = "";
                        }
                        else if (tcSubDi[tct].Frequency < 10)
                        {
                            tcSubDi[tct].ThreatCaseLevel = "不预警";
                            tcSubDi[tct].Suggest = "";
                        }

                        if (tcDi[tcc].Frequency > 50)
                        {
                            tcDi[tcc].ThreatCaseLevel = "红色预警";
                            tcDi[tcc].Suggest = "";
                        }
                        else if (tcDi[tcc].Frequency > 20 && tcDi[tcc].Frequency < 50)
                        {
                            tcDi[tcc].ThreatCaseLevel = "橙色预警";
                            tcDi[tcc].Suggest = "";
                        }
                        else if (tcDi[tcc].Frequency > 10 && tcDi[tcc].Frequency < 20)
                        {
                            tcDi[tcc].ThreatCaseLevel = "黄色预警";
                            tcDi[tcc].Suggest = "";
                        }
                        else if (tcDi[tcc].Frequency < 10)
                        {
                            tcDi[tcc].ThreatCaseLevel = "不预警";
                            tcDi[tcc].Suggest = "";
                        }
                    }
                }
            }
            
            return tcDi.Values.ToList();
        }

        public ActionResult GetSubGrid(string threatCaseCategory)
        {
            var data = new List<ThreatCaseStatistics>();
            foreach (var threatCaseStatisticse in tcSubDi.Values.ToList())
            {
                if (threatCaseStatisticse.ThreatCaseCategory == threatCaseCategory)
                {
                    data.Add(threatCaseStatisticse);
                }
            }
            
            GridPanel grid = new GridPanel
            {
                Height = 150,
                //ID = "SubReportGridPanel",
                EnableColumnHide = false,
                SelectionModel = 
                { 
                    new RowSelectionModel() { Mode = SelectionMode.Single }
                },
                Store = 
                { 
                    new Store 
                    { 
                        Model = 
                        {
                            new Model {
                                IDProperty = "ID",
                                Fields = {
                                    new ModelField("ThreatCaseType"),
                                    new ModelField("Frequency"),
                                    new ModelField("ThreatCaseLevel"),
                                    new ModelField("Suggest")
                                }
                            }
                        },
                        DataSource = data
                    }
                },
                ColumnModel =
                {
                    Columns = 
                    { 
                        new Column { Text = "隐患小类", DataIndex = "ThreatCaseType", MinWidth = 145 },
                        new Column { Text = "发生频次", DataIndex = "Frequency", MinWidth = 150 },
                        new Column { Text = "隐患预警等级", DataIndex = "ThreatCaseLevel", MinWidth = 150 },
                        new Column { Text = "建议", DataIndex = "Suggest", MinWidth = 540 }
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

            return this.ComponentConfig(grid);
        }

        public ActionResult GetThreatCategory()
        {
            return this.Store(_correctionTemplate.Category);
        }

        public ActionResult GetThreatType(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                var threatCorrectionCategory = _correctionTemplate.Category.FirstOrDefault();
                if (threatCorrectionCategory != null)
                    return this.Store(threatCorrectionCategory.Type);
            }

            foreach (var cat in _correctionTemplate.Category.Where(cat => cat.CategoryName == category))
            {
                return this.Store(cat.Type);
            }

            return this.Store("隐患数据库破坏，请联系系统管理员。");
        }
    }
}
