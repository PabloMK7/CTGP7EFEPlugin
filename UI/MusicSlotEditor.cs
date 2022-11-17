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
    public partial class MusicSlotViewer : Form
    {
        public CTGP7CourseList TranslateList;
        public MusicConfigFile MusicConfig;
        public MusicSlotViewer()
        {
            InitializeComponent();
        }
        public bool dataIsChanging = false;
        public void updateNameLabel()
        {
            if (musicFileBox.Text == "") nameInfo.Text = "";
            else nameInfo.Text = "Normal: STRM_C" + musicFileBox.Text + "_N.bcstm; Fast: STRM_C" + musicFileBox.Text + "_F.bcstm";
        }
        public void fieldChanged()
        {

            if (dataGrid.SelectedRows.Count == 0) return;
            if (dataGrid.SelectedRows[0].Index == -1) return;
            if (dataIsChanging) return;
            dataIsChanging = true;
            MusicConfigEntry currEntry = MusicConfig.Entries[dataGrid.SelectedRows[0].Index];
            currEntry.szsNameInner = courseSelector.SelectedCourseNameEntry.szsName;
            currEntry.MusicFileName = musicFileBox.Text;
            updateNameLabel();
            currEntry.MusicMode = musicModeBox.Text;
            currEntry.NormalBPM = bpmNormalBox.Text;
            currEntry.FastBPM = bpmFastBox.Text;
            currEntry.NormalOffset = offsetNormalBox.Text;
            currEntry.FastOffset = offsetFastBox.Text;
            currEntry.MusicName = musicNameBox.Text;
            currEntry.MusicAuthors = musicAuthorBox.Text;
            dataGrid.DataSource = MusicConfig.Entries;
            dataIsChanging = false;
            dataGrid.Refresh();
        }
        public void rowChanged()
        {
            if (dataGrid.SelectedRows.Count == 0 || dataGrid.SelectedRows[0].Index == -1) { DisableInputs(true); return; }
            if (dataIsChanging) return;
            dataIsChanging = true;
            DisableInputs(false);
            MusicConfigEntry currEntry = MusicConfig.Entries[dataGrid.SelectedRows[0].Index];
            CTGP7CourseList.NameEntry entry = TranslateList.NameEntryFromSzsName(currEntry.szsNameInner);
            courseSelector.SelectedCourseNameEntry = entry;
            musicFileBox.Text = currEntry.MusicFileName;
            updateNameLabel();
            musicModeBox.Text = currEntry.MusicMode;
            bpmNormalBox.Text = currEntry.NormalBPM;
            bpmFastBox.Text = currEntry.FastBPM;
            offsetNormalBox.Text = currEntry.NormalOffset;
            offsetFastBox.Text = currEntry.FastOffset;
            musicNameBox.Text = currEntry.MusicName;
            musicAuthorBox.Text = currEntry.MusicAuthors;
            dataGrid.DataSource = MusicConfig.Entries;
            dataIsChanging = false;
        }

        private void DataGrid_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            rowChanged();
        }

        private void MultipleControl_SelectionChanged(object sender, EventArgs e)
        {
            fieldChanged();
        }

        private void DisableInputs(bool forceDisable)
        {
            bool disable = MusicConfig.Entries.Count == 0 || forceDisable;
            courseSelector.Enabled = !disable;
            musicFileBox.Enabled = !disable;
            musicModeBox.Enabled = !disable;
            bpmNormalBox.Enabled = !disable;
            bpmFastBox.Enabled = !disable;
            offsetNormalBox.Enabled = !disable;
            offsetFastBox.Enabled = !disable;
            deleteButton.Enabled = !disable;
            musicNameBox.Enabled = !disable;
            musicAuthorBox.Enabled = !disable;
            if (disable)
            {
                musicFileBox.Text = "";
                musicModeBox.SelectedIndex = -1;
                bpmNormalBox.Text = "";
                bpmFastBox.Text = "";
                offsetNormalBox.Text = "";
                offsetFastBox.Text = "";
                musicNameBox.Text = "";
                musicAuthorBox.Text = "";
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            MusicConfig.Entries.Add(new MusicConfigEntry(TranslateList));
            dataGrid.DataSource = null;
            dataGrid.DataSource = MusicConfig.Entries;
            DisableInputs(false);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count == 0 || dataGrid.SelectedRows[0].Index == -1) { return; }
            MusicConfigEntry currEntry = MusicConfig.Entries[dataGrid.SelectedRows[0].Index];
            MusicConfig.Entries.Remove(currEntry);
            dataGrid.DataSource = null;
            dataGrid.DataSource = MusicConfig.Entries;
            DisableInputs(false);
        }

        private void MusicSlotViewer_Load(object sender, EventArgs e)
        {
            DisableInputs(true);
            TranslateList = MusicConfig.TranslateList;
            musicModeBox.Items.AddRange(new string[] { "Single Channel", "Multi Channel Water", "Multi Channel Area" });
            dataGrid.DataSource = MusicConfig.Entries;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ctgp7.page.link/MusicSlotConfig");
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            CTGP7CourseList.DownloadFromInternet(false);
        }
    }
}
