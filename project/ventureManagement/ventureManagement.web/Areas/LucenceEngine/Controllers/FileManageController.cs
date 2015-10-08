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
using VentureManagement.Models;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Areas.LucenceEngine.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_FILEMANAGE)]
    public class FileManageController : Controller
    {
        //
        // GET: /LucenceEngine/FileManage/
        public ActionResult Index()
        {
            return View();
        }

        private string _filesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

        public ActionResult UploadClick(string ComboBoxCountry, string ComboBoxCity)
        {
            string tpl = "Uploaded file: {0}<br/>Size: {1} bytes";

            if (this.GetCmp<FileUploadField>("FileUploadField1").HasFile)
            {
                string strSavePath = _filesDir;
                if (!string.IsNullOrEmpty(ComboBoxCountry))
                {
                    strSavePath = Path.Combine(strSavePath, ComboBoxCountry);
                    if (!string.IsNullOrEmpty(ComboBoxCity) && ComboBoxCountry != "企业内部管理制度")
                    {
                        strSavePath = Path.Combine(strSavePath, ComboBoxCity);
                    }
                }

                if (!System.IO.Directory.Exists(strSavePath))
                {
                    System.IO.Directory.CreateDirectory(strSavePath);
                }

                strSavePath = Path.Combine(strSavePath, this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.FileName);

                this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.SaveAs(strSavePath);
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
