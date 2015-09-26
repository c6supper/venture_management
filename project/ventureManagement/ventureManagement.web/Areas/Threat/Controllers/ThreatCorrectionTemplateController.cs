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
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_THREATCORRECTIONTEMPLATE_WRITE + "," + Role.PERIMISSION_THREATCORRECTIONTEMPLATE_READ)]
    public class ThreatCorrectionTemplateController : ThreatBaseController
    {
        //
        // GET: /Threat/ThreatCorrectionTemplate/
        // ReSharper disable once InconsistentNaming
        public ActionResult Index()
        {
            return View();
        }

        // ReSharper disable once InconsistentNaming
        private readonly string _filesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

        public ActionResult UploadClick()
        {
            const string tpl = "模板文件: {0}<br/>大小: {1} 字节";

            if (this.GetCmp<FileUploadField>("FileUploadField").HasFile)
            {
                this.GetCmp<FileUploadField>("FileUploadField").PostedFile.SaveAs(Path.Combine(_filesDir, this.GetCmp<FileUploadField>("FileUploadField").PostedFile.FileName));
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "上传失败",
                    Message = "请重新选择文件",
                    Handler = "#{BasicForm}.getForm().reset();"
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
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "上传失败",
                        Message = "请检查上传的隐患整改措施文件以及数据格式",
                        Handler = "#{BasicForm}.getForm().reset();"
                    });
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO,
                        Title = "上传成功",
                        Message = string.Format(tpl, this.GetCmp<FileUploadField>("FileUploadField").PostedFile.FileName, this.GetCmp<FileUploadField>("FileUploadField").PostedFile.ContentLength),
                        Handler = "#{BasicForm}.getForm().reset();"
                    });
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
                    var catgoryList = new List<ThreatCorrectionCategory>();
                    ThreatCorrectionCategory category = null;
                    var threatTypeList = new List<ThreatCorrectionCategoryType>();
                    var categoryText = "";
                    for (var rowIndex = 0; rowIndex < table.Tables[0].Rows.Count; rowIndex++)
                    {
                        var row = table.Tables[0].Rows[rowIndex];
                        var columnIndex = 0;
                        var threatType = new ThreatCorrectionCategoryType();
                        foreach (string cellValue in row.ItemArray.Where(cellValue => cellValue.GetType() == string.Empty.GetType()))
                        {
                            switch (columnIndex++)
                            {
                                case 0:
                                    if (categoryText != cellValue)
                                    {
                                        if (category != null)
                                        {
                                            category.Type = threatTypeList.ToArray();
                                            threatTypeList.Clear();
                                            catgoryList.Add(category);
                                        }
                                        
                                        category = new ThreatCorrectionCategory
                                        {
                                            CategoryName = cellValue
                                        };
                                    }
                                    categoryText = cellValue;
                                    continue;
                                case 1:
                                    threatType.TypeName = cellValue;
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
                        Debug.Assert(category != null, "category != null");
                        category.Type = threatTypeList.ToArray();
                        catgoryList.Add(category);
                    }

                    excelReader.Close();
                    template.Category = catgoryList.ToArray();
                    template.ModifyDate = DateTime.Now;
                    template.Version += 1;
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "上传失败",
                    Message = "请检查上传的隐患整改措施文件以及数据格式",
                    Handler = "#{BasicForm}.getForm().reset();"
                });
            }

            return template;
        }
    }
}
