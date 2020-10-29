using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTGP7.Common;
using LibEveryFileExplorer.Files;
using LibEveryFileExplorer.IO;
using LibEveryFileExplorer.IO.Serialization;

namespace CTGP7
{
	public class CMSN //: FileFormat<CMSN.CMSNIdentifier>, IViewable, IEmptyCreatable, IWriteable
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
			Sections.Add(new ItemOptionsSection());
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
					case BaseSection.SectionType.ItemOptions:
						Sections.Add(new ItemOptionsSection(er));
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
				ItemOptions = 3
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
				er.ReadPadding(4);
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
				ew.WritePadding(4);
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
			public byte Class;
			public byte CPUDifficulty;
			public MissionFlagsSection()
			{
				CourseID = 0;
				Class = 0;
				CPUDifficulty = 1;
			}
			public MissionFlagsSection(EndianBinaryReaderEx er) : this()
			{
				CourseID = er.ReadUInt32();
				Class = er.ReadByte();
				CPUDifficulty = er.ReadByte();
				er.ReadPadding(4);
			}
			public override void Write(EndianBinaryWriterEx ew)
			{
				ew.Write(CourseID);
				ew.Write(Class);
				ew.Write(CPUDifficulty);
				ew.WritePadding(4);
			}
		}
		
		public class ItemOptionsSection : BaseSection
        {
            public override SectionType GetSectionType()
            {
				return SectionType.ItemOptions;
            }
			public enum ItemMode
            {
				All = 0,
				Shells = 1,
				Bananas = 2,
				Mushrooms = 3,
				Bobombs = 4,
				None = 5,
				Custom = 6
			}
			public enum ItemSlot
			{
				Banana = 0,
				KouraGreen = 1,
				KouraRed = 2,
				Kinoko = 3,
				Bombhei = 4,
				Gesso = 5,
				KouraBlue = 6,
				Kinoko3 = 7,
				Star = 8,
				Killer = 9,
				Thunder = 10,
				KinokoGold = 11,
				Flower = 12,
				Konoha = 13,
				Seven = 14,
				Test3 = 15,
				Test4 = 16,
				Banana3 = 17,
				KouraGreen3 = 18,
				KouraRed3 = 19
			};
			public const int ItemAmount = 20;
			public ItemMode Mode;
			public bool SpawnItemBoxes;
			
			public class ItemConfig
            {
				public byte[][] Probabilities;

				public ItemConfigMode ConfigMode;
				public MK7Timer GiveItemOffset;
				public MK7Timer GiveItemEach;
				public byte GiveItemID;
				public MK7Timer RouletteSpeed;
				private bool IsPlayer;
				public enum ItemConfigMode
                {
					BoxID = 0,
					Rank = 1,
					DriverID = 2,
                }

				public ItemConfig(bool isPlayer)
                {
					IsPlayer = isPlayer;
					Probabilities = new byte[8][];
					for (int i = 0; i < Probabilities.Length; i++)
                    {
						Probabilities[i] = new byte[ItemAmount];
						for (int j = 0; j < Probabilities[i].Length; j++)
                        {
							Probabilities[i][j] = 0;
                        }
                    }
					ConfigMode = ItemConfigMode.Rank;
					GiveItemOffset = new MK7Timer(0);
					GiveItemEach = new MK7Timer(0);
					GiveItemID = 255;
					RouletteSpeed = new MK7Timer(0);

                }
				public ItemConfig(EndianBinaryReaderEx er, bool isPlayer) : this(isPlayer)
                {
					RouletteSpeed = new MK7Timer(er.ReadUInt16());
					GiveItemOffset = new MK7Timer(er.ReadUInt16());
					GiveItemEach = new MK7Timer(er.ReadUInt16());
					GiveItemID = er.ReadByte();
					ConfigMode = (ItemConfigMode)er.ReadByte();
					for (int i = 0; i < Probabilities.Length; i++)
                    {
						for (int j = 0; j < Probabilities[i].Length; j++)
                        {
							Probabilities[i][j] = er.ReadByte();
                        }
                    }
					er.ReadPadding(4);
                }
				public void Write(EndianBinaryWriterEx ew)
                {
					ew.Write((UInt16)RouletteSpeed.Frames);
					ew.Write((UInt16)GiveItemOffset.Frames);
					ew.Write((UInt16)GiveItemEach.Frames);
					ew.Write(GiveItemID);
					ew.Write((byte)ConfigMode);
					for (int i = 0; i < Probabilities.Length; i++)
					{
						for (int j = 0; j < Probabilities[i].Length; j++)
						{
							ew.Write(Probabilities[i][j]);
						}
					}
					ew.WritePadding(4);
				}

            }
			public ItemConfig PlayerConfig;
			public ItemConfig CPUConfig;
			public ItemOptionsSection()
            {
				Mode = ItemMode.All;
				SpawnItemBoxes = true;
				PlayerConfig = new ItemConfig(true);
				CPUConfig = new ItemConfig(false);
            }
			public ItemOptionsSection(EndianBinaryReaderEx er) : this()
            {
				long basePos = er.BaseStream.Position;
				long offsetPlayer = er.ReadUInt16();
				long offsetCPU = er.ReadUInt16();
				Mode = (ItemMode)er.ReadByte();
				SpawnItemBoxes = er.ReadByte() != 0;
				if (Mode == ItemMode.Custom)
                {
					er.BaseStream.Position = basePos + offsetPlayer;
					PlayerConfig = new ItemConfig(er, true);
					er.BaseStream.Position = basePos + offsetCPU;
					CPUConfig = new ItemConfig(er, false);
                }
				er.ReadPadding(4);
			}
            public override void Write(EndianBinaryWriterEx ew)
            {
				EndianBinaryWriterEx tempWritter = new EndianBinaryWriterEx(new MemoryStream(), Endianness.LittleEndian); // I don't know how else I can do this without a hardcoded size...
				tempWritter.Write(new ushort()); // offsetPlayer
				tempWritter.Write(new ushort()); // offsetCPU
				tempWritter.Write(new byte()); // Mode
				tempWritter.Write(new byte()); // SpawnItemBoxes
				tempWritter.WritePadding(4);

				UInt16 baseOffset = (UInt16)tempWritter.BaseStream.Position;
				EndianBinaryWriterEx sectionWritter = new EndianBinaryWriterEx(new MemoryStream(), Endianness.LittleEndian);

				if (Mode == ItemMode.Custom)
                {
					UInt16 offsetPlayer = baseOffset, offsetCPU = baseOffset;
					
					offsetPlayer += (UInt16)sectionWritter.BaseStream.Position;
					PlayerConfig.Write(sectionWritter);

					offsetCPU += (UInt16)sectionWritter.BaseStream.Position;
					CPUConfig.Write(sectionWritter);

					ew.Write(offsetPlayer); // offsetPlayer
					ew.Write(offsetCPU); // offsetCPU
					ew.Write((byte)Mode);
					ew.Write((byte)(SpawnItemBoxes ? 1 : 0));
					ew.WritePadding(4);
					ew.Write((sectionWritter.BaseStream as MemoryStream).ToArray(), 0, (int)sectionWritter.BaseStream.Length);

				} else
                {
					ew.Write((ushort)0xFFFF); // offsetPlayer
					ew.Write((ushort)0xFFFF); // offsetCPU
					ew.Write((byte)Mode);
					ew.Write((byte)(SpawnItemBoxes ? 1 : 0));
					ew.WritePadding(4);
				}	
				
				ew.WritePadding(4);

				return;
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
