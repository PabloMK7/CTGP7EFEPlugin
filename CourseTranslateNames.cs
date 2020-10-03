using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CTGP_7_Music_Slot_User
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class NameInfo
    {

        private NameEntry[] entryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Entry")]
        public NameEntry[] Entry
        {
            get
            {
                return this.entryField;
            }
            set
            {
                this.entryField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class NameEntry
    {

        private string szsNameField;

        private string humanNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string szsName
        {
            get
            {
                return this.szsNameField;
            }
            set
            {
                this.szsNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string humanName
        {
            get
            {
                return this.humanNameField;
            }
            set
            {
                this.humanNameField = value;
            }
        }
    }

    public class NameList
    {
        public NameInfo nameInf;

        public NameList()
        {
            try
            {
                byte[] xmlfile = File.ReadAllBytes(Application.StartupPath + @"\TranslateName.xml");
                MemoryStream m = new MemoryStream(xmlfile);
                XmlSerializer deserializer = new XmlSerializer(typeof(NameInfo));
                TextReader reader = new StreamReader(m);
                nameInf = (NameInfo)deserializer.Deserialize(reader);
                reader.Close();
            }
            catch
            {
                nameInf = new NameInfo();
            }
        }
        public string GetHumanName(string szsFileName)
        {
            if (szsFileName == null) return null;
            foreach(var i in nameInf.Entry)
            {
                if (i.szsName == szsFileName) return i.humanName;
            }
            return null;
        }
        public string GetSzsName(string humanN)
        {
            if (humanN == null) return null;
            foreach (var i in nameInf.Entry)
            {
                if (i.humanName == humanN) return i.szsName;
            }
            return null;
        }
    }
}

