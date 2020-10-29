using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CTGP7
{
    public class CTGP7CourseList
    {
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class NameInfo
        {
            public NameInfo()
            {
                entryField = new NameEntry[3] { new NameEntry(), new NameEntry(), new NameEntry() };
                entryField[0].courseID = entryField[1].courseID = entryField[2].courseID = 0;
                entryField[0].courseType = NameEntry.CourseType.OriginalRace;
                entryField[1].courseType = NameEntry.CourseType.CustomRace;
                entryField[2].courseType = NameEntry.CourseType.OriginalBattle;
                entryField[0].szsName = entryField[1].szsName = entryField[2].szsName = "dummy";
                entryField[0].humanName = entryField[1].humanName = entryField[2].humanName = "Data not loaded";
            }

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
            public enum CourseType
            {
                OriginalRace = 0,
                CustomRace = 1,
                OriginalBattle = 2,
                Unknown=7
            }

            private string szsNameField;

            private string humanNameField;

            [XmlIgnore]
            public uint courseID { get; set; }

            private CourseType courseTypeField;

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

            [XmlAttribute(AttributeName = "courseID")]
            public string hexCourseID
            {
                get
                {
                    // convert int to hex representation
                    return courseID.ToString("X");
                }
                set
                {
                    // convert hex representation back to int
                    courseID = uint.Parse(value,
                        System.Globalization.NumberStyles.AllowHexSpecifier);
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public CourseType courseType
            {
                get
                {
                    return this.courseTypeField;
                }
                set
                {
                    this.courseTypeField = value;
                }
            }
            public static string HumanCourseType(CourseType type)
            {
                switch (type)
                {
                    case CourseType.OriginalRace:
                        return "Original Tracks";
                    case CourseType.CustomRace:
                        return "Custom Tracks";
                    case CourseType.OriginalBattle:
                        return "Original Arenas";
                    default:
                        return "Other";
                }
            }
            public static CourseType EnumCourseType(string type)
            {
                switch(type)
                {
                    case "Custom Tracks":
                        return CourseType.CustomRace;
                    case "Original Arenas":
                        return CourseType.OriginalBattle;
                    case "Original Tracks":
                        return CourseType.OriginalRace;
                    default:
                        return CourseType.Unknown;
                }
            }
        }

        public NameInfo Info;
        private static CTGP7CourseList InstanceImpl = null;
        public static CTGP7CourseList Instance
        {
            get
            {
                if (InstanceImpl == null) InstanceImpl = new CTGP7CourseList();
                return InstanceImpl;
            }
        }
        private void LoadFromFile()
        {
            byte[] xmlfile = File.ReadAllBytes(Application.StartupPath + @"\Plugins\CTGP-7CourseNames.xml");
            MemoryStream m = new MemoryStream(xmlfile);
            XmlSerializer deserializer = new XmlSerializer(typeof(NameInfo));
            TextReader reader = new StreamReader(m);
            Info = (NameInfo)deserializer.Deserialize(reader);
            reader.Close();
        }
        private void LoadDefault()
        {
            Info = new NameInfo();
        }
        private CTGP7CourseList()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                LoadDefault();
                return;
            }
            try
            {
                LoadFromFile();
            }
            catch
            {
                if(DownloadFromInternet(true))
                {
                    try
                    {
                        LoadFromFile();
                    }
                    catch
                    {
                        LoadDefault();
                    }
                } else LoadDefault();
            }
        }
        public NameEntry NameEntryFromHumanName(string humanName)
        {
            if (humanName == null) return null;
            foreach (var i in Info.Entry)
            {
                if (i.humanName == humanName) return i;
            }
            return null;
        }
        public NameEntry NameEntryFromSzsName(string szsName)
        {
            if (szsName == null) return null;
            foreach (var i in Info.Entry)
            {
                if (i.szsName == szsName) return i;
            }
            return new NameEntry() { humanName = szsName, szsName = szsName, courseID = 0, courseType = NameEntry.CourseType.Unknown};
        }
        public NameEntry NameEntryFromID(uint ID)
        {
            foreach (var i in Info.Entry)
            {
                if (i.courseID == ID) return i;
            }
            return null;
        }
        public static bool DownloadFromInternet(bool isCorrupted)
        {
            bool fileExists = File.Exists(Application.StartupPath + @"\Plugins\CTGP-7CourseNames.xml");
            DialogResult res;
            if (!isCorrupted)
            {
                res = MessageBox.Show("Download the most recent CTGP-7 course names?", "CTGP-7 Course Names", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            } else
            {
                res = MessageBox.Show("The CTGP-7 course names file doesn't exist or is corrupted.\nDownload the most recent version?", "CTGP-7 Course Names", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (res == DialogResult.Yes)
            {
                if (fileExists) File.Delete(Application.StartupPath + @"\Plugins\CTGP-7CourseNames.xml");
            }
            else return false;
            System.Net.WebClient webClient = new System.Net.WebClient();
            try
            {
                webClient.DownloadFile("https://raw.githubusercontent.com/mariohackandglitch/CTGP7EFEPlugin/master/CTGP-7CourseNames.xml", Application.StartupPath + @"\Plugins\CTGP-7CourseNames.xml");
            } catch (Exception e)
            {
                MessageBox.Show("Failed to download data:\n\n" + e.Message, "CTGP-7 Course Names", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            MessageBox.Show("Data downloaded successfully!" + (isCorrupted ? "" : "\nPlease restart application for changes to take effect."), "CTGP-7 Course Names", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

    }
}

