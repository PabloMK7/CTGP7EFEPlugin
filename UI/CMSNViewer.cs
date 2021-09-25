using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private List<List<string>> MissionSubtypes;
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
            MissionSubtypes = new List<List<string>>()
            {
                new List<string>() {"None" },
                new List<string>() {"Order", "All"},
                new List<string>() {"ItemBox", "Coin", "Rocky Wrenches"},
                new List<string>() {"Boost Pad", "Mini-Turbo", "Super Mini-Turbo", "Rocket Start", "Trick", "Improved Trick", "Star Ring"},
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
                missionTypeCombo.SelectedIndex = missionFlags.MissionType;
                missionSubTypeCombo.SelectedIndex = missionFlags.MissionSubType;
                calculationCombo.SelectedIndex = missionFlags.CalculationType;
                initialTimerSelector.Time = missionFlags.InitialTimer;
                timerDirectionCombo.SelectedIndex = missionFlags.CountTimerUp ? 1 : 0;
                finishRaceTimerCheck.Checked = missionFlags.AutoFinishRaceTimer;
                finishRaceTimerSelector.Time = missionFlags.FinishRaceTimer;
                minGradeTimerSelector.Time = missionFlags.MinGradeTimer;
                maxGradeTimerSelector.Time = missionFlags.MaxGradeTimer;
                scoreEnabledCheck.Checked = missionFlags.UseScore;
                initialScoreUpDown.Value = missionFlags.InitialScore;
                directionScoreCombo.SelectedIndex = missionFlags.CountScoreUp ? 1 : 0;
                finishRaceScoreCheck.Checked = missionFlags.AutoFinishRaceScore;
                finishRaceScoreUpDown.Value = missionFlags.FinishRaceScore;
                minGradeScoreUpDown.Value = missionFlags.MinGradeScore;
                maxGradeScoreUpDown.Value = missionFlags.MaxGradeScore;
                scoreYellowUpDown.Value = missionFlags.YellowScore;
                lapAmountUpDown.Value = missionFlags.LapAmount;
                rankHiddenCheck.Checked = !missionFlags.RankVisible;
                lakituHiddenCheck.Checked = !missionFlags.LakituVisible;
                hideCoinsCheck.Checked = missionFlags.CoinCounterHidden;
                hideLapsCheck.Checked = !missionFlags.LapCounterVisible;
                playCourseIntroCheck.Checked = missionFlags.CourseIntroVisible;
                hideScoreCheck.Checked = missionFlags.ScoreHidden;
                scoreIsBadCheck.Checked = missionFlags.ScoreNegative;
                forceBackwardsCheck.Checked = missionFlags.ForceBackwards;
                finishRaceSectionCheck.Checked = missionFlags.FinishOnSection;
                givePointHitCheck.Checked = missionFlags.GivePointOnHit;
                coinRespawnCheck.Checked = missionFlags.RespawnCoins;
                coinRespawnTimer.Time = missionFlags.RespawnCoinsTimer;
                try
                {
                    completeCondition1Combo.SelectedIndex = missionFlags.CompleteCondition1;
                    completeCondition2Combo.SelectedIndex = missionFlags.CompleteCondition2;
                } catch (Exception e)
                {
                    completeCondition1Combo.SelectedIndex = 0;
                    completeCondition2Combo.SelectedIndex = 0;
                }

                scoreEnabledCheck_CheckedChanged(scoreEnabledCheck, new EventArgs());
                finishRaceTimerCheck_CheckedChanged(finishRaceTimerCheck, new EventArgs());
                finishRaceScoreCheck_CheckedChanged(finishRaceScoreCheck, new EventArgs());
                coinRespawnCheck_CheckedChanged(coinRespawnCheck, new EventArgs());
            }
            { // Item Options
                CMSN.ItemOptionsSection itemOptions = (CMSN.ItemOptionsSection)MissionData.GetSection(CMSN.BaseSection.SectionType.ItemOptions);
                itemsModeBox.SelectedIndex = (int)itemOptions.Mode;
                spawnBoxCheck.Checked = itemOptions.SpawnItemBoxes;
                itemBoxRespawnCheck.Checked = itemOptions.RespawnItemBox;
                itemBoxRespawnTimer.Time = itemOptions.RespawnItemBoxTimer;
                { // Player Config
                    probabilityPlayerViewer.PopulateData(GetProbabilityTableRows(itemOptions.PlayerConfig.ConfigMode, true), ItemNames, itemOptions.PlayerConfig.Probabilities);
                    modePlayerBox.SelectedIndex = (int)itemOptions.PlayerConfig.ConfigMode;
                    giveItemPlayerCheck.Checked = itemOptions.PlayerConfig.GiveItemEach.Frames != 0;
                    if (giveItemPlayerCheck.Checked)
                    {
                        giveAfterPlayerNum.Value = (decimal)itemOptions.PlayerConfig.GiveItemOffset.TotalSeconds;
                        giveEachPlayerNum.Value = (decimal)itemOptions.PlayerConfig.GiveItemEach.TotalSeconds;
                    }
                    disableCooldownPlayerCheck.Checked = itemOptions.PlayerConfig.DisableCooldown;
                    roulettePlayerCheck.Checked = itemOptions.PlayerConfig.RouletteSpeed.Frames != 0;
                    if (roulettePlayerCheck.Checked)
                    {
                        if (itemOptions.PlayerConfig.RouletteSpeed.Frames == 1)
                            roulettePlayerNum.Value = 0;
                        else
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
                    }
                    disableCooldownCPUCheck.Checked = itemOptions.CPUConfig.DisableCooldown;
                    rouletteCPUCheck.Checked = itemOptions.CPUConfig.RouletteSpeed.Frames != 0;
                    if (rouletteCPUCheck.Checked)
                    {
                        if (itemOptions.CPUConfig.RouletteSpeed.Frames == 1)
                            rouletteCPUNum.Value = 0;
                        else 
                            rouletteCPUNum.Value = (decimal)itemOptions.CPUConfig.RouletteSpeed.TotalSeconds;
                    }
                }
                itemBoxRespawnCheck_CheckedChanged(itemBoxRespawnCheck, new EventArgs());
            }
            { // Text entries
                CMSN.TextSection textSection = (CMSN.TextSection)MissionData.GetSection(CMSN.BaseSection.SectionType.TextStrings);
                foreach (var entry in textSection.entries)
                {
                    textEntryList.Items.Add(new CMSN.TextSection.LanguageEntry(entry));
                }
            }
            { // Info section
                CMSN.InfoSection infoSection = (CMSN.InfoSection)MissionData.GetSection(CMSN.BaseSection.SectionType.Info);
                missionUUIDLabel.Text = "Mission UUID: " + BitConverter.ToString(infoSection.MissionUUID);
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
                missionFlags.MissionType = (byte)missionTypeCombo.SelectedIndex;
                missionFlags.MissionSubType = (byte)missionSubTypeCombo.SelectedIndex;
                missionFlags.CalculationType = (byte)calculationCombo.SelectedIndex;
                missionFlags.InitialTimer = initialTimerSelector.Time;
                missionFlags.CountTimerUp = timerDirectionCombo.SelectedIndex == 1;
                missionFlags.AutoFinishRaceTimer = finishRaceTimerCheck.Checked;
                missionFlags.FinishRaceTimer = finishRaceTimerSelector.Time;
                missionFlags.MinGradeTimer = minGradeTimerSelector.Time;
                missionFlags.MaxGradeTimer = maxGradeTimerSelector.Time;
                missionFlags.UseScore = scoreEnabledCheck.Checked;
                missionFlags.InitialScore = (byte)initialScoreUpDown.Value;
                missionFlags.CountScoreUp = directionScoreCombo.SelectedIndex == 1;
                missionFlags.AutoFinishRaceScore = finishRaceScoreCheck.Checked;
                missionFlags.FinishRaceScore = (byte)finishRaceScoreUpDown.Value;
                missionFlags.MinGradeScore = (byte)minGradeScoreUpDown.Value;
                missionFlags.MaxGradeScore = (byte)maxGradeScoreUpDown.Value;
                missionFlags.YellowScore = (byte)scoreYellowUpDown.Value;
                missionFlags.LapAmount = (byte)lapAmountUpDown.Value;
                missionFlags.RankVisible = !rankHiddenCheck.Checked;
                missionFlags.LakituVisible = !lakituHiddenCheck.Checked;
                missionFlags.CoinCounterHidden = hideCoinsCheck.Checked;
                missionFlags.LapCounterVisible = !hideLapsCheck.Checked;
                missionFlags.CourseIntroVisible = playCourseIntroCheck.Checked;
                missionFlags.ScoreHidden = hideScoreCheck.Checked;
                missionFlags.ScoreNegative = scoreIsBadCheck.Checked;
                missionFlags.ForceBackwards = forceBackwardsCheck.Checked;
                missionFlags.FinishOnSection = finishRaceSectionCheck.Checked;
                missionFlags.GivePointOnHit = givePointHitCheck.Checked;
                missionFlags.RespawnCoins = coinRespawnCheck.Checked;
                missionFlags.RespawnCoinsTimer = coinRespawnTimer.Time;
                missionFlags.CompleteCondition1 = (byte)completeCondition1Combo.SelectedIndex;
                missionFlags.CompleteCondition2 = (byte)completeCondition2Combo.SelectedIndex;
            }
            { // Item Options
                CMSN.ItemOptionsSection itemOptions = (CMSN.ItemOptionsSection)MissionData.GetSection(CMSN.BaseSection.SectionType.ItemOptions);
                itemOptions.Mode = (CMSN.ItemOptionsSection.ItemMode)itemsModeBox.SelectedIndex;
                itemOptions.SpawnItemBoxes = spawnBoxCheck.Checked;
                itemOptions.RespawnItemBox = itemBoxRespawnCheck.Checked;
                itemOptions.RespawnItemBoxTimer = itemBoxRespawnTimer.Time;
                if (itemOptions.Mode == CMSN.ItemOptionsSection.ItemMode.Custom)
                {
                    { // Player config
                        itemOptions.PlayerConfig.ConfigMode = (CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode)modePlayerBox.SelectedIndex;
                        if (giveItemPlayerCheck.Checked)
                        {
                            itemOptions.PlayerConfig.GiveItemOffset = new Common.MK7Timer((double)giveAfterPlayerNum.Value);
                            itemOptions.PlayerConfig.GiveItemEach = new Common.MK7Timer((double)giveEachPlayerNum.Value);
                        } else
                        {
                            itemOptions.PlayerConfig.GiveItemOffset = new Common.MK7Timer(0);
                            itemOptions.PlayerConfig.GiveItemEach = new Common.MK7Timer(0);
                        }
                        if (roulettePlayerCheck.Checked)
                        {
                            if (roulettePlayerNum.Value == 0)
                            {
                                itemOptions.PlayerConfig.RouletteSpeed = new Common.MK7Timer(1);
                            }
                            else
                            {
                                itemOptions.PlayerConfig.RouletteSpeed = new Common.MK7Timer((double)roulettePlayerNum.Value);
                            }
                        } else
                        {
                            itemOptions.PlayerConfig.RouletteSpeed = new Common.MK7Timer(0);
                        }
                        itemOptions.PlayerConfig.DisableCooldown = disableCooldownPlayerCheck.Checked;
                        itemOptions.PlayerConfig.Probabilities = probabilityPlayerViewer.Values;
                    }
                    { // CPU config
                        itemOptions.CPUConfig.ConfigMode = (CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode)modeCPUBox.SelectedIndex;
                        if (giveItemCPUCheck.Checked)
                        {
                            itemOptions.CPUConfig.GiveItemOffset = new Common.MK7Timer((double)giveAfterCPUNum.Value);
                            itemOptions.CPUConfig.GiveItemEach = new Common.MK7Timer((double)giveEachCPUNum.Value);
                        }
                        else
                        {
                            itemOptions.CPUConfig.GiveItemOffset = new Common.MK7Timer(0);
                            itemOptions.CPUConfig.GiveItemEach = new Common.MK7Timer(0);
                        }
                        if (rouletteCPUCheck.Checked)
                        {
                            if (rouletteCPUNum.Value == 0)
                            {
                                itemOptions.CPUConfig.RouletteSpeed = new Common.MK7Timer(1);
                            } else
                            {
                                itemOptions.CPUConfig.RouletteSpeed = new Common.MK7Timer((double)rouletteCPUNum.Value);
                            }
                        }
                        else
                        {
                            itemOptions.CPUConfig.RouletteSpeed = new Common.MK7Timer(0);
                        }
                        itemOptions.CPUConfig.DisableCooldown = disableCooldownCPUCheck.Checked;
                        itemOptions.CPUConfig.Probabilities = probabilityCPUViewer.Values;
                    }
                }
            }
            { // Text entries
                CMSN.TextSection textSection = (CMSN.TextSection)MissionData.GetSection(CMSN.BaseSection.SectionType.TextStrings);
                textSection.entries.Clear();
                foreach (var entry in textEntryList.Items)
                {
                    textSection.entries.Add(new CMSN.TextSection.LanguageEntry(entry as CMSN.TextSection.LanguageEntry));
                }
            }
            { // Info section

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
        }

        private void roulettePlayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            roulettePlayerNum.Enabled = roulettePlayerCheck.Checked;
        }

        private void giveItemCPUCheck_CheckedChanged(object sender, EventArgs e)
        {
            giveAfterCPUNum.Enabled = giveEachCPUNum.Enabled = giveItemCPUCheck.Checked;
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
                else if (i == 0 && configMode == CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode.BoxID) tableRowNames.Add("(Unused)");
                else
                {
                    if (configMode == CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode.Rank || configMode == CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode.TrueRank)
                        tableRowNames.Add(String.Format("{0}", i + 1));
                    else
                        tableRowNames.Add(String.Format("{0}", i));
                }
            }
            return tableRowNames;
        }

        private void modePlayerBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            probabilityPlayerViewer.SetRowNames(GetProbabilityTableRows((CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode)modePlayerBox.SelectedIndex, true));
        }
        private void modeCPUBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            probabilityCPUViewer.SetRowNames(GetProbabilityTableRows((CMSN.ItemOptionsSection.ItemConfig.ItemConfigMode)modeCPUBox.SelectedIndex, false));
        }

        private void textEntryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textEntryList.SelectedIndex != -1)
            {
                langCodeText.Enabled = true;
                langTitleText.Enabled = true;
                langDescriptionText.Enabled = true;
                if (textEntryList.Items.Count > 1)
                    removeTextEntryButton.Enabled = true;

                langCodeText.Text = (textEntryList.Items[textEntryList.SelectedIndex] as CMSN.TextSection.LanguageEntry).langCode;
                langTitleText.Text = (textEntryList.Items[textEntryList.SelectedIndex] as CMSN.TextSection.LanguageEntry).title;
                langDescriptionText.RichText = (textEntryList.Items[textEntryList.SelectedIndex] as CMSN.TextSection.LanguageEntry).description;
            } else
            {
                langCodeText.Enabled = false;
                langTitleText.Enabled = false;
                langDescriptionText.Enabled = false;
                removeTextEntryButton.Enabled = false;

                langCodeText.Text = "";
                langTitleText.Text = "";
                langDescriptionText.RichText = new byte[0];
            }
        }

        private void langCodeText_TextChanged(object sender, EventArgs e)
        {
            if (textEntryList.SelectedIndex != -1)
            {
                (textEntryList.Items[textEntryList.SelectedIndex] as CMSN.TextSection.LanguageEntry).langCode = langCodeText.Text;
            }
        }

        private void langTitleText_TextChanged(object sender, EventArgs e)
        {
            if (textEntryList.SelectedIndex != -1)
                (textEntryList.Items[textEntryList.SelectedIndex] as CMSN.TextSection.LanguageEntry).title = langTitleText.Text;
        }

        private void langDescriptionText_RichTextChanged(object sender, EventArgs e)
        {
            if (textEntryList.SelectedIndex != -1)
            {
                (textEntryList.Items[textEntryList.SelectedIndex] as CMSN.TextSection.LanguageEntry).description = langDescriptionText.RichText;
                (textEntryList.Items[textEntryList.SelectedIndex] as CMSN.TextSection.LanguageEntry).descriptionNewlines = (byte)langDescriptionText.RichTextNewLines;
            }
        }

        private void addTextEntryButton_Click(object sender, EventArgs e)
        {
            textEntryList.Items.Add(new CMSN.TextSection.LanguageEntry());
        }
        private void removeTextEntryButton_Click(object sender, EventArgs e)
        {
            if (textEntryList.SelectedIndex != -1)
                textEntryList.Items.RemoveAt(textEntryList.SelectedIndex);
        }

        private void langCodeText_Validating(object sender, CancelEventArgs e)
        {
            if (textEntryList.SelectedIndex != -1)
                textEntryList.Items[textEntryList.SelectedIndex] = textEntryList.Items[textEntryList.SelectedIndex]; // Bruh
        }

        private void calculationCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (calculationCombo.SelectedIndex == 0) // Timer
            {
                minGradeTimerSelector.Enabled = true;
                maxGradeTimerSelector.Enabled = true;
                scoreEnabledCheck.Enabled = true;
                minGradeScoreUpDown.Enabled = false;
                maxGradeScoreUpDown.Enabled = false;
            } else if (calculationCombo.SelectedIndex == 1) // Score
            {
                minGradeTimerSelector.Enabled = false;
                maxGradeTimerSelector.Enabled = false;
                scoreEnabledCheck.Checked = true;
                scoreEnabledCheck.Enabled = false;
                minGradeScoreUpDown.Enabled = true;
                maxGradeScoreUpDown.Enabled = true;
            }
        }

        private void finishRaceScoreCheck_CheckedChanged(object sender, EventArgs e)
        {
            finishRaceScoreUpDown.Enabled = finishRaceScoreCheck.Checked;
        }

        private void finishRaceTimerCheck_CheckedChanged(object sender, EventArgs e)
        {
            finishRaceTimerSelector.Enabled = finishRaceTimerCheck.Checked;
        }

        private void scoreEnabledCheck_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = scoreEnabledCheck.Checked;
            initialScoreUpDown.Enabled = enable;
            directionScoreCombo.Enabled = enable;
            finishRaceScoreCheck.Enabled = enable;
            finishRaceScoreUpDown.Enabled = enable && finishRaceScoreCheck.Checked;
            minGradeScoreUpDown.Enabled = enable && calculationCombo.SelectedIndex == 1;
            maxGradeScoreUpDown.Enabled = enable && calculationCombo.SelectedIndex == 1;
            scoreYellowUpDown.Enabled = enable;
        }

        private void missionTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (missionTypeCombo.SelectedIndex == -1) return;
            missionSubTypeCombo.DataSource = null;
            missionSubTypeCombo.DataSource = MissionSubtypes[missionTypeCombo.SelectedIndex].ToArray();
            missionSubTypeCombo.SelectedIndex = 0;
        }

        private void coinRespawnCheck_CheckedChanged(object sender, EventArgs e)
        {
            coinRespawnTimer.Enabled = coinRespawnCheck.Checked;
        }

        private void itemBoxRespawnCheck_CheckedChanged(object sender, EventArgs e)
        {
            itemBoxRespawnTimer.Enabled = itemBoxRespawnCheck.Checked;
        }

        private void resetMissionSaveDataButton_Click(object sender, EventArgs e)
        {
            CMSN.InfoSection infoSection = (CMSN.InfoSection)MissionData.GetSection(CMSN.BaseSection.SectionType.Info);

            saveResetLabel.Text = "";
            DialogResult res = MessageBox.Show("Do you want to reset the save data identifier for this mission? This will cause the checksum to break and the best time stored in the console to reset.", "Reset Mission Save Data", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                saveResetLabel.Text = "Done!";
                infoSection.SaveIteration++;
            }
        }
    }
}
