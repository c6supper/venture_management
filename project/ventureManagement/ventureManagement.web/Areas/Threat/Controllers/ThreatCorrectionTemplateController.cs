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

            ParseExcel(Path.Combine(_filesDir,this.GetCmp<FileUploadField>("FileUploadField").PostedFile.FileName));
            
            var result = new DirectResult {IsUpload = true};
            return result;
        }

        protected virtual ThreatCorrection ParseExcel(string file)
        {
            var template = new ThreatCorrection();
            try
            {
                var missing = System.Reflection.Missing.Value;
                var stream = new FileStream(file,FileMode.Open);
                using (var excelReader = ExcelReaderFactory.CreateBinaryReader(stream))
                {
                    excelReader.IsFirstRowAsColumnNames = true;

                    var table = excelReader.AsDataSet();

                    for (var rowIndex = 0; rowIndex < table.Tables[0].Rows.Count; rowIndex++)
                    {
                        var row = table.Tables[0].Rows[rowIndex];
                        foreach (var cellValue in row.ItemArray.Where(cellValue => cellValue.GetType() == string.Empty.GetType()))
                        {
                            //template.Catgory = new T;
                            //template.Catgory.Concat()
                        }
                    }

                    excelReader.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            X.Msg.Alert("", "请检查上传的隐患整改措施文件以及数据格式").Show();

            return template;
        }
    }
}
