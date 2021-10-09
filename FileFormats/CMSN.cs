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
			Sections.Add(new ItemOptionsSection());
			Sections.Add(new TextSection());
			Sections.Add(new InfoSection());
		}
		public CMSN(byte[] Data)
		{
			Sections = new List<BaseSection>();
			EndianBinaryReaderEx er = new EndianBinaryReaderEx(new MemoryStream(Data), Endianness.LittleEndian);
			Header = new CMSNHeader(er);
			er.BaseStream.Position = Header.SectionTableOffset;
			uint sections = er.ReadUInt32();
			bool infoSectionPresent = false;
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
					case BaseSection.SectionType.TextStrings:
						Sections.Add(new TextSection(er));
						break;
					case BaseSection.SectionType.Info:
						infoSectionPresent = true;
						Sections.Add(new InfoSection(er));
						break;
					default:
						throw new NotImplementedException("Section " + sectionType + " not implemented!");
				}
				er.BaseStream.Position = prevPos;
            }
			if (!infoSectionPresent) Sections.Add(new InfoSection());
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
				ItemOptions = 3,
				TextStrings = 4,
				Info = 5,
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
				DriverChoices = new int[8][];
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
			public byte MissionType;
			public byte MissionSubType;
			public byte CalculationType;
			public byte LapAmount;
			public MK7Timer InitialTimer;
			public bool AutoFinishRaceTimer;
			public MK7Timer FinishRaceTimer;
			public MK7Timer MinGradeTimer;
			public MK7Timer MaxGradeTimer;
			public bool UseScore;
			public byte MinGradeScore;
			public byte MaxGradeScore;
			public byte InitialScore;
			public bool AutoFinishRaceScore;
			public byte FinishRaceScore;
			public bool CountTimerUp;
			public bool CountScoreUp;
			public byte YellowScore;
			private UInt32 Flags;
			public bool RespawnCoins;
			public MK7Timer RespawnCoinsTimer;
			public byte CompleteCondition1;
			public byte CompleteCondition2;
			public byte ImprovedTricksOption;
			public bool UseCCSelector;
			public UInt16 CCSelectorSpeed;

			private static UInt32 ClearBit(UInt32 value, int bit)
            {
				return (UInt32)(value & ~(1u << bit));
            }

			private static UInt32 SetBit(UInt32 value, int bit)
            {
				return (UInt32)(value | (1u << bit));
            }

			private static UInt32 ChangeBit(UInt32 value, int bit, bool state)
            {
				return state ? SetBit(value, bit) : ClearBit(value, bit);
            }

			private static bool GetBit(UInt32 value, int bit)
            {
				return (value & (1u << bit)) != 0;
            }

			public bool RankVisible { get { return GetBit(Flags, 0); } set { Flags = ChangeBit(Flags, 0, value); } }
			public bool LakituVisible { get { return GetBit(Flags, 1); } set { Flags = ChangeBit(Flags, 1, value); } }
			public bool CourseIntroVisible { get { return GetBit(Flags, 2); } set { Flags = ChangeBit(Flags, 2, value); } }
			public bool ScoreHidden { get { return GetBit(Flags, 3); } set { Flags = ChangeBit(Flags, 3, value); } }
			public bool ScoreNegative { get { return GetBit(Flags, 4); } set { Flags = ChangeBit(Flags, 4, value); } }
			public bool ForceBackwards { get { return GetBit(Flags, 5); } set { Flags = ChangeBit(Flags, 5, value); } }
			public bool FinishOnSection { get { return GetBit(Flags, 6); } set { Flags = ChangeBit(Flags, 6, value); } }
			public bool CoinCounterHidden { get { return GetBit(Flags, 7); } set { Flags = ChangeBit(Flags, 7, value); } }
			public bool LapCounterVisible { get { return GetBit(Flags, 8); } set { Flags = ChangeBit(Flags, 8, value); } }
			public bool GivePointOnHit { get { return GetBit(Flags, 9); } set { Flags = ChangeBit(Flags, 9, value); } }

			public MissionFlagsSection()
			{
				CourseID = 0;
				Class = 0;
				CPUDifficulty = 1;
				MissionType = 0;
				MissionSubType = 0;
				CalculationType = 0;
				LapAmount = 3;
				InitialTimer = new MK7Timer();
				AutoFinishRaceTimer = false;
				FinishRaceTimer = new MK7Timer();
				MinGradeTimer = new MK7Timer();
				MaxGradeTimer = new MK7Timer();
				UseScore = false;
				MinGradeScore = 0;
				MaxGradeScore = 0;
				InitialScore = 0;
				AutoFinishRaceScore = false;
				FinishRaceScore = 0;
				CountTimerUp = false;
				CountScoreUp = false;
				YellowScore = 0;
				Flags = 0;
				RespawnCoins = false;
				RespawnCoinsTimer = new MK7Timer();
				CompleteCondition1 = 0;
				CompleteCondition2 = 0;
				ImprovedTricksOption = 0;
				UseCCSelector = false;
				CCSelectorSpeed = 150;
			}
			public MissionFlagsSection(EndianBinaryReaderEx er) : this()
			{
				CourseID = er.ReadUInt32();
				Class = er.ReadByte();
				CPUDifficulty = er.ReadByte();

				MissionType = er.ReadByte();
				CalculationType = er.ReadByte();

				Flags = er.ReadUInt32();

				InitialTimer = new MK7Timer(er.ReadUInt32());

				uint finishRaceTimerValue = er.ReadUInt32();
				if (finishRaceTimerValue == 0xFFFFFFFF) AutoFinishRaceTimer = false;
				else { AutoFinishRaceTimer = true; FinishRaceTimer = new MK7Timer(finishRaceTimerValue); }

				switch (CalculationType)
                {
					case 0: // Timer
						MinGradeTimer = new MK7Timer(er.ReadUInt32());
						MaxGradeTimer = new MK7Timer(er.ReadUInt32());
						break;
					case 1: // Score
						MinGradeScore = (byte)er.ReadUInt32();
						MaxGradeScore = (byte)er.ReadUInt32();
						break;
                }

				byte initialScoreValue = er.ReadByte();
				if (initialScoreValue == 0xFF) UseScore = false;
				else { UseScore = true; InitialScore = initialScoreValue; }

				byte finishRaceScoreValue = er.ReadByte();
				if (finishRaceScoreValue == 0xFF) AutoFinishRaceScore = false;
				else { AutoFinishRaceScore = true; FinishRaceScore = finishRaceScoreValue; }

				MissionSubType = er.ReadByte();

				byte countDirectionValue = er.ReadByte();
				CountTimerUp = (countDirectionValue & 1) != 0;
				CountScoreUp = (countDirectionValue & (1 << 1)) != 0;

				YellowScore = er.ReadByte();
				LapAmount = er.ReadByte();

				ushort respawnCoinTimeValue = er.ReadUInt16();
				if (respawnCoinTimeValue == 0xFFFF) RespawnCoins = false;
				else { RespawnCoins = true; RespawnCoinsTimer = new MK7Timer((uint)(respawnCoinTimeValue * 20)); }

				byte completeConditionPacked = er.ReadByte();
				CompleteCondition1 = (byte)((uint)completeConditionPacked & 0xF);
				CompleteCondition2 = (byte)((uint)completeConditionPacked >> 4);

				ImprovedTricksOption = er.ReadByte();

				UInt16 ccSelectorValue = er.ReadUInt16();
				if (ccSelectorValue == 0) { UseCCSelector = false; CCSelectorSpeed = 150; }
				else { UseCCSelector = true; CCSelectorSpeed = ccSelectorValue; }

				er.ReadPadding(4);
			}
			public override void Write(EndianBinaryWriterEx ew)
			{
				ew.Write(CourseID);
				ew.Write(Class);
				ew.Write(CPUDifficulty);

				ew.Write(MissionType);
				ew.Write(CalculationType);

				ew.Write(Flags);

				ew.Write(InitialTimer.Frames);

				if (AutoFinishRaceTimer) ew.Write(FinishRaceTimer.Frames);
				else ew.Write(0xFFFFFFFF);

				switch (CalculationType)
				{
					case 0: // Timer
						ew.Write(MinGradeTimer.Frames);
						ew.Write(MaxGradeTimer.Frames);
						break;
					case 1: // Score
						ew.Write((uint)MinGradeScore);
						ew.Write((uint)MaxGradeScore);
						break;
				}

				if (UseScore) ew.Write(InitialScore);
				else ew.Write((byte)0xFF);

				if (AutoFinishRaceScore && UseScore) ew.Write(FinishRaceScore);
				else ew.Write((byte)0xFF);

				ew.Write(MissionSubType);

				byte countDirectionValue = (byte)(((CountTimerUp ? 1 : 0) << 0) | ((CountScoreUp ? 1 : 0) << 1));
				ew.Write(countDirectionValue);

				ew.Write(YellowScore);

				ew.Write(LapAmount);

				if (RespawnCoins) ew.Write((ushort)(RespawnCoinsTimer.Frames / 20f));
				else ew.Write((ushort)0xFFFF);

				byte completeConditionPacked = (byte)(((uint)CompleteCondition1 & 0xF) | ((uint)CompleteCondition2 << 4));
				ew.Write(completeConditionPacked);

				ew.Write(ImprovedTricksOption);
				if (UseCCSelector) ew.Write(CCSelectorSpeed);
				else ew.Write((ushort)0);

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
			public bool RespawnItemBox;
			public MK7Timer RespawnItemBoxTimer;
			
			public class ItemConfig
            {
				public byte[][] Probabilities;

				public ItemConfigMode ConfigMode;
				public MK7Timer GiveItemOffset;
				public MK7Timer GiveItemEach;
				public bool DisableCooldown;
				public MK7Timer RouletteSpeed;
				private bool IsPlayer;
				public enum ItemConfigMode
                {
					BoxID = 0,
					Rank = 1,
					DriverID = 2,
					TrueRank = 3
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
					DisableCooldown = false;
					RouletteSpeed = new MK7Timer(0);

                }
				public ItemConfig(EndianBinaryReaderEx er, bool isPlayer) : this(isPlayer)
                {
					RouletteSpeed = new MK7Timer(er.ReadUInt16());
					GiveItemOffset = new MK7Timer(er.ReadUInt16());
					GiveItemEach = new MK7Timer(er.ReadUInt16());
					DisableCooldown = er.ReadByte() != 0;
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
					ew.Write((byte)(DisableCooldown ? 1 : 0));
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
				RespawnItemBox = false;
				RespawnItemBoxTimer = new MK7Timer();
            }
			public ItemOptionsSection(EndianBinaryReaderEx er) : this()
            {
				long basePos = er.BaseStream.Position;
				long offsetPlayer = er.ReadUInt16();
				long offsetCPU = er.ReadUInt16();
				Mode = (ItemMode)er.ReadByte();
				SpawnItemBoxes = er.ReadByte() != 0;

				ushort respawnItemBoxTimeValue = er.ReadUInt16();
				if (respawnItemBoxTimeValue == 0xFFFF) RespawnItemBox = false;
				else { RespawnItemBox = true; RespawnItemBoxTimer = new MK7Timer(respawnItemBoxTimeValue); }

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
				tempWritter.Write(new ushort());
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
					if (RespawnItemBox) ew.Write((ushort)RespawnItemBoxTimer.Frames);
					else ew.Write((ushort)0xFFFF);
					ew.WritePadding(4);
					ew.Write((sectionWritter.BaseStream as MemoryStream).ToArray(), 0, (int)sectionWritter.BaseStream.Length);

				} else
                {
					ew.Write((ushort)0xFFFF); // offsetPlayer
					ew.Write((ushort)0xFFFF); // offsetCPU
					ew.Write((byte)Mode);
					ew.Write((byte)(SpawnItemBoxes ? 1 : 0));
					if (RespawnItemBox) ew.Write((ushort)RespawnItemBoxTimer.Frames);
					else ew.Write((ushort)0xFFFF);
					ew.WritePadding(4);
				}	
				
				ew.WritePadding(4);

				return;
            }
        }

		public class TextSection : BaseSection
		{
			public override SectionType GetSectionType()
			{
				return SectionType.TextStrings;
			}
			public List<LanguageEntry> entries;
			public class LanguageEntry
			{
				public string langCode;
				public string title;
				public byte[] description;
				public byte descriptionNewlines;

				public LanguageEntry()
				{
					langCode = "ENG";
					title = "Mission title";
					description = Encoding.UTF8.GetBytes("Mission description.");
					descriptionNewlines = 0;
				}

				public LanguageEntry(LanguageEntry other)
                {
					langCode = other.langCode;
					title = other.title;
					description = new byte[other.description.Length];
					other.description.CopyTo(description, 0);
					descriptionNewlines = other.descriptionNewlines;
                }

				public LanguageEntry(EndianBinaryReaderEx er)
                {
					long basepos = er.BaseStream.Position;
					ushort langOffset = er.ReadUInt16();
					ushort titleOffset = er.ReadUInt16();
					ushort descriptionOffset = er.ReadUInt16();
					descriptionNewlines = er.ReadByte();

					er.BaseStream.Position = basepos + langOffset;
					langCode = er.ReadStringNT(Encoding.UTF8);
					er.BaseStream.Position = basepos + titleOffset;
					title = er.ReadStringNT(Encoding.UTF8);

					MemoryStream tmpStream = new MemoryStream();
					er.BaseStream.Position = basepos + descriptionOffset;
					byte b;

					while (true)
                    {
						b = er.ReadByte();
						if (b != 0)
							tmpStream.WriteByte(b);
						else
							break;
                    }
					description = tmpStream.ToArray();
                }

				public void Write(EndianBinaryWriterEx ew)
                {
					long offset = 3 * sizeof(ushort) + sizeof(byte);
					EndianBinaryWriterEx tmpWritter = new EndianBinaryWriterEx(new MemoryStream());
					long lastpos = tmpWritter.BaseStream.Position;

					ew.Write((ushort)(offset + lastpos));
					tmpWritter.Write(langCode, Encoding.UTF8, true);
					lastpos = tmpWritter.BaseStream.Position;

					ew.Write((ushort)(offset + lastpos));
					tmpWritter.Write(title, Encoding.UTF8, true);
					lastpos = tmpWritter.BaseStream.Position;

					ew.Write((ushort)(offset + lastpos));
					tmpWritter.Write(description, 0, description.Length);
					tmpWritter.Write((byte)0);
					ew.Write(descriptionNewlines);
					ew.Write((tmpWritter.BaseStream as MemoryStream).ToArray(), 0, (int)tmpWritter.BaseStream.Length);
					ew.WritePadding(2);
				}

				public override string ToString()
				{
					return langCode;
				}
			}

			public TextSection()
			{
				entries = new List<LanguageEntry>();
				entries.Add(new LanguageEntry());
			}
			public TextSection(EndianBinaryReaderEx er)
			{
				entries = new List<LanguageEntry>();

				long basepos = er.BaseStream.Position;
				uint tableCount = er.ReadUInt32();
				for (int i = 0; i < tableCount; i++)
                {
					uint offset = er.ReadUInt32();
					long backup = er.BaseStream.Position;
					er.BaseStream.Position = basepos + offset;
					entries.Add(new LanguageEntry(er));
					er.BaseStream.Position = backup;
                }
			}
			public override void Write(EndianBinaryWriterEx ew)
			{
				EndianBinaryWriterEx langWritter = new EndianBinaryWriterEx(new MemoryStream(), Endianness.LittleEndian);
				ew.Write((uint)entries.Count);
				foreach (var entry in entries)
                {
					ew.Write((uint)(langWritter.BaseStream.Position + entries.Count * sizeof(uint) + sizeof(uint)));
					entry.Write(langWritter);
                }
				ew.Write((langWritter.BaseStream as MemoryStream).ToArray(), 0, (int)langWritter.BaseStream.Length);
				ew.WritePadding(4);
			}
		}

		public class InfoSection : BaseSection
        {
			public override SectionType GetSectionType()
			{
				return SectionType.Info;
			}

			public byte[] MissionUUID;
			public UInt32 SaveIteration;
			public UInt32 Checksum;
			public UInt32 Key;
			public byte World;
			public byte Level;
			public InfoSection()
            {
				MissionUUID = new byte[0xC];
				System.Security.Cryptography.RNGCryptoServiceProvider RandomGenerator = new System.Security.Cryptography.RNGCryptoServiceProvider();
				RandomGenerator.GetNonZeroBytes(MissionUUID);
				MissionUUID[MissionUUID.Length - 1] = 0;
				SaveIteration = 0;
				Checksum = 0;
				Key = 0;
				World = 1;
				Level = 1;
            }
			public InfoSection(EndianBinaryReaderEx er)
            {
				MissionUUID = er.ReadBytes(0xC);
				SaveIteration = er.ReadUInt32();
				Checksum = er.ReadUInt32();
				try
                {
					Key = er.ReadUInt32();
					World = er.ReadByte();
					Level = er.ReadByte();
					er.ReadPadding(4);
				} catch (Exception)
                {
					Key = 0;
					World = 1;
					Level = 1;
                }
				if (World == 0) World = 1;
				if (Level == 0 || Level > 4) Level = 1;
            }
			public override void Write(EndianBinaryWriterEx ew)
            {
				ew.Write(MissionUUID, 0, 0xC);

				ew.Write(SaveIteration);
				ew.Write(Checksum);
				ew.Write(Key);
				ew.Write(World);
				ew.Write(Level);
				ew.WritePadding(4);
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
