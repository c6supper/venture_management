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
    public class RoleModel
    {
        private static NodeCollection GetFilterdNode(NodeCollection nodes)
        {
            NodeCollection filterdNodeCollection = new NodeCollection();
            foreach (var node in nodes)
            {
                if(node.Children.Count == 0)
                    filterdNodeCollection.Add(node);
                else
                    filterdNodeCollection.AddRange(GetFilterdNode(node.Children));
            }
            return filterdNodeCollection;
        }

        public static NodeCollection GetRoleNode()
        {
            NodeCollection roleNodeCollection = new NodeCollection();

            foreach (var node in GetFilterdNode(MenusModel.GetMenuNodes()))
            {
                var roleNodeRead = node;
                roleNodeRead.NodeID += "_read";
                roleNodeRead.Text = node.Text.Replace(" ", "_") + "Read";

                var roleNodeWrite = node;
                roleNodeWrite.NodeID += "_write";
                roleNodeWrite.Text = node.Text.Replace(" ", "_") + "Write";

                roleNodeCollection.Add(roleNodeRead);
                roleNodeCollection.Add(roleNodeWrite);
            }

            return roleNodeCollection;
        }
    }
}