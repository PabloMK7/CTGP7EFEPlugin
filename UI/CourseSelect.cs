using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTGP7.UI
{
    public partial class CourseSelect : UserControl
    {
        private CTGP7CourseList CourseList;
        private Dictionary<CTGP7CourseList.NameEntry.CourseType, List<CTGP7CourseList.NameEntry>> NameEntries;
        private CTGP7CourseList.NameEntry.CourseType InUseType;
        public CTGP7CourseList.NameEntry.CourseType[] AllowedTypes { get; set; } = null;
        [Browsable(true)]
        [Description("Invoked when the selected course changes")]
        public event EventHandler SelectedTrackChanged;
        private bool PreventTrackEventOnSet = false;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CTGP7CourseList.NameEntry SelectedCourseNameEntry
        {
            get
            {
                if (InUseType == CTGP7CourseList.NameEntry.CourseType.Unknown)
                {
                    return new CTGP7CourseList.NameEntry() { courseID = 0, courseType = CTGP7CourseList.NameEntry.CourseType.Unknown, humanName = trackNameBox.Text, szsName = trackNameBox.Text };
                }
                if (trackNameBox.Items.Count == 0 || trackNameBox.SelectedIndex == -1) return null;
                return CourseList.NameEntryFromHumanName(trackNameBox.Items[trackNameBox.SelectedIndex] as string);
            }
            set
            {
                CTGP7CourseList.NameEntry entry;
                if (value == null) entry = new CTGP7CourseList.NameEntry() { courseID = 0, courseType = CTGP7CourseList.NameEntry.CourseType.Unknown, humanName = "", szsName = "" };
                else entry = CourseList.NameEntryFromSzsName(value.szsName);
                if (entry != null)
                {
                    PreventTrackEventOnSet = true;
                    trackTypeBox.SelectedItem = CTGP7CourseList.NameEntry.HumanCourseType(entry.courseType);
                    if (entry.courseType == CTGP7CourseList.NameEntry.CourseType.Unknown)
                        trackNameBox.Text = entry.szsName;
                    else 
                        trackNameBox.SelectedItem = entry.humanName;
                    PreventTrackEventOnSet = false;
                }
            }
        }
        public CourseSelect()
        {
            InitializeComponent();
            CourseList = CTGP7CourseList.Instance;
            NameEntries = new Dictionary<CTGP7CourseList.NameEntry.CourseType, List<CTGP7CourseList.NameEntry>>();
            NameEntries.Add(CTGP7CourseList.NameEntry.CourseType.OriginalRace, new List<CTGP7CourseList.NameEntry>());
            NameEntries.Add(CTGP7CourseList.NameEntry.CourseType.CustomRace, new List<CTGP7CourseList.NameEntry>());
            NameEntries.Add(CTGP7CourseList.NameEntry.CourseType.OriginalBattle, new List<CTGP7CourseList.NameEntry>());
        }

        public void PopulateCourseBox()
        {
            trackNameBox.Items.Clear();
            if (InUseType != CTGP7CourseList.NameEntry.CourseType.Unknown)
            {
                foreach (var entry in NameEntries[InUseType])
                {
                    trackNameBox.Items.Add(entry.humanName);
                }
                if (NameEntries[InUseType].Count > 0)
                    trackNameBox.SelectedIndex = 0;
                else
                    trackNameBox.SelectedIndex = -1;
            }
            else
            {
                trackNameBox.SelectedIndex = -1;
                trackNameBox.Text = "";
            }
            
        }

        private void trackNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!PreventTrackEventOnSet)
                SelectedTrackChanged?.Invoke(this, new EventArgs());
        }

        private void trackTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InUseType = CTGP7CourseList.NameEntry.EnumCourseType(trackTypeBox.Items[trackTypeBox.SelectedIndex] as string);
            if (InUseType == CTGP7CourseList.NameEntry.CourseType.Unknown)
            {
                trackNameBox.DropDownStyle = ComboBoxStyle.Simple;
            }
            else trackNameBox.DropDownStyle = ComboBoxStyle.DropDownList;
            PopulateCourseBox();
        }

        private void CourseSelect_Load(object sender, EventArgs e)
        {
            if (AllowedTypes == null || AllowedTypes.Length == 0) AllowedTypes = new CTGP7CourseList.NameEntry.CourseType[1] { CTGP7CourseList.NameEntry.CourseType.OriginalRace };
            foreach (var type in AllowedTypes)
            {
                trackTypeBox.Items.Add(CTGP7CourseList.NameEntry.HumanCourseType(type));
            }
            foreach (var entry in CourseList.Info.Entry)
            {
                NameEntries[entry.courseType].Add(entry);
            }
            InUseType = CTGP7CourseList.NameEntry.CourseType.OriginalRace;
            trackTypeBox.SelectedIndex = 0;
        }

        private void trackNameBox_TextChanged(object sender, EventArgs e)
        {
            trackNameBox_SelectedIndexChanged(sender, e);
        }
    }
}
