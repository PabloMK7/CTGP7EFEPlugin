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
        public CMSNViewer(CMSN cmsn)
        {
            MissionData = cmsn;
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
        }
        public void LoadData()
        {
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
        public void SaveData()
        {
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
            return DriverPartPreviews[slot][option].GetSelection();
        }
        public void SetPartsSelection(uint slot, uint option, int value)
        {
            if (slot >= DriverAmount || option > 3) throw new ArgumentOutOfRangeException();
            DriverPartPreviews[slot][option].SetSelection(value);
        }
        private void UpdateDriverEnable()
        {
            for (int i = 0; i < DriverPartPreviews.Length; i++)
            {
                for (int j = 0; j < DriverPartPreviews[i].Length; j++)
                {
                    DriverPartPreviews[i][j].Enabled = i < DriverAmount;
                    DriverPartPreviews[i][j].Visible = i < DriverAmount;
                }
            }
        }
        private void driverAmount_ValueChanged(object sender, EventArgs e)
        {
            UpdateDriverEnable();
        }

        private void CMSNViewer_Load(object sender, EventArgs e)
        {
            UpdateDriverEnable();
        }
    }
}
