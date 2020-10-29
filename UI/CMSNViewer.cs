using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTGP7.UI
{
    public partial class CMSNViewer : Form
    {
        public CMSN MissionData;
        private PartPreview[][] DriverPartPreviews;
        CTGP7CourseList TranslateTable;
        private List<string> ItemNames;
        public CMSNViewer(CMSN cmsn)
        {
            MissionData = cmsn;
            TranslateTable = CTGP7CourseList.Instance;
            InitializeComponent();
            DriverPartPreviews = new PartPreview[][] {
                new PartPreview[] {playerDriver, playerBody, playerTire, playerGlider},
                new PartPreview[] {cpu1Driver, cpu1Body, cpu1Tire, cpu1Glider},
                new PartPreview[] {cpu2Driver, cpu2Body, cpu2Tire, cpu2Glider},
                new PartPreview[] {cpu3Driver, cpu3Body, cpu3Tire, cpu3Glider},
                new PartPreview[] {cpu4Driver, cpu4Body, cpu4Tire, cpu4Glider},
                new PartPreview[] {cpu5Driver, cpu5Body, cpu5Tire, cpu5Glider},
                new PartPreview[] {cpu6Driver, cpu6Body, cpu6Tire, cpu6Glider},
                new PartPreview[] {cpu7Driver, cpu7Body, cpu7Tire, cpu7Glider},
            };
            ItemNames = new List<string>() {
                "Banana", "Green Shell", "Red Shell", "Mushroom",
                "Bob-omb", "Blooper", "Blue Shell", "Triple Mushrooms",
                "Star", "Bullet Bill", "Lightning", "Golden Mushroom",
                "Fire Flower", "Super Leaf", "Lucky Seven", "Test3",
                "Test4", "Triple Banana", "Triple Green Shells", "Triple Red Shells"
            };
        }
        
        public void LoadData()
        {

            { // Driver options
                CMSN.DriverOptionsSection driverOpts = (CMSN.DriverOptionsSection)MissionData.GetSection(CMSN.BaseSection.SectionType.DriverOptions);
                if (driverOpts != null)
                {
                    DriverAmount = driverOpts.DriverAmount;
                    for (uint i = 0; i < DriverAmount; i++)
                    {
                        for (uint j = 0; j < 4; j++)
                        {
                            SetPartsSelection(i, j, driverOpts.DriverChoices[i][j]);
                        }
                    }
                }
            }
            { // Mission Flags

                CMSN.MissionFlagsSection missionFlags = (CMSN.MissionFlagsSection)MissionData.GetSection(CMSN.BaseSection.SectionType.MissionFlags);
                CTGP7CourseList.NameEntry nameEntry = TranslateTable.NameEntryFromID(missionFlags.CourseID);
                if (nameEntry == null) nameEntry = TranslateTable.NameEntryFromID(0);
                if (nameEntry == null) throw new InvalidOperationException("CTGP-7 course name data needs to be downloaded.");
                courseSelector.SelectedCourseNameEntry = nameEntry;
                ccComboBox.SelectedIndex = missionFlags.Class;
                cpuComboBox.SelectedIndex = missionFlags.CPUDifficulty - 1;
            }
            { // Item Options
                CMSN.ItemOptionsSection itemOptions = (CMSN.ItemOptionsSection)MissionData.GetSection(CMSN.BaseSection.SectionType.ItemOptions);
                itemsModeBox.SelectedIndex = (int)itemOptions.Mode;
                spawnBoxCheck.Checked = itemOptions.SpawnItemBoxes;
                { // Player Config
                    probabilityPlayerViewer.PopulateData(GetProbabilityTableRows(itemOptions.PlayerConfig.ConfigMode, true), ItemNames, itemOptions.PlayerConfig.Probabilities);
                    modePlayerBox.SelectedIndex = (int)itemOptions.PlayerConfig.ConfigMode;
                    giveItemPlayerCheck.Checked = itemOptions.PlayerConfig.GiveItemEach.Frames != 0;
                    if (giveItemPlayerCheck.Checked)
                    {
                        giveAfterPlayerNum.Value = (decimal)itemOptions.PlayerConfig.GiveItemOffset.TotalSeconds;
                        giveEachPlayerNum.Value = (decimal)itemOptions.PlayerConfig.GiveItemEach.TotalSeconds;
                        giveIDPlayerNum.Value = itemOptions.PlayerConfig.GiveItemID;
                    }
                    roulettePlayerCheck.Checked = itemOptions.PlayerConfig.RouletteSpeed.Frames != 0;
                    if (roulettePlayerCheck.Checked)
                    {
                        roulettePlayerNum.Value = (decimal)itemOptions.PlayerConfig.RouletteSpeed.TotalSeconds;
                    }
                }
                { // CPU Config
                    probabilityCPUViewer.PopulateData(GetProbabilityTableRows(itemOptions.CPUConfig.ConfigMode, false), ItemNames, itemOptions.CPUConfig.Probabilities);
                    modeCPUBox.SelectedIndex = (int)itemOptions.CPUConfig.ConfigMode;
                    giveItemCPUCheck.Checked = itemOptions.CPUConfig.GiveItemEach.Frames != 0;
                    if (giveItemCPUCheck.Checked)
                    {
                        giveAfterCPUNum.Value = (decimal)itemOptions.CPUConfig.GiveItemOffset.TotalSeconds;
                        giveEachCPUNum.Value = (decimal)itemOptions.CPUConfig.GiveItemEach.TotalSeconds;
                        giveIDCPUNum.Value = itemOptions.CPUConfig.GiveItemID;
                    }
                    rouletteCPUCheck.Checked = itemOptions.CPUConfig.RouletteSpeed.Frames != 0;
                    if (rouletteCPUCheck.Checked)
                    {
                        rouletteCPUNum.Value = (decimal)itemOptions.CPUConfig.RouletteSpeed.TotalSeconds;
                    }
                }
            }
        }
        public void SaveData()
        {
            { // Driver options
                CMSN.DriverOptionsSection driverOpts = (CMSN.DriverOptionsSection)MissionData.GetSection(CMSN.BaseSection.SectionType.DriverOptions);
                if (driverOpts != null)
                {
                    driverOpts.DriverAmount = DriverAmount;
                    for (uint i = 0; i < DriverAmount; i++)
                    {
                        for (uint j = 0; j < 4; j++)
                        {
                            driverOpts.DriverChoices[i][j] = GetPartsSelection(i, j);
                        }
                    }
                }
            }
            { // Mission Flags
                CMSN.MissionFlagsSection missionFlags = (CMSN.MissionFlagsSection)MissionData.GetSection(CMSN.BaseSection.SectionType.MissionFlags);
                CTGP7CourseList.NameEntry nameEntry = courseSelector.SelectedCourseNameEntry;
                if (nameEntry == null) missionFlags.CourseID = 0;
                else missionFlags.CourseID = nameEntry.courseID;
                missionFlags.Class = (byte)ccComboBox.SelectedIndex;
                missionFlags.CPUDifficulty = (byte)(cpuComboBox.SelectedIndex + 1);
            }
            { // Item Options
                CMSN.ItemOptionsSection itemOptions = (CMSN.ItemOptionsSection)MissionData.GetSection(CMSN.BaseSection.SectionType.ItemOptions);
                itemOptions.Mode = (CMSN.ItemOptionsSection.ItemMode)itemsModeBox.SelectedIndex;
                itemOptions.SpawnItemBoxes = spawnBoxCheck.Checked;
                if (itemOptions.Mode == CMSN.ItemOptionsSection.ItemMode.Custom)
                {
                    { // Player config
                        itemOptions.PlayerConfig.ConfigMode = (CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode)modePlayerBox.SelectedIndex;
                        if (giveItemPlayerCheck.Checked)
                        {
                            itemOptions.PlayerConfig.GiveItemOffset = new Common.MK7Timer((double)giveAfterPlayerNum.Value);
                            itemOptions.PlayerConfig.GiveItemEach = new Common.MK7Timer((double)giveEachPlayerNum.Value);
                            itemOptions.PlayerConfig.GiveItemID = (byte)giveIDPlayerNum.Value;
                        } else
                        {
                            itemOptions.PlayerConfig.GiveItemOffset = new Common.MK7Timer(0);
                            itemOptions.PlayerConfig.GiveItemEach = new Common.MK7Timer(0);
                            itemOptions.PlayerConfig.GiveItemID = 0;
                        }
                        if (roulettePlayerCheck.Checked)
                        {
                            itemOptions.PlayerConfig.RouletteSpeed = new Common.MK7Timer((double)roulettePlayerNum.Value);
                        } else
                        {
                            itemOptions.PlayerConfig.RouletteSpeed = new Common.MK7Timer(0);
                        }
                        itemOptions.PlayerConfig.Probabilities = probabilityPlayerViewer.Values;
                    }
                    { // CPU config
                        itemOptions.CPUConfig.ConfigMode = (CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode)modeCPUBox.SelectedIndex;
                        if (giveItemCPUCheck.Checked)
                        {
                            itemOptions.CPUConfig.GiveItemOffset = new Common.MK7Timer((double)giveAfterCPUNum.Value);
                            itemOptions.CPUConfig.GiveItemEach = new Common.MK7Timer((double)giveEachCPUNum.Value);
                            itemOptions.CPUConfig.GiveItemID = (byte)giveIDCPUNum.Value;
                        }
                        else
                        {
                            itemOptions.CPUConfig.GiveItemOffset = new Common.MK7Timer(0);
                            itemOptions.CPUConfig.GiveItemEach = new Common.MK7Timer(0);
                            itemOptions.CPUConfig.GiveItemID = 0;
                        }
                        if (rouletteCPUCheck.Checked)
                        {
                            itemOptions.CPUConfig.RouletteSpeed = new Common.MK7Timer((double)rouletteCPUNum.Value);
                        }
                        else
                        {
                            itemOptions.CPUConfig.RouletteSpeed = new Common.MK7Timer(0);
                        }
                        itemOptions.CPUConfig.Probabilities = probabilityCPUViewer.Values;
                    }
                }
            }
        }
        public int DriverAmount
        {
            get
            {
                return (int)driverAmount.Value;
            }
            set
            {
                driverAmount.Value = value;
            }
        }
        public int GetPartsSelection(uint slot, uint option)
        {
            if (slot >= DriverAmount || option > 3) throw new ArgumentOutOfRangeException();
            return DriverPartPreviews[slot][option].Selection;
        }
        public void SetPartsSelection(uint slot, uint option, int value)
        {
            if (slot >= DriverAmount || option > 3) throw new ArgumentOutOfRangeException();
            DriverPartPreviews[slot][option].Selection = value;
        }
        public void UpdateDriverEnable()
        {
            for (int i = 0; i < DriverPartPreviews.Length; i++)
            {
                for (int j = 0; j < DriverPartPreviews[i].Length; j++)
                {
                    DriverPartPreviews[i][j].Enabled = (i < DriverAmount) && (j == 0 || DriverPartPreviews[i][0].Selection != -2);
                    DriverPartPreviews[i][j].Visible = (i < DriverAmount) && (j == 0 || DriverPartPreviews[i][0].Selection != -2);
                }
            }
        }
        private void driverAmount_ValueChanged(object sender, EventArgs e)
        {
            UpdateDriverEnable();
        }

        private void CMSNViewer_Load(object sender, EventArgs e)
        {
            LoadData();
            UpdateDriverEnable();
            for (int i = 0; i < DriverPartPreviews.Length; i++)
            {
                for (int j = 0; j < DriverPartPreviews[i].Length; j++)
                {
                    DriverPartPreviews[i][j].ParentViewer = this;
                }
            }
        }

        private void itemsModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            splitContainer1.Enabled = itemsModeBox.SelectedIndex == (int)CMSN.ItemOptionsSection.ItemMode.Custom;
            spawnBoxCheck.Enabled = itemsModeBox.SelectedIndex == (int)CMSN.ItemOptionsSection.ItemMode.Custom;
        }

        private void giveItemPlayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            giveAfterPlayerNum.Enabled = giveEachPlayerNum.Enabled = giveItemPlayerCheck.Checked;
            giveIDPlayerNum.Enabled = modePlayerBox.SelectedIndex == (int)CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode.BoxID && giveItemPlayerCheck.Checked;
        }

        private void roulettePlayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            roulettePlayerNum.Enabled = roulettePlayerCheck.Checked;
        }

        private void giveItemCPUCheck_CheckedChanged(object sender, EventArgs e)
        {
            giveAfterCPUNum.Enabled = giveEachCPUNum.Enabled = giveItemCPUCheck.Checked;
            giveIDCPUNum.Enabled = modeCPUBox.SelectedIndex == (int)CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode.BoxID && giveItemCPUCheck.Checked;
        }

        private void rouletteCPUCheck_CheckedChanged(object sender, EventArgs e)
        {
            rouletteCPUNum.Enabled = rouletteCPUCheck.Checked;
        }

        private List<string> GetProbabilityTableRows(CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode configMode, bool isPlayer)
        {
            List<string> tableRowNames = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                bool isizero = i == 0;
                if (isPlayer) isizero = !isizero;

                if (isizero && configMode == CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode.DriverID) tableRowNames.Add("(Unused)");
                else
                {
                    tableRowNames.Add(String.Format("{0}", i));
                }
            }
            return tableRowNames;
        }

        private void modePlayerBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            giveIDPlayerNum.Enabled = modePlayerBox.SelectedIndex == (int)CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode.BoxID && giveItemPlayerCheck.Checked;
            probabilityPlayerViewer.SetRowNames(GetProbabilityTableRows((CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode)modePlayerBox.SelectedIndex, true));
        }
        private void modeCPUBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            giveIDCPUNum.Enabled = modeCPUBox.SelectedIndex == (int)CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode.BoxID && giveItemCPUCheck.Checked;
            probabilityCPUViewer.SetRowNames(GetProbabilityTableRows((CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode)modeCPUBox.SelectedIndex, false));
        }
    }
}
