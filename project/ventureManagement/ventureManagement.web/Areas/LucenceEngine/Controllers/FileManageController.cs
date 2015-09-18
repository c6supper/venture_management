using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Newtonsoft.Json.Serialization;

namespace VentureManagement.Web.Areas.LucenceEngine.Controllers
{
    public class FileManageController : Controller
    {
        //
        // GET: /LucenceEngine/FileManage/
        public ActionResult Index()
        {
            return View();
        }

        private string _filesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

        public ActionResult UploadClick()
        {
            string tpl = "Uploaded file: {0}<br/>Size: {1} bytes";

            if (this.GetCmp<FileUploadField>("FileUploadField1").HasFile)
            {
                this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.SaveAs(Path.Combine(_filesDir, this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.FileName));
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO,
                    Title = "Success",
                    Message = string.Format(tpl, this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.FileName, this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.ContentLength)
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
            DirectResult result = new DirectResult();
            result.IsUpload = true;
            return result;
        }
    }
}
