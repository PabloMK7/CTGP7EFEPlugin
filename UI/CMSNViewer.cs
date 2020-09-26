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
        private void UpdateDriverEnable()
        {
            int amount = (int)driverAmount.Value;
            for (int i = 0; i < DriverPartPreviews.Length; i++)
            {
                for (int j = 0; j < DriverPartPreviews[i].Length; j++)
                {
                    DriverPartPreviews[i][j].Enabled = i < amount;
                    DriverPartPreviews[i][j].Visible = i < amount;
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
