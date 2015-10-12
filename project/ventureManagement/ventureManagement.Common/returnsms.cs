using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VentureManagement.Models;

// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToAutoProperty

namespace Common
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class returnsms
    {
        public static returnsms Deserialize(string templateFile)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(returnsms));
                //return serializer.Deserialize(templateFile) as returnsms;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            return null;
        }

        private string returnstatusField;

        private string messageField;

        private string remainpointField;

        private string taskIDField;

        private string successCountsField;

        /// <remarks/>
        public string returnstatus
        {
            get
            {
                return this.returnstatusField;
            }
            set
            {
                this.returnstatusField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public string remainpoint
        {
            get
            {
                return this.remainpointField;
            }
            set
            {
                this.remainpointField = value;
            }
        }

        /// <remarks/>
        public string taskID
        {
            get
            {
                return this.taskIDField;
            }
            set
            {
                this.taskIDField = value;
            }
        }

        /// <remarks/>
        public string successCounts
        {
            get
            {
                return this.successCountsField;
            }
            set
            {
                this.successCountsField = value;
            }
        }
    }


}
