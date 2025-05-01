using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CshToolHelpers;

namespace WOFFRandomizer.Dependencies
{
    internal class DialogueReduce
    {
        private static void VerifyOpenAndCopy(string basepath, string path, string name, RichTextBox log)
        {
            // Make backup of file. Verify the file is there first.
            string source = Path.GetFullPath(basepath + path + "/" + name + "");
            string backup = Path.GetFullPath(basepath + path + "/" + name + "_original");
            if (!File.Exists(source))
            {
                log.AppendText("Cannot locate file " + path + "/" + name + "\n");
            }

            string destinationToCopyTo = Path.GetFullPath(basepath + path + "/" + name);
            // If original data exists already as a backup (if running the randomizer twice or more in a row), get the original data
            if (File.Exists(backup))
            {
                // Create a copy locally for easy management
                File.Copy(backup, destinationToCopyTo, true);
            }
            else
            {
                File.Copy(source, backup, true);
            }
        }

        private static void DefineCopyAndRemove(string dir, string name)
        {
            string source = Path.Combine(dir, name);
            string backup = Path.Combine(dir, name + "_original");

            if (!File.Exists(backup))
            {
                return;
            }
            else if (source != "")
            {
                File.Copy(backup, source, true);
                File.Delete(backup);
            }
        }

        public static void ReduceTimeToZeroOnDialogue(string basepath, string path, string name, RichTextBox log)
        {
            VerifyOpenAndCopy(basepath, path, "\\" + name + ".csh", log);

            string fullpathCsh = Path.GetFullPath(basepath + path + "/" + name + ".csh");

            // Convert Csh to Csv
            ConversionHelpers.ConvertToCsv(fullpathCsh);

            string fullpathCsv = Path.GetFullPath(basepath + path + "/" + name + ".csv");

            // Edit Csv
            List<List<string>> csvData = CsvHandling.CsvReadDataIncHeadRow(fullpathCsv);

            // (I may need to make a special case for ev17_0040, but I'll check when I get there (talking to Cloud)
            foreach (List<string> row in csvData)
            {
                row[7] = "0";
            }

            CsvHandling.CsvWriteData(fullpathCsv, csvData);

            // Convert Csv to Csh
            ConversionHelpers.ConvertToCsh(fullpathCsv);

            // Delete Csv
            File.Delete(fullpathCsv);

        }

        public static void UninstallEventCshFiles(string basepath)
        {
            string messageUSfolder = basepath + "/resource/finalizedCommon/mithril/system/csv/message/us/";

            List<string> eventCshs = ["ev00_0065", "ev01_0016", "ev01_0017", "ev01_0021", "ev01_0050", "ev01_0086",
                "ev02_0085", "ev02_add_00", "ev03_0027", "ev03_0050", "ev04_add_gimmick00", "ev06_add_gimmick00",
                "ev07_0016", "ev08_add_gimmick00", "ev08_add_gimmick01", "ev08_add_gimmick03",
                "ev08_0035", "ev08_0045", "ev09_0035", "ev09_add_gimmick00", "ev12_0020", "ev14_0015",
                "ev14_0060", "ev15_add_gimmick00", "ev16_0095", "ev17_0100", "ev18_0080", "ev18_0082", "ev18_0083",
                "ev18_0110", "ev19_0108", "ev19_0109", "ev21_0020", "ev21_0136",
                "ev02_0015", "ev03_0040", "ev06_0040", "sub02_add_00", "ev08_0025", "ev08_0028", "ev09_0040",
                "ev10_add_00", "ev10_add_01", "ev10_add_02", "ev10_add_03", "ev10_add_04", "ev17_0020",
                "ev18_0180", "ev19_add_03", "ev19_add_04", "ev20_add_00"];

            foreach (string csh in eventCshs)
            {
                DefineCopyAndRemove(messageUSfolder, csh + ".csh");
            }
        }

    }
}
