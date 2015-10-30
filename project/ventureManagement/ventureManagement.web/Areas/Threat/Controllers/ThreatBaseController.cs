using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Controllers;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    public class ThreatBaseController : BaseController
    {
        //
        // GET: /Threat/ThreatBase/
        // ReSharper disable once InconsistentNaming
        protected const string _template = "~/Areas/Threat/Content/CorrectionTemplate.xml";
        // ReSharper disable once InconsistentNaming
        protected readonly ThreatCorrection _correctionTemplate = ThreatCorrection.Deserialize(System.Web.HttpContext.Current.Server.MapPath(_template));
        // ReSharper disable once InconsistentNaming
        protected readonly InterfaceThreatCaseService _threatCaseService;
        protected readonly InterfaceThreatCaseAttachmentService _threatCaseAttachmentService;

        protected const string ImgKey = "IMGKEY";

        private static readonly string ThreatCaseAttachmentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Files\\ThreatCaseAttachments");

        protected ThreatBaseController()
        {
            _threatCaseService = new ThreatCaseService(_currentOrgList);
            _threatCaseAttachmentService = new ThreatCaseAttachmentService();
        }

        protected bool SaveAttachments(int threatCaseId)
        {
            try
            {
                var imgsString = Session[ImgKey] as string;
                var subFolderPath = Convert.ToString(threatCaseId) + "_" + DateTime.Now.ToString("yyyy_MM_dd");
                var currentFileFolder = Path.Combine(ThreatCaseAttachmentPath, subFolderPath);

                if (!Directory.Exists(currentFileFolder))
                {
                    Directory.CreateDirectory(currentFileFolder);
                }

                if (imgsString != null)
                {
                    foreach (var imgString in imgsString.Split(Convert.ToChar(";")))
                    {
                        var currentFilePath = Path.Combine(currentFileFolder, Convert.ToString(DateTime.Now.Ticks)) + ".jpg";
                        var attachment = new ThreatCaseAttachment
                        {
                            ThreatCaseId = threatCaseId,
                            AttachmentUrl = "~\\" + currentFilePath.Replace(Server.MapPath("~\\"), "")
                        };
                        _threatCaseAttachmentService.Add(attachment);

                        var byteImg = Convert.FromBase64String(imgString);
                        var ms = new MemoryStream(byteImg);
                        var bmp = new Bitmap(ms);
                        bmp.Save(currentFilePath, ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }

            return true;
        }
    }
}
