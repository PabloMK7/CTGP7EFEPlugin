using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using LibEveryFileExplorer;
using LibEveryFileExplorer.Files;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CTGP7
{
    public class MusicConfigEntry
    {
        [Browsable(false)]
        CTGP7CourseList info;

        [Browsable(false)]
        public string szsNameInner;

        public string CourseName
        {
            get
            {
                CTGP7CourseList.NameEntry entry = info.NameEntryFromSzsName(szsNameInner);
                if (entry == null) return "";
                return entry.humanName + " (" + CTGP7CourseList.NameEntry.HumanCourseType(entry.courseType) + ")";
            }
            set
            {
                if (value == "") szsNameInner = "";
                else szsNameInner = info.NameEntryFromHumanName(value).szsName;
            }
        }
        public string MusicFileName { get; set; }

        [Browsable(false)]
        public string MusicModeInner;

        public string MusicMode
        {
            get
            {
                switch (MusicModeInner)
                {
                    case "MULTI_WATER":
                        return "Multi Channel Water";
                    case "MULTI_AREA":
                        return "Multi Channel Area";
                    default:
                        return "Single Channel";
                }
            }
            set
            {
                switch (value)
                {
                    case "Multi Channel Water":
                        MusicModeInner = "MULTI_WATER";
                        break;
                    case "Multi Channel Area":
                        MusicModeInner = "MULTI_AREA";
                        break;
                    default:
                        MusicModeInner = "SINGLE";
                        break;
                }
            }
        }

        [Browsable(false)]
        public byte NormalBPMInner;
        public string NormalBPM
        {
            get
            {
                return NormalBPMInner.ToString();
            }
            set
            {
                try
                {
                    NormalBPMInner = Byte.Parse(value);
                }
                catch { }
            }
        }

        [Browsable(false)]
        public byte FastBPMInner;
        public string FastBPM
        {
            get
            {
                return FastBPMInner.ToString();
            }
            set
            {
                try { 
                    FastBPMInner = Byte.Parse(value);
                }
                catch { }
            }
        }

        [Browsable(false)]
        public uint NormalOffsetInner;
        public string NormalOffset
        {
            get
            {
                return NormalOffsetInner.ToString();
            }
            set
            {
                try
                {
                    NormalOffsetInner = UInt32.Parse(value);
                }
                catch { }
            }
        }

        [Browsable(false)]
        public uint FastOffsetInner;
        public string FastOffset
        {
            get
            {
                return FastOffsetInner.ToString();
            }
            set
            {
                try
                {
                    FastOffsetInner = UInt32.Parse(value);
                } catch { }
            }
        }

        public string MusicName { get; set; }

        [Browsable(false)]
        public List<string> MusicAuthorsInner;

        public string MusicAuthors
        {
            get
            {
                string ret = "";
                if (MusicAuthorsInner.Count == 0)
                    return ret;
                ret = MusicAuthorsInner[0];
                for (int i = 1; i < MusicAuthorsInner.Count; i++)
                {
                    ret += ", " + MusicAuthorsInner[i];
                }
                return ret;
            }

            set
            {
                string[] parts = value.Split(',');
                MusicAuthorsInner.Clear();
                for (int i = 0; i < parts.Length; i++)
                {
                    MusicAuthorsInner.Add(parts[i].Trim());
                }
            }
        }

        public MusicConfigEntry(CTGP7CourseList list, string courseName, string musicFileName, string musicMode, byte normalBPM, byte fastBPM, uint normalOffset, uint fastOffset) 
        {
            info = list;
            szsNameInner = courseName;
            MusicFileName = musicFileName;
            MusicModeInner = musicMode;
            NormalBPMInner = normalBPM;
            FastBPMInner = fastBPM;
            NormalOffsetInner = normalOffset;
            FastOffsetInner = fastOffset;
            MusicName = "";
            MusicAuthorsInner = new List<string>();
        }

        public MusicConfigEntry(CTGP7CourseList list)
        {
            info = list;
            szsNameInner = list.NameEntryFromID(0).szsName;
            MusicFileName = "";
            MusicMode = "";
            NormalBPMInner = 0;
            FastBPMInner = 0;
            NormalOffsetInner = 0;
            FastOffsetInner = 0;
            MusicName = "";
            MusicAuthorsInner = new List<string>();
        }
    }
    public class MusicConfigFile : FileFormat<MusicConfigFile.MCFIdentifier>, IViewable, IEmptyCreatable, IWriteable
    {
        public BindingList<MusicConfigEntry> Entries;
        public CTGP7CourseList TranslateList;
        
        public MusicConfigFile()
        {
            Entries = new BindingList<MusicConfigEntry>();
            TranslateList = CTGP7CourseList.Instance;
        }
        public MusicConfigFile(byte[] Data) : this()
        {
            System.IO.StreamReader input = new System.IO.StreamReader(new MemoryStream(Data), Encoding.UTF8);
            string line;
            while ((line = input.ReadLine()) != null)
            {
                if (line.StartsWith("#")) continue;
                string[] elements = line.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                if (elements.Length < 7) continue;
                for (int i = 0; i < elements.Length; i++)
                {
                    elements[i] = elements[i].Trim();
                }
                try
                {
                    Entries.Add(new MusicConfigEntry(TranslateList, elements[0], elements[1], elements[2], Byte.Parse(elements[3]), Byte.Parse(elements[5]), UInt32.Parse(elements[4]), UInt32.Parse(elements[6])));
                    for (int i = 7; i < elements.Length; i++)
                    {
                        if (i == 7)
                            Entries.Last().MusicName = elements[i];
                        else
                            Entries.Last().MusicAuthorsInner.Add(elements[i]);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
        public byte[] Write()
        {
            System.IO.StreamWriter output = new System.IO.StreamWriter(new MemoryStream(), new UTF8Encoding(false));
            string defaultText = "#$MSC\n#-------------------------------------------------------------------------------------------------------------------#\n# This is the custom music configuration file, you can find more info here: https://ctgp7.page.link/MusicSlotConfig #\n#-------------------------------------------------------------------------------------------------------------------#\n";
            output.WriteLine(defaultText);
            foreach(var entry in Entries)
            {
                if (entry.CourseName == "") continue;
                string line = entry.szsNameInner + " :: " + entry.MusicFileName + " :: " + entry.MusicModeInner + " :: " + entry.NormalBPMInner + " :: " + entry.NormalOffsetInner + " :: " + entry.FastBPMInner + " :: " + entry.FastOffsetInner;
                if (entry.MusicName != "")
                {
                    line += " :: " + entry.MusicName;
                    for (int i = 0; i < entry.MusicAuthorsInner.Count; i++)
                    {
                        line +=  " :: " + entry.MusicAuthorsInner[i];
                    }
                }
                output.WriteLine(line);
            }
            output.Flush();
            return (output.BaseStream as MemoryStream).ToArray();
        }

        public Form GetDialog()
        {
            UI.MusicSlotViewer musicSlotViewer = new UI.MusicSlotViewer();
            musicSlotViewer.MusicConfig = this;
            return musicSlotViewer;
        }

        public string GetSaveDefaultFileFilter()
        {
            return new MCFIdentifier().GetFileFilter();
        }

        public class MCFIdentifier : FileFormatIdentifier
        {
            public override string GetCategory()
            {
                return "CTGP-7";
            }

            public override string GetFileDescription()
            {
                return "Music Slots Config (INI)";
            }

            public override string GetFileFilter()
            {
                return "Music Slots Config (*.ini)|*.ini";
            }

            public override Bitmap GetIcon()
            {
                return new Bitmap(64, 64);
            }

            public override FormatMatch IsFormat(EFEFile File)
            {
                if (File.Data.Length < 9) return FormatMatch.No;
                byte[] signature = new byte[9];
                Array.Copy(File.Data, signature, 9);
                try
                {
                    string data = System.Text.Encoding.UTF8.GetString(signature);
                    return data.StartsWith("#$MSC") ? FormatMatch.Content : FormatMatch.No;
                } catch (Exception)
                {
                    return FormatMatch.No;
                }                
            }
        }
    }
}
