using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace VentureManagement.Models
{
    /// <remarks />
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
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

        public bool Serialize(FileStream file)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(ThreatCorrection));
                serializer.Serialize(file,this);

                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            return false;
        }

        /// <remarks />
        [XmlElement("Catgory")]
        public ThreatCorrectionCatgory[] Catgory { get; set; }

        /// <remarks />
        [XmlAttribute]
        public decimal Version { get; set; }

        /// <remarks />
        [XmlAttribute(DataType = "date")]
        public DateTime ModifyDate { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class ThreatCorrectionCatgory
    {
        /// <remarks />
        [XmlElement("Type")]
        public ThreatCorrectionCatgoryType[] Type { get; set; }

        /// <remarks />
        [XmlText]
        public string[] Text { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class ThreatCorrectionCatgoryType
    {
        /// <remarks />
        public string Cause { get; set; }

        /// <remarks />
        public string Correction { get; set; }

        /// <remarks />
        public object Description { get; set; }

        /// <remarks />
        [XmlText]
        public string[] Text { get; set; }
    }
}