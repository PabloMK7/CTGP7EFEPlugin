using CTGP7.Common;
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
    public partial class TimeSelector : UserControl
    {
        public TimeSelector()
        {
            InitializeComponent();
        }

        public MK7Timer Time
        {
            get
            {
                return new MK7Timer((uint)minuteUpDown.Value, (uint)secondsUpDown.Value, (uint)mSecondsUpDown.Value);
            }
            set
            {
                minuteUpDown.Value = value.Minutes;
                secondsUpDown.Value = value.Seconds;
                mSecondsUpDown.Value = value.Milliseconds;
            }
        }
    }
}
