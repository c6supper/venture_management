using System.IO;
using System.Xml.Serialization;

namespace VentureManagement.Models
{
    public class ThreatCorrection
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class CorrectionDataBase
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Catgory")]
            public CorrectionDataBaseCatgory[] Catgory { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal Version { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
            public System.DateTime ModifyDate { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CorrectionDataBaseCatgory
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Type")]
            public CorrectionDataBaseCatgoryType[] Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string[] Text { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CorrectionDataBaseCatgoryType
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Cause")]
            public CorrectionDataBaseCatgoryTypeCause[] Cause { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string[] Text { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CorrectionDataBaseCatgoryTypeCause
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Correction")]
            public string[] Correction { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string[] Text { get; set; }
        }

        public static ThreatCorrection Deserialize(TextReader reader)
        {
            var serializer = new XmlSerializer(typeof(ThreatCorrection));

            return serializer.Deserialize(reader) as ThreatCorrection; 
        }
    }
}