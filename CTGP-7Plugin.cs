using LibEveryFileExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using _3DS;
using LibEveryFileExplorer.Script;
using System.Collections.Concurrent;
using _3DS.UI;
using System.Windows.Forms;

namespace CTGP7
{
    public class CTGP7Plugin : EFEPlugin
    {
        public override void OnLoad()
        {
            EFEScript.RegisterCommand("CTGP7.SARC.GenerateHashTable", (Action<String>)GenerateHashTable);
            SARCViewer.AddSARCNameGuessHint(SARCGuessHint);
            try
            {
                String path = Path.GetDirectoryName(Application.ExecutablePath) + "\\Plugins\\CTGP-7HashTable.saht";
                if (SARCHashTable.DefaultHashTable != null) SARCHashTable.DefaultHashTable.Merge(new SARCHashTable(File.ReadAllBytes(path)));
            } catch { }
        }

        private static String[] filenamePrefixes =
        {
            "Common.szs/",
            "Menu3D.szs/",
            "RaceCommon.szs/",
            "Thankyou3D.szs/",
            "UI/common.szs/",
            "UI/common-ed.szs/",
            "UI/common-ee.szs/",
            "UI/common-ef.szs/",
            "UI/common-ei.szs/",
            "UI/common-en.szs/",
            "UI/common-ep.szs/",
            "UI/common-er.szs/",
            "UI/common-es.szs/",
            "UI/common-jp.szs/",
            "UI/ending.szs/",
            "UI/menu.szs/",
            "UI/menu-ed.szs/",
            "UI/menu-ee.szs/",
            "UI/menu-ef.szs/",
            "UI/menu-ei.szs/",
            "UI/menu-en.szs/",
            "UI/menu-ep.szs/",
            "UI/menu-er.szs/",
            "UI/menu-es.szs/",
            "UI/menu-jp.szs/",
            "UI/race.szs/",
            "UI/race-ed.szs/",
            "UI/race-ee.szs/",
            "UI/race-ef.szs/",
            "UI/race-ei.szs/",
            "UI/race-en.szs/",
            "UI/race-ep.szs/",
            "UI/race-er.szs/",
            "UI/race-es.szs/",
            "UI/race-jp.szs/",
            "UI/thankyou.szs/",
            "UI/thankyou-ed.szs/",
            "UI/thankyou-ee.szs/",
            "UI/thankyou-ef.szs/",
            "UI/thankyou-ei.szs/",
            "UI/thankyou-en.szs/",
            "UI/thankyou-ep.szs/",
            "UI/thankyou-er.szs/",
            "UI/thankyou-es.szs/",
            "UI/thankyou-jp.szs/",
            "UI/trophy.szs/",
            "UI/trophy-ed.szs/",
            "UI/trophy-ee.szs/",
            "UI/trophy-ef.szs/",
            "UI/trophy-ei.szs/",
            "UI/trophy-en.szs/",
            "UI/trophy-ep.szs/",
            "UI/trophy-er.szs/",
            "UI/trophy-es.szs/",
            "UI/trophy-jp.szs/",
        };

        private static SARCHashTable modTable = null;
        public static SARCHashTable GetHashTable()
        {
            if (modTable != null)
                return modTable;
            if (SARCHashTable.DefaultHashTable == null)
            {
                return null;
            }
            ConcurrentBag<Tuple<uint, String>> bag = new ConcurrentBag<Tuple<uint, string>>();
            Parallel.ForEach(filenamePrefixes, p =>
            {
                uint startHash = SARC.GetHashFromName(p, 0x65);
                foreach (var entry in SARCHashTable.DefaultHashTable.Entries)
                {
                    uint newHash = SARC.GetHashFromName(entry.Name, 0x65, startHash);
                    bag.Add(new Tuple<uint, String>(newHash, p + entry.Name));
                }
            });

            SARCHashTable output = new SARCHashTable();
            foreach (var entry in bag)
            {
                SARCHashTable.SAHTEntry e = new SARCHashTable.SAHTEntry();
                e.Name = entry.Item2;
                e.Hash = entry.Item1;
                output.Entries.Add(e);
            }
            output.SortEntriesByHash();
            modTable = output;
            return output;
        }

        public static void GenerateHashTable(String outputPath)
        {

            SARCHashTable ret = GetHashTable();
            if (ret != null)
                File.WriteAllBytes(outputPath, ret.Write());
        }
        public static void SARCGuessHint(SARCViewer viewer)
        {
            SARCHashTable ret = GetHashTable();
            if (ret == null)
                return;
            SARCHashTable newTable = new SARCHashTable();
            foreach (var item in viewer.Root.Files)
            {
                if (item.FileName.StartsWith("0x"))
                {
                    var entry = ret.GetEntryByHash((uint)item.FileID);
                    if (entry != null)
                    {
                        newTable.Entries.Add(entry);
                        item.FileName = entry.Name;
                    }
                }
            }
            SARCHashTable.DefaultHashTable.Merge(newTable);
        }
    }
}
