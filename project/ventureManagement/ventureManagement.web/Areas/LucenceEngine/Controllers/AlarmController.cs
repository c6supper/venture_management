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
using PanGu;
using VentureManagement.Models;
using VentureManagement.Web.Areas.LucenceEngine.Models;
using Field = Lucene.Net.Documents.Field;

namespace VentureManagement.Web.Areas.LucenceEngine.Controllers
{
    public class AlarmController : Controller
    {
        //
        // GET: /LucenceEngine/Alarm/
        private int _page = 1;
        private int _pageSize = 5;
        private int _maxNumber = 100;
        private static string _keyword = string.Empty;
        private static List<Alarm> _alarmResultList = new List<Alarm>();

        public ActionResult Index()
        {
            GetAllIndex();
            Paged(_page, _pageSize);

            return View();
        }

        /*
        public ActionResult Sortable(bool? ascending, string sortBy = "EmployeeNO")
        {
            var model = new AlarmGridModel()
            {
                SortBy = sortBy,
                SortAscending = ascending.GetValueOrDefault(),
                Employees = DataContext.Employee
            };

            model.Employees = DataContext.Employee.OrderBy(model.SortExpression);

            return View(model);
        }*/

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

        private string _filesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
        private string _luceneDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LuceneData");
        private PanGuAnalyzer _analyzer = new PanGuAnalyzer();
        /*
        private FSDirectory _directoryTemp;
        private FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));

                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);

                var lockFilePath = Path.Combine(_luceneDir, "write.lock");

                if (System.IO.File.Exists(lockFilePath))
                    System.IO.File.Delete(lockFilePath);

                return _directoryTemp;
            }
        }
        */

        private void GetAllIndex()
        {
            if (!System.IO.Directory.Exists(_filesDir))
            {
                System.IO.Directory.CreateDirectory(_filesDir);
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(_filesDir);
            FileInfo[] files = dirInfo.GetFiles();
            if (files.Count() == 0)
            {
                //MessageBox.Show("Files目录下没有*.txt文件");
                return;
            }

            if (!System.IO.Directory.Exists(_luceneDir))
            {
                System.IO.Directory.CreateDirectory(_luceneDir);
            }

            IndexWriter writer = new IndexWriter(FSDirectory.Open(new DirectoryInfo(_luceneDir)), _analyzer, true, IndexWriter.MaxFieldLength.LIMITED);

            for (int i = 0; i < files.Count(); i++)
            {
                FileInfo fileInfo = files[i];
                StreamReader reader = new StreamReader(fileInfo.FullName);

                //OutputMessage("正在索引文件[" + fileInfo.Name + "]");

                Document doc = new Document();
                doc.Add(new Field("FileName", fileInfo.Name, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Content", reader.ReadToEnd(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Path", fileInfo.FullName, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Date", fileInfo.LastWriteTime.ToString(), Field.Store.YES, Field.Index.ANALYZED));

                writer.AddDocument(doc);
                writer.Optimize();
            }
            writer.Dispose();
        }

        public ActionResult Search(string keyword, int page = 1, int pageSize = 5)
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
                reader = IndexReader.Open(FSDirectory.Open(new DirectoryInfo(_luceneDir)), true);
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
                    Document doc = searcher.Doc(hit.Doc);
                    Field fileNameField = doc.GetField("FileName");
                    Field content = doc.GetField("Content");
                    Field pathField = doc.GetField("Path");
                    Field date = doc.GetField("Date");

                    Alarm a = new Alarm();
                    a.AlarmId = i + 1;
                    a.FileName = fileNameField.StringValue;
                    a.Content = Preview(content.StringValue, _keyword);//content.StringValue;
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
