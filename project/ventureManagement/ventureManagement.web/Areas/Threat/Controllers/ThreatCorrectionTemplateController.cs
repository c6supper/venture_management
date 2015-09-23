using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.Models;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    public class ThreatCorrectionTemplateController : Controller
    {
        //
        // GET: /Threat/ThreatCorrectionTemplate/
        // ReSharper disable once InconsistentNaming
        private const string _template = "~/Areas/Threat/Content/CorrectionTemplate.xml";
        private ThreatCorrection _correctionTemplate = null;

        public ThreatCorrectionTemplateController()
        {
            _correctionTemplate = ThreatCorrection.Deserialize(System.Web.HttpContext.Current.Server.MapPath(_template));
        }

        public ActionResult Index()
        {
            return View();
        }

        // ReSharper disable once InconsistentNaming
        private readonly string _filesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

        public ActionResult UploadClick()
        {
            const string tpl = "Uploaded file: {0}<br/>Size: {1} bytes";

            if (this.GetCmp<FileUploadField>("FileUploadField").HasFile)
            {
                this.GetCmp<FileUploadField>("FileUploadField").PostedFile.SaveAs(Path.Combine(_filesDir, this.GetCmp<FileUploadField>("FileUploadField").PostedFile.FileName));
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO,
                    Title = "Success",
                    Message = string.Format(tpl, this.GetCmp<FileUploadField>("FileUploadField").PostedFile.FileName, this.GetCmp<FileUploadField>("FileUploadField").PostedFile.ContentLength)
                });
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Fail",
                    Message = "No file uploaded"
                });
            }

            var template = ParseExcel(Path.Combine(_filesDir,this.GetCmp<FileUploadField>("FileUploadField").PostedFile.FileName));

            if (template != null)
            {
                var file = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(_template));
                file.MoveTo(file.FullName + file.LastWriteTime.ToString("yyyyMMddHHmmss"));

                var fileStream = new FileStream(System.Web.HttpContext.Current.Server.MapPath(_template),
                    FileMode.Create);
                if(!template.Serialize(fileStream))
                {
                    fileStream.Close();
                    X.Msg.Alert("", "请检查上传的隐患整改措施文件以及数据格式").Show();
                }
            }
            
            var result = new DirectResult {IsUpload = true};
            return result;
        }

        //0 隐患大类	1隐患小类	2可能的原因	3整改措施	4备注
        protected virtual ThreatCorrection ParseExcel(string file)
        {
            var template = new ThreatCorrection();
            try
            {
                var stream = new FileStream(file,FileMode.Open);
                using (var excelReader = ExcelReaderFactory.CreateBinaryReader(stream))
                {
                    excelReader.IsFirstRowAsColumnNames = true;
                    var table = excelReader.AsDataSet();
                    var catgoryList = new List<ThreatCorrectionCatgory>();
                    ThreatCorrectionCatgory catgory = null;
                    var threatTypeList = new List<ThreatCorrectionCatgoryType>();
                    var catgoryText = "";
                    for (var rowIndex = 0; rowIndex < table.Tables[0].Rows.Count; rowIndex++)
                    {
                        var row = table.Tables[0].Rows[rowIndex];
                        var columnIndex = 0;
                        var threatType = new ThreatCorrectionCatgoryType();
                        foreach (string cellValue in row.ItemArray.Where(cellValue => cellValue.GetType() == string.Empty.GetType()))
                        {
                            switch (columnIndex++)
                            {
                                case 0:
                                    if (catgoryText != cellValue)
                                    {
                                        if (catgory != null)
                                        {
                                            catgory.Type = threatTypeList.ToArray();
                                            threatTypeList.Clear();
                                            catgoryList.Add(catgory);
                                        }
                                        
                                        catgory = new ThreatCorrectionCatgory
                                        {
                                            Text = cellValue.ToCharArray().Select(c => c.ToString()).ToArray()
                                        };
                                    }
                                    catgoryText = cellValue;
                                    continue;
                                case 1:
                                    threatType.Text = cellValue.ToCharArray().Select(c=>c.ToString()).ToArray();
                                    continue;
                                case 2:
                                    threatType.Cause = cellValue;
                                    continue;
                                case 3:
                                    threatType.Correction = cellValue;
                                    continue;
                                case 4:
                                    threatType.Description = cellValue;
                                    break;
                            }
                        }
                        threatTypeList.Add(threatType);

                        if (rowIndex < table.Tables[0].Rows.Count - 1) continue;
                        Debug.Assert(catgory != null, "catgory != null");
                        catgory.Type = threatTypeList.ToArray();
                        catgoryList.Add(catgory);
                    }

                    excelReader.Close();
                    template.Catgory = catgoryList.ToArray();
                    template.ModifyDate = DateTime.Now;
                    template.Version += 1;
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                X.Msg.Alert("", "请检查上传的隐患整改措施文件以及数据格式").Show();
            }

            return template;
        }
    }
}
