using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Common;
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
using PanGu;
using VentureManagement.Models;
using VentureManagement.Web.Areas.LucenceEngine.Models;
using VentureManagement.Web.Attributes;
using Field = Lucene.Net.Documents.Field;
using System.Reflection;
using Lucene.Net.Analysis.Standard;

namespace VentureManagement.Web.Areas.LucenceEngine.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_ALARM)]
    public class AlarmController : Controller
    {
        //
        // GET: /LucenceEngine/Alarm/
        private int _page = 1;
        private int _pageSize = 20;
        private int _maxNumber = 100;
        private static string _keyword = string.Empty;
        private static List<Alarm> _alarmResultList = new List<Alarm>();

        public ActionResult Index()
        {
            _alarmResultList.Clear();
            Paged(_page, _pageSize);

            return View();
        }

        public ActionResult Paged(int page, int pageSize)
        {
            var model = new AlarmGridModel
            {
                CurrentPageIndex = page,
                PageSize = pageSize,
                TotalRecordCount = _alarmResultList.Count
            };

            model.Alarms = _alarmResultList
            .Skip((model.CurrentPageIndex - 1) * model.PageSize)
            .Take(model.PageSize);

            return View(model);
        }

        private static string _filesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
        private static string _luceneDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LuceneData");
        private StandardAnalyzer _analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

        public static void GetAllIndex()
        {
            if (!System.IO.Directory.Exists(_luceneDir))
            {
                System.IO.Directory.CreateDirectory(_luceneDir);
            }

            if (!System.IO.Directory.Exists(_filesDir))
            {
                System.IO.Directory.CreateDirectory(_filesDir);
                System.IO.Directory.CreateDirectory(Path.Combine(_filesDir, "法律及法规"));
                System.IO.Directory.CreateDirectory(Path.Combine(_filesDir, "法律及法规/国家"));
                System.IO.Directory.CreateDirectory(Path.Combine(_filesDir, "法律及法规/行业"));
                System.IO.Directory.CreateDirectory(Path.Combine(_filesDir, "法律及法规/地方"));
                System.IO.Directory.CreateDirectory(Path.Combine(_filesDir, "企业内部管理制度"));

                return;
            }

            var dirInfo = new DirectoryInfo(_filesDir);
            var list = getFileInfoList(dirInfo);
            if (list.Count == 0)
            {
                return;
            }

            var writer = new IndexWriter(FSDirectory.Open(new DirectoryInfo(_luceneDir)), new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), true, IndexWriter.MaxFieldLength.LIMITED);

            foreach (FileInfo fileInfo in list)
            {
                var doc = new Lucene.Net.Documents.Document();
                doc.Add(new Field("FileName", fileInfo.Name, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Content", File2Text(fileInfo.FullName), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Path", fileInfo.FullName, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Date", fileInfo.LastWriteTime.ToString(), Field.Store.YES, Field.Index.ANALYZED));

                writer.AddDocument(doc);
                writer.Optimize();
            }
            writer.Dispose();
        }

        public static List<FileInfo> getFileInfoList(System.IO.DirectoryInfo dir)
        {
            var list = new List<FileInfo>();
            foreach (var subdir in dir.GetDirectories())
            {
                list.AddRange(getFileInfoList(subdir));
            }
            list.AddRange(dir.GetFiles().ToList());

            return list;
        }

        private static string File2Text(string fileName)
        {
            var outText = string.Empty;
            DocumentHandler docHandler = null;

            try
            {
                if (fileName.ToLower().EndsWith(".docx"))
                {
                    docHandler = new DocumentHandler(fileName);
                    outText = docHandler.ReadWordDocument();
                }
                else
                {
                    StreamReader reader = new StreamReader(fileName);
                    outText = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (docHandler != null)
                {
                    docHandler.Dispose();
                }
            }

            return outText;
        }

        public ActionResult Search(string keyword, int page = 1, int pageSize = 20)
        {
            if (keyword != null)
            {
                keyword = keyword.Trim();                
            }

            if (string.IsNullOrEmpty(keyword))
            {
                Paged(page, pageSize);
                return View();
            }

            _keyword = keyword;
            _alarmResultList.Clear();

            IndexReader reader = null;
            IndexSearcher searcher = null;
            try
            {
                FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                if (fsDir.Directory.GetFiles().Count() == 0)
                {
                    Paged(page, pageSize);
                    return View();
                }

                reader = IndexReader.Open(fsDir, true);
                searcher = new IndexSearcher(reader);

                PerFieldAnalyzerWrapper wrapper = new PerFieldAnalyzerWrapper(_analyzer);
                wrapper.AddAnalyzer("FileName", _analyzer);
                wrapper.AddAnalyzer("Content", _analyzer);
                wrapper.AddAnalyzer("Path", _analyzer);
                wrapper.AddAnalyzer("Date", _analyzer);
                string[] fields = { "FileName", "Content", "Path", "Date" };

                QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, wrapper);
                Query query = parser.Parse(keyword);
                TopScoreDocCollector collector = TopScoreDocCollector.Create(_maxNumber, true);

                searcher.Search(query, collector);
                var hits = collector.TopDocs().ScoreDocs;

                int numTotalHits = collector.TotalHits;
                //OutputMessage("查找 " + keyword + " ...共找到 " + numTotalHits + "个匹配的文档");

                for (int i = 0; i < hits.Count(); i++)
                {
                    var hit = hits[i];
                    Lucene.Net.Documents.Document doc = searcher.Doc(hit.Doc);
                    Field fileNameField = doc.GetField("FileName");
                    Field content = doc.GetField("Content");
                    Field pathField = doc.GetField("Path");
                    Field date = doc.GetField("Date");

                    Alarm a = new Alarm();
                    a.AlarmId = i + 1;
                    a.FileName = fileNameField.StringValue;
                    a.Content = Preview(content.StringValue, _keyword);
                    a.FilePath = pathField.StringValue;
                    a.FileDate = Convert.ToDateTime(date.StringValue);
                    _alarmResultList.Add(a);
                }
            }
            finally
            {
                if (searcher != null)
                    searcher.Dispose();

                if (reader != null)
                    reader.Dispose();
            }

            Paged(page, pageSize);

            return View();
        }

        private string Preview(string body, string keyword)
        {
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter = new PanGu.HighLight.SimpleHTMLFormatter("", "");//"<font color=\"Red\">","</font>"
            PanGu.HighLight.Highlighter highlighter = new PanGu.HighLight.Highlighter(simpleHTMLFormatter, new Segment());
            highlighter.FragmentSize = 50;
            string bodyPreview = highlighter.GetBestFragment(keyword, body);
            return bodyPreview;
        }

        public FileStreamResult DownloadFile(string filePath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(filePath);
            
            return File(new FileStream(filePath, FileMode.Open), "application/octet-stream", Server.UrlEncode(dirInfo.Name));
        }
    }
}
