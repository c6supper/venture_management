using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using Ext.Net;
using Ext.Net.Utilities;

namespace VentureManagement.Web.Models
{
    public class MenusModel
    {
        internal static string MenusRoot = "~/Areas/";
        
        public static NodeCollection GetMenuNodes()
        {
            var nodes = new NodeCollection();
            HttpContext.Current.Server.MapPath(MenusRoot);

            return BuildAreasLevel();
        }

        public static string ApplicationRoot
        {
            get
            {
                string root = HttpContext.Current.Request.ApplicationPath;
                return root == "/" ? "" : root;
            }
        }

        private static readonly string[] excludeList = { ".svn", "_svn", "Shared" };

        private static NodeCollection BuildAreasLevel()
        {
            string path = HttpContext.Current.Server.MapPath(MenusRoot);
            DirectoryInfo root = new DirectoryInfo(path);
            DirectoryInfo[] folders = root.GetDirectories();
            folders = SortFolders(root, folders);

            NodeCollection nodes = new NodeCollection(false);

            foreach (DirectoryInfo folder in folders)
            {
                if ((folder.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden ||
                    excludeList.Contains(folder.Name) || folder.Name.StartsWith("_"))
                {
                    continue;
                }

                MenuConfig cfg = new MenuConfig(folder.FullName + "\\config.xml");

                string iconCls = string.IsNullOrEmpty(cfg.IconCls) ? "" : cfg.IconCls;
                Node node = null;
                var index = folder.Name.IndexOf('_');
                
                if (cfg.MainGroup || index < 0)
                {
                    node = new Node();
                    node.NodeID = BaseControl.GenerateID();
                    node.Text = GetDisplayName(folder.Name);
                    nodes.Add(node);

                    node.IconCls = iconCls;
                    if (IsNew(folder.FullName))
                    {
                        node.CustomAttributes.Add(new ConfigItem("isNew", "true", ParameterMode.Raw));
                    }
                }
                else
                {
                    var mainGroupName = GetDisplayName(folder.Name.Substring(0, index));
                    node = nodes.FirstOrDefault(n => n.Text == mainGroupName);

                    if (node == null)
                    {
                        node = new Node();
                        node.NodeID = BaseControl.GenerateID();
                        node.Text = mainGroupName;
                        nodes.Add(node);
                    }

                    if (iconCls.IsNotEmpty())
                    {
                        node.IconCls = iconCls;
                    }

                    if (IsNew(folder.FullName) && !node.CustomAttributes.Contains("isNew"))
                    {
                        node.CustomAttributes.Add(new ConfigItem("isNew", "true", ParameterMode.Raw));
                    }
                    
                    var groupNode = new Node();

                    groupNode.NodeID = BaseControl.GenerateID();
                    groupNode.Text = GetDisplayName(folder.Name.Substring(index + 1).Replace("_", " "));

                    if (IsNew(folder.FullName) && !groupNode.CustomAttributes.Contains("isNew"))
                    {
                        groupNode.CustomAttributes.Add(new ConfigItem("isNew", "true", ParameterMode.Raw));
                    }

                    node.Children.Add(groupNode);
                    node = groupNode;
                }

                BuildViewsLevel(folder, node);
            }

            return nodes;
        }

        private static void BuildViewsLevel(DirectoryInfo area, Node areaNode)
        {
            DirectoryInfo[] folders = new DirectoryInfo(area.FullName + "\\Views").GetDirectories();

            folders = SortFolders(area, folders);

            foreach (DirectoryInfo folder in folders)
            {
                if ((folder.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden ||
                    excludeList.Contains(folder.Name) || folder.Name.StartsWith("_"))
                {
                    continue;
                }

                MenuConfig cfg = new MenuConfig(folder.FullName + "\\config.xml");

                string iconCls = string.IsNullOrEmpty(cfg.IconCls) ? "" : cfg.IconCls;
                Node node = new Node();

                var type = Type.GetType(Assembly.GetExecutingAssembly().GetName().Name + "." + area.Parent.Name + "." + area.Name + ".Controllers." + folder.Name + "Controller");
                var attr = Common.ReflectionHelper.GetCustomAttribute<System.Web.Mvc.AuthorizeAttribute>(type);

                node.Text = GetDisplayName(folder.Name.Replace("_", " "));
                
                if (IsNew(folder.FullName))
                {
                    node.CustomAttributes.Add(new ConfigItem("isNew", "true", ParameterMode.Raw));
                }

                node.IconCls = iconCls;
                string url = string.Concat(ApplicationRoot, "/", area.Name, "/", folder.Name, "/");
                node.NodeID = "e" + Math.Abs(url.ToLower().GetHashCode());
                node.Href = url;

                node.Leaf = true;

                areaNode.Children.Add(node);
            }
        }

        private static MenuConfig rootCfg;

        private static string GetDisplayName(string name)
        {
            string display = String.Empty;
            if (rootCfg == null)
            {
                rootCfg = new MenuConfig(new DirectoryInfo(HttpContext.Current.Server.MapPath(MenusRoot)) + "\\config.xml");
            }

            if (rootCfg.MenuDisplays.ContainsKey(name))
                return rootCfg.MenuDisplays[name];

            return name;
        }

        private static bool IsNew(string folder)
        {
            if (rootCfg == null)
            {
                rootCfg = new MenuConfig(new DirectoryInfo(HttpContext.Current.Server.MapPath(MenusRoot)) + "\\config.xml");
            }

            foreach (string newFolder in rootCfg.NewFolders)
            {
                if (string.Concat(HttpContext.Current.Server.MapPath(MenusRoot), newFolder).StartsWith(folder, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static DirectoryInfo[] SortFolders(DirectoryInfo root, DirectoryInfo[] folders)
        {
            string cfgPath = root.FullName + "\\config.xml";

            if (File.Exists(root.FullName + "\\config.xml"))
            {
                MenuConfig rootCfg = new MenuConfig(cfgPath);

                if (rootCfg.OrderFolders.Count > 0)
                {
                    List<DirectoryInfo> list = new List<DirectoryInfo>(folders);
                    int pasteIndex = 0;

                    foreach (string orderFolder in rootCfg.OrderFolders)
                    {
                        int dIndex = 0;

                        for (int ind = 0; ind < list.Count; ind++)
                        {
                            if (list[ind].Name.ToLower() == orderFolder)
                            {
                                dIndex = ind;
                            }
                        }

                        if (dIndex > 0)
                        {
                            DirectoryInfo di = list[dIndex];
                            list.RemoveAt(dIndex);
                            list.Insert(pasteIndex++, di);
                        }
                    }

                    folders = list.ToArray();
                }
            }

            return folders;
        }
    }

    public class MenuConfig
    {
        private string path;

        public MenuConfig(string path)
        {
            this.path = path;
            this.Load();
        }

        private void Load()
        {
            this.Description = "No description";
            XmlDocument xml = new XmlDocument();

            if (File.Exists(path))
            {
                try
                {
                    xml.Load(path);
                }
                catch (FileNotFoundException)
                {
                    return;
                }
            }

            XmlNode root = xml.SelectSingleNode("menu");

            if (root == null)
            {
                return;
            }

            XmlAttribute iconCls = root.Attributes["iconCls"];

            if (iconCls != null)
            {
                this.IconCls = iconCls.Value;
            }

            XmlAttribute mainGroup = root.Attributes["group"];

            if (mainGroup != null)
            {
                this.MainGroup = mainGroup.Value == "1";
            }

            XmlNode desc = root.SelectSingleNode("description");

            if (desc != null)
            {
                this.Description = desc.InnerText;
            }

            XmlNodeList folders = root.SelectNodes("order/folder");

            if (folders != null)
            {
                foreach (XmlNode folder in folders)
                {
                    XmlAttribute urlAttr = folder.Attributes["name"];

                    if (urlAttr != null && !string.IsNullOrEmpty(urlAttr.InnerText))
                    {
                        string folderName = urlAttr.InnerText;
                        this.OrderFolders.Add(folderName.ToLower());
                    }
                }
            }

            folders = root.SelectNodes("menuAttribute/menu");

            if (folders != null)
            {
                foreach (XmlNode folder in folders)
                {
                    XmlAttribute nameAttribute = folder.Attributes["name"];
                    XmlAttribute displayAttribute = folder.Attributes["display"];

                    if (nameAttribute != null && !string.IsNullOrEmpty(nameAttribute.InnerText)
                        && displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.InnerText))
                    {
                        this.MenuDisplays.Add(nameAttribute.InnerText, displayAttribute.InnerText);
                    }
                }
            }
        }

        public string IconCls { get; private set; }

        public bool MainGroup { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        private List<string> orderFolders;
        public List<string> OrderFolders
        {
            get
            {
                if (this.orderFolders == null)
                {
                    this.orderFolders = new List<string>();
                }
                return this.orderFolders;
            }
        }

        private List<string> newFolders;
        public List<string> NewFolders
        {
            get
            {
                if (this.newFolders == null)
                {
                    this.newFolders = new List<string>();
                }
                return this.newFolders;
            }
        }

        private Dictionary<string,string> _menuDisplays;
        public Dictionary<string, string> MenuDisplays
        {
            get { return this._menuDisplays ?? (this._menuDisplays = new Dictionary<string, string>()); }
        }
    }
}