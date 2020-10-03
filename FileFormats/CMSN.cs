using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibEveryFileExplorer.Files;
using LibEveryFileExplorer.IO;
using LibEveryFileExplorer.IO.Serialization;

namespace CTGP7
{
	public class CMSN : FileFormat<CMSN.CMSNIdentifier>, IViewable, IEmptyCreatable, IWriteable
	{
		static private readonly UInt16 FILE_VERSION = 1;
		public CMSNHeader Header;
		public List<BaseSection> Sections;
		public UI.CMSNViewer Viewer = null;
		public CMSN()
		{
			Header = new CMSNHeader();
			Sections = new List<BaseSection>();
			Sections.Add(new DriverOptionsSection());
			Sections.Add(new MissionFlagsSection());
		}
		public CMSN(byte[] Data)
		{
			Sections = new List<BaseSection>();
			EndianBinaryReaderEx er = new EndianBinaryReaderEx(new MemoryStream(Data), Endianness.LittleEndian);
			Header = new CMSNHeader(er);
			er.BaseStream.Position = Header.SectionTableOffset;
			uint sections = er.ReadUInt32();
			for (int i = 0; i < sections; i++)
            {
				uint sectionType = er.ReadUInt32();
				uint sectionOffset = er.ReadUInt32();
				long prevPos = er.BaseStream.Position;
				er.BaseStream.Position = sectionOffset + Header.SectionsOffset;
				switch ((BaseSection.SectionType)sectionType)
                {
					case BaseSection.SectionType.DriverOptions:
						Sections.Add(new DriverOptionsSection(er));
						break;
					case BaseSection.SectionType.MissionFlags:
						Sections.Add(new MissionFlagsSection(er));
						break;
					default:
						throw new NotImplementedException("Section " + sectionType + " not implemented!");
                }
				er.BaseStream.Position = prevPos;
            }
		}
		public byte[] Write()
		{
			if (Viewer != null) Viewer.SaveData();

			EndianBinaryWriterEx sectionWritter = new EndianBinaryWriterEx(new MemoryStream(), Endianness.LittleEndian);
			List<Tuple<BaseSection.SectionType, UInt32>> sectionOffsets = new List<Tuple<BaseSection.SectionType, UInt32>>();
			foreach (var section in Sections)
            {
				sectionOffsets.Add(Tuple.Create(section.GetSectionType(), (UInt32)sectionWritter.BaseStream.Position));
				section.Write(sectionWritter);
            }

			EndianBinaryWriterEx sectionTableWritter = new EndianBinaryWriterEx(new MemoryStream(), Endianness.LittleEndian);
			sectionTableWritter.Write((UInt32)Sections.Count);
			foreach (var section in sectionOffsets)
            {
				sectionTableWritter.Write((UInt32)section.Item1);
				sectionTableWritter.Write((UInt32)section.Item2);
			}

			Header.FileSize = (uint)(CMSNHeader.HeaderSize + sectionTableWritter.BaseStream.Length + sectionWritter.BaseStream.Length);
			Header.SectionTableOffset = (UInt16)CMSNHeader.HeaderSize;
			Header.SectionsOffset = (uint)(Header.SectionTableOffset + sectionTableWritter.BaseStream.Length);

			EndianBinaryWriterEx ew = new EndianBinaryWriterEx(new MemoryStream(), Endianness.LittleEndian);
			Header.Write(ew);
			ew.Write((sectionTableWritter.BaseStream as MemoryStream).ToArray(), 0, (int)sectionTableWritter.BaseStream.Length);
			ew.Write((sectionWritter.BaseStream as MemoryStream).ToArray(), 0, (int)sectionWritter.BaseStream.Length);

			return (ew.BaseStream as MemoryStream).ToArray();
		}

		public class CMSNHeader {
			public static readonly uint HeaderSize = 0x10;

			[BinaryStringSignature("CMSN")]
			[BinaryFixedSize(4)]
			public String Signature;
			public UInt32 FileSize;
			public UInt16 Version;
			public UInt16 SectionTableOffset;
			public UInt32 SectionsOffset;
			public CMSNHeader()
            {
				Signature = "CMSN";
				Version = FILE_VERSION;
            }
			public CMSNHeader(EndianBinaryReaderEx er)
            {
				er.ReadObject(this);
            }
			public void Write(EndianBinaryWriterEx ew)
            {
				ew.Write(Signature, Encoding.ASCII, false);
				ew.Write(FileSize);
				ew.Write(FILE_VERSION);
				ew.Write(SectionTableOffset);
				ew.Write(SectionsOffset);
			}
		}
		public BaseSection GetSection(BaseSection.SectionType sectionType)
        {
			for (int i = 0; i < Sections.Count; i++)
            {
				if (Sections[i].GetSectionType() == sectionType) return Sections[i];
            }
			return null;
        } 
		public abstract class BaseSection
        {
			public enum SectionType
            {
				DriverOptions = 0,
				TimingOptions = 1,
				MissionFlags = 2,
            }
			public abstract SectionType GetSectionType();
			public abstract void Write(EndianBinaryWriterEx ew);
        }

		public class DriverOptionsSection : BaseSection {
			public override SectionType GetSectionType()
            {
				return SectionType.DriverOptions;
            }
			public int DriverAmount;
			public int[][] DriverChoices;
			public DriverOptionsSection()
            {
				DriverAmount = 1;
				DriverChoices = new int[7][];
				for (int i = 0; i < DriverChoices.Length; i++)
                {
					DriverChoices[i] = new int[4];
					for (int j = 0; j < DriverChoices[i].Length; j++) DriverChoices[i][j] = -1;
                }
            }
			public DriverOptionsSection(EndianBinaryReaderEx er) : this()
            {
				DriverAmount = (int)er.ReadUInt32();
				for (uint i = 0; i < DriverAmount; i++)
                {
					for (uint j = 0; j < 4; j++)
					{
						DriverChoices[i][j] = er.ReadInt32();
					}
                }
            }
			public override void Write(EndianBinaryWriterEx ew)
            {
				ew.Write((UInt32)DriverAmount);
				for (uint i = 0; i < DriverAmount; i++)
				{
					for (uint j = 0; j < 4; j++)
					{
						ew.Write(DriverChoices[i][j]);
					}
				}
			}
        }

		public class TimingsSection : BaseSection
		{
			public override SectionType GetSectionType()
			{
				return SectionType.TimingOptions;
			}
			public TimingsSection()
			{
				
			}
			public TimingsSection(EndianBinaryReaderEx er) : this()
			{
				
			}
			public override void Write(EndianBinaryWriterEx ew)
			{
				
			}
		}

		public class MissionFlagsSection : BaseSection
		{
			public override SectionType GetSectionType()
			{
				return SectionType.MissionFlags;
			}
			public UInt32 CourseID;
			public MissionFlagsSection()
			{
				CourseID = 0;
			}
			public MissionFlagsSection(EndianBinaryReaderEx er) : this()
			{
				CourseID = er.ReadUInt32();
			}
			public override void Write(EndianBinaryWriterEx ew)
			{
				ew.Write(CourseID);
			}
		}

		public Form GetDialog()
        {
			Viewer = new UI.CMSNViewer(this);
			return Viewer;
        }

        public string GetSaveDefaultFileFilter()
        {
			return new CMSNIdentifier().GetFileFilter();
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
