using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOFFRandomizer.Dependencies;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace WOFFRandomizer
{
    internal class QoL
    {
        private static float CheckFloatOrIntAndConvert(string s)
        {
            // If it's ends with an f, return it as a float
            if (s.EndsWith("f")) return float.Parse(s.Substring(0, s.Length - 1));
            else // If it doesn't end with an f, it's an int. Convert to a float
            {
                int i = Int32.Parse(s);
                return (float)i;
            }
        }
        public static void IncreaseBattleSpeed(string currDir, RichTextBox log)
        {
            log.AppendText("Increasing battle speed....\n");

            string cpPath = Path.Combine(currDir, "config_param.csv");

            List<List<string>> cpData = CsvHandling.CsvReadData(cpPath);

            cpData[5][3] = "1.2f";
            cpData[6][3] = "1.5f";
            cpData[7][3] = "1.8f";
            cpData[8][3] = "2.1f";
            cpData[9][3] = "2.4f";

            CsvHandling.CsvWriteDataAddHeadRow(cpPath, cpData, 4);
        }
        public static void AddCSSkipToIntroCutscene(string currDir, RichTextBox log)
        {
            string scriptBinary = Path.Combine(currDir, "script64.bin");
            using (FileStream barcStream = new FileStream(scriptBinary, FileMode.Open, FileAccess.ReadWrite))
            {
                using (BinaryReader barcReader = new BinaryReader(barcStream))
                {
                    // This is the position of lua script ev02_0010.lua, where the intro cutscene is
                    barcReader.BaseStream.Position = 0x19A001;

                    string luaMagic = Encoding.UTF8.GetString(barcReader.ReadBytes(3));
                    if (luaMagic != "Lua")
                    {
                        log.AppendText("Couldn't put skip button in intro cutscene.\n");
                        return;
                    }
                    // Go to the specific code manually that I need to change from 00 to 80
                    // It's like flipping a bit
                    barcReader.BaseStream.Position = 0x19A091;
                    barcStream.WriteByte(0x80);
                }
            }
        }

        public static void AddCSSkipsToEnding(string currDir, RichTextBox log)
        {
            string scriptBinary = Path.Combine(currDir, "script64.bin");
            using (FileStream barcStream = new FileStream(scriptBinary, FileMode.Open, FileAccess.ReadWrite))
            {
                using (BinaryReader barcReader = new BinaryReader(barcStream))
                {
                    // This is the position of lua script ev22_0110.lua, where the first ending cutscene is
                    barcReader.BaseStream.Position = 0x604001;

                    string luaMagic = Encoding.UTF8.GetString(barcReader.ReadBytes(3));
                    if (luaMagic != "Lua")
                    {
                        log.AppendText("Couldn't put skip button in intro cutscene.\n");
                        return;
                    }
                    // Go to the specific code manually that I need to change from 00 to 80
                    // It's like flipping a bit
                    barcReader.BaseStream.Position = 0x604091;
                    barcStream.WriteByte(0x80);

                    // This is the position of lua script ev22_0120.lua, where the credits movie is
                    barcReader.BaseStream.Position = 0x608801;

                    luaMagic = Encoding.UTF8.GetString(barcReader.ReadBytes(3));
                    if (luaMagic != "Lua")
                    {
                        log.AppendText("Couldn't put skip button in intro cutscene.\n");
                        return;
                    }
                    // Go to the specific code manually that I need to change from 00 to 80
                    // It's like flipping a bit
                    barcReader.BaseStream.Position = 0x608891;
                    barcStream.WriteByte(0x80);
                }
            }
        }

        public static void AddCSSkipToGigans(string currDir, RichTextBox log)
        {
            string scriptBinary = Path.Combine(currDir, "script64.bin");
            using (FileStream barcStream = new FileStream(scriptBinary, FileMode.Open, FileAccess.ReadWrite))
            {
                using (BinaryReader barcReader = new BinaryReader(barcStream))
                {
                    // This is the position of lua script ev22_0110.lua, where the first ending cutscene is
                    barcReader.BaseStream.Position = 0x3F3801;

                    string luaMagic = Encoding.UTF8.GetString(barcReader.ReadBytes(3));
                    if (luaMagic != "Lua")
                    {
                        log.AppendText("Couldn't put skip button in Gigantaur/Gigantrot cutscene.\n");
                        return;
                    }
                    // Go to the specific code manually that I need to change from 00 to 80
                    // It's like flipping a bit
                    barcReader.BaseStream.Position = 0x3F3891;
                    barcStream.WriteByte(0x80);
                }
            }
        }

        public static void DoubleMovementSpeed(string currDir, RichTextBox log)
        {
            log.AppendText("Increasing movement speed....\n");
            string mmpPath = Path.Combine(currDir, "map_move_param.csv");
            List<List<string>> mmpData = CsvHandling.CsvReadData(mmpPath);

            // Go through each row, get data from 4th column (index 3), and double the value there
            foreach (List<string> row in mmpData)
            {
                // Double the value
                float f = CheckFloatOrIntAndConvert(row[3]) * 2;
                row[3] = f.ToString() + "f";
            }

            CsvHandling.CsvWriteDataAddHeadRow(mmpPath, mmpData, 18);
        }

        public static void DecreaseDialogue(string basepath, RichTextBox log)
        {
            log.AppendText("Speeding up dialogue....\n");
            // For this to work, many ev##_#### csv files are going to have copies made and edited.
            // There will be a separate function for uninstallation, since this operation will probably be large
            // I will not include cutscenes that can be skipped to save on operation cost
            string path = "/resource/finalizedCommon/mithril/system/csv/message/us";

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
                DialogueReduce.ReduceTimeToZeroOnDialogue(basepath, path, csh, log);
            }
            
        }
    }
}
