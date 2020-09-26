using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibEveryFileExplorer.Files;

namespace CTGP7
{
    public class CMSN : FileFormat<CMSN.CMSNIdentifier>, IViewable
    {
		public CMSN(byte[] Data)
        {

        }
		public Form GetDialog()
        {
			return new UI.CMSNViewer(this);
        }
		public class CMSNIdentifier : FileFormatIdentifier
		{
			public override string GetCategory()
			{
				return "CTGP-7";
			}

			public override string GetFileDescription()
			{
				return "Custom Mission (CMSN)";
			}

			public override string GetFileFilter()
			{
				return "Custom Mission (*.cmsn)|*.cmsn";
			}

			public override Bitmap GetIcon()
			{
				return Resources.missionIcon;
			}

			public override FormatMatch IsFormat(EFEFile File)
			{
				if (File.Data.Length > 4 && File.Data[0] == 'C' && File.Data[1] == 'M' && File.Data[2] == 'S' && File.Data[3] == 'N') return FormatMatch.Content;
				return FormatMatch.No;
			}
		}
	}
}
