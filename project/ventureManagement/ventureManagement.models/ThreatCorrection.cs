using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace VentureManagement.Models
{
    public class ThreatCorrection
    {
        public static ThreatCorrection Deserialize(string templateFile)
        {
            try
            {
                using (TextReader reader = new StreamReader(templateFile))
                {
                    var serializer = new XmlSerializer(typeof(ThreatCorrection));
                    return serializer.Deserialize(reader) as ThreatCorrection;
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            return null;
        }

        /// <remarks />
        [XmlType(AnonymousType = true)]
        [XmlRoot(Namespace = "", IsNullable = false)]
        public class CorrectionDataBase
        {
            /// <remarks />
            [XmlElement("Catgory")]
            public CorrectionDataBaseCatgory[] Catgory { get; set; }

            /// <remarks />
            [XmlAttribute]
            public decimal Version { get; set; }

            /// <remarks />
            [XmlAttribute(DataType = "date")]
            public DateTime ModifyDate { get; set; }
        }

        /// <remarks />
        [XmlType(AnonymousType = true)]
        public class CorrectionDataBaseCatgory
        {
            /// <remarks />
            [XmlElement("Type")]
            public CorrectionDataBaseCatgoryType[] Type { get; set; }

            /// <remarks />
            [XmlText]
            public string[] Text { get; set; }
        }

        /// <remarks />
        [XmlType(AnonymousType = true)]
        public class CorrectionDataBaseCatgoryType
        {
            /// <remarks />
            [XmlElement("Cause")]
            public CorrectionDataBaseCatgoryTypeCause[] Cause { get; set; }

            /// <remarks />
            [XmlText]
            public string[] Text { get; set; }
        }

        /// <remarks />
        [XmlType(AnonymousType = true)]
        public class CorrectionDataBaseCatgoryTypeCause
        {
            /// <remarks />
            [XmlElement("Correction")]
            public string[] Correction { get; set; }

            /// <remarks />
            [XmlText]
            public string[] Text { get; set; }
        }
    }
}