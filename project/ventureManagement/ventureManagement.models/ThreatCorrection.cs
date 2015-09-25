using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace VentureManagement.Models
{

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
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
                serializer.Serialize(file, this);

                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            return false;
        }

        /// <remarks/>
        [XmlElementAttribute("Category")]
        public ThreatCorrectionCategory[] Category { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute]
        public decimal Version { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute(DataType = "date")]
        public DateTime ModifyDate { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class ThreatCorrectionCategory
    {
        /// <remarks/>
        public string CategoryName { get; set; }

        /// <remarks/>
        [XmlElementAttribute("Type")]
        public ThreatCorrectionCategoryType[] Type { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class ThreatCorrectionCategoryType
    {
        /// <remarks/>
        public string TypeName { get; set; }

        /// <remarks/>
        public string Cause { get; set; }

        /// <remarks/>
        public string Correction { get; set; }

        /// <remarks/>
        public string Description { get; set; }
    }
}