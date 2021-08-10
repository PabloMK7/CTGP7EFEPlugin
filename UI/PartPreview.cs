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
    public partial class PartPreview : UserControl
    {
        private readonly Tuple<int, string, Bitmap>[] DriverData =
        {
            Tuple.Create(0, "Bowser", MenuImages.select_bowser),
            Tuple.Create(1, "Daisy", MenuImages.select_daisy),
            Tuple.Create(2, "Donkey Kong", MenuImages.select_donkey),
            Tuple.Create(3, "Honey Queen", MenuImages.select_honeyQueen),
            Tuple.Create(4, "Koopa Troopa", MenuImages.select_koopaTroopa),
            Tuple.Create(5, "Lakitu", MenuImages.select_lakitu),
            Tuple.Create(6, "Luigi", MenuImages.select_luigi),
            Tuple.Create(7, "Mario", MenuImages.select_mario),
            Tuple.Create(8, "Metal Mario", MenuImages.select_metal),
            Tuple.Create(9, "Male Mii", MenuImages.select_hatena),
            Tuple.Create(10, "Female Mii", MenuImages.select_hatena),
            Tuple.Create(11, "Peach", MenuImages.select_peach),
            Tuple.Create(12, "Rosalina", MenuImages.select_rosalina),
            Tuple.Create(14, "Toad", MenuImages.select_toad),
            Tuple.Create(15, "Wario", MenuImages.select_wario),
            Tuple.Create(16, "Wiggler", MenuImages.select_wiggler),
            Tuple.Create(17, "Yoshi", MenuImages.select_yoshi),
            Tuple.Create(100 + 0, "Shy Guy (Red)", MenuImages.select_sh_red),
            Tuple.Create(100 + 1, "Shy Guy (Blue)", MenuImages.select_sh_blue),
            Tuple.Create(100 + 2, "Shy Guy (Yellow)", MenuImages.select_sh_yellow),
            Tuple.Create(100 + 3, "Shy Guy (Green)", MenuImages.select_sh_green),
            Tuple.Create(100 + 4, "Shy Guy (Cyan)", MenuImages.select_sh_lightblue),
            Tuple.Create(100 + 5, "Shy Guy (Pink)", MenuImages.select_sh_pink),
            Tuple.Create(100 + 6, "Shy Guy (Black)", MenuImages.select_sh_black),
            Tuple.Create(100 + 7, "Shy Guy (White)", MenuImages.select_sh_white),
            Tuple.Create(-100, "Shy Guy (Random)", MenuImages.select_hatena),
            Tuple.Create(-1, "Random", MenuImages.select_hatena),
            Tuple.Create(-2, "Recommended", MenuImages.select_hatena)
        };
        private readonly Tuple<int, string, Bitmap>[] BodyData =
        {
            Tuple.Create(0, "Standard", MenuImages.b_std),
            Tuple.Create(1, "Bolt Buggy", MenuImages.b_rally),
            Tuple.Create(2, "Birthday Girl", MenuImages.b_rib),
            Tuple.Create(3, "Egg 1", MenuImages.b_egg),
            Tuple.Create(4, "B Dasher", MenuImages.b_dsh),
            Tuple.Create(5, "Zucchini", MenuImages.b_cuc),
            Tuple.Create(6, "Koopa Clown", MenuImages.b_kpc),
            Tuple.Create(7, "Tiny Tug", MenuImages.b_bot),
            Tuple.Create(8, "Bumble V", MenuImages.b_hny),
            Tuple.Create(9, "Cact-X", MenuImages.b_sab),
            Tuple.Create(10, "Bruiser", MenuImages.b_gng),
            Tuple.Create(11, "Pipe Frame", MenuImages.b_pip),
            Tuple.Create(12, "Barrel Train", MenuImages.b_trn),
            Tuple.Create(13, "Cloud 9", MenuImages.b_cld),
            Tuple.Create(14, "Blue Seven", MenuImages.b_rac),
            Tuple.Create(15, "Soda Jet", MenuImages.b_jet),
            Tuple.Create(16, "Gold Standard", MenuImages.b_gld),
            Tuple.Create(-1, "Random", MenuImages.b_q),
        };
        private readonly Tuple<int, string, Bitmap>[] TireData =
        {
            Tuple.Create(0, "Standard", MenuImages.t_std),
            Tuple.Create(1, "Monster", MenuImages.t_big),
            Tuple.Create(2, "Roller", MenuImages.t_small),
            Tuple.Create(3, "Slick", MenuImages.t_rac),
            Tuple.Create(4, "Slim", MenuImages.t_cls),
            Tuple.Create(5, "Sponge", MenuImages.t_spg),
            Tuple.Create(6, "Gold", MenuImages.t_gld),
            Tuple.Create(7, "Wood", MenuImages.t_wod),
            Tuple.Create(8, "Red Monster", MenuImages.t_red),
            Tuple.Create(9, "Mushroom", MenuImages.t_mus),
            Tuple.Create(-1, "Random", MenuImages.t_q),
        };
        private readonly Tuple<int, string, Bitmap>[] WingData =
        {
            Tuple.Create(0, "Standard", MenuImages.g_std),
            Tuple.Create(1, "Paraglider", MenuImages.g_par),
            Tuple.Create(2, "Parasol", MenuImages.g_umb),
            Tuple.Create(3, "Flower", MenuImages.g_flw),
            Tuple.Create(4, "Swooper", MenuImages.g_bas),
            Tuple.Create(5, "Beast", MenuImages.g_met),
            Tuple.Create(6, "Gold", MenuImages.g_gld),
            Tuple.Create(-1, "Random", MenuImages.g_q),
        };
        public enum PartPreviewMode
        {
            Driver,
            Body,
            Tire,
            Wing
        }
        private Tuple<int, string, Bitmap>[] UseData;
        private int UseIndex;
        private bool HasLoaded;
        private int SetIndex;
        public CMSNViewer ParentViewer;
        public int Selection
        {
            get
            {
                if (!HasLoaded) return SetIndex;
                if (UseIndex < 0 || UseData == null) return -1;
                return UseData[UseIndex].Item1;
            }
            set
            {
                if (HasLoaded) SetSelectionImpl(value);
                else SetIndex = value;
            }
        }
        public bool MainPlayer { get; set; }
        public PartPreviewMode PreviewMode { get; set; }
        public PartPreview()
        {
            HasLoaded = false;
            SetIndex = int.MinValue;
            InitializeComponent();
        }
        private void UpdateGraphic(bool updateText)
        {
            if (updateText) comboBox.SelectedIndex = UseIndex;
            if (UseIndex >= 0) picture.Image = UseData[UseIndex].Item3;
            else picture.Image = null;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UseIndex = (sender as ComboBox).SelectedIndex;
            UpdateGraphic(false);
            if (ParentViewer != null && PreviewMode == PartPreviewMode.Driver) ParentViewer.UpdateDriverEnable();
        }
        private void SetSelectionImpl(int value)
        {
            for (int i = 0; i < UseData.Length; i++)
            {
                if (value == UseData[i].Item1) { UseIndex = i; return; }
                else if (UseData[i].Item1 == -1) UseIndex = i;
            }
        }

        private void PartPreview_Load(object sender, EventArgs e)
        {
            switch (PreviewMode)
            {
                case PartPreviewMode.Driver:
                    UseData = DriverData;
                    UseIndex = 26;
                    break;
                case PartPreviewMode.Body:
                    UseData = BodyData;
                    UseIndex = 17;
                    break;
                case PartPreviewMode.Tire:
                    UseData = TireData;
                    UseIndex = 10;
                    break;
                case PartPreviewMode.Wing:
                    UseData = WingData;
                    UseIndex = 7;
                    break;
            };
            foreach (var item in UseData)
            {
                if (PreviewMode == PartPreviewMode.Driver && MainPlayer && item.Item1 == -2) continue;
                comboBox.Items.Add(item.Item2);
            }
            if (SetIndex != int.MinValue) SetSelectionImpl(SetIndex);
            HasLoaded = true;
            UpdateGraphic(true);
        }
    }
}
