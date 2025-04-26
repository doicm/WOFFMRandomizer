using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static void MultiplyFiveBattleSpeedByFive(string currDir, RichTextBox log)
        {
            log.AppendText("Increasing battle speed....\n");

            string cpPath = Path.Combine(currDir, "config_param.csv");

            List<List<string>> cpData = CsvHandling.CsvReadData(cpPath);

            cpData[7][3] = "4.0f";

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

        //// This does not work
        //public static void DoubleEncounterRate(string currDir, RichTextBox log)
        //{
        //    log.AppendText("Doubling encounter rate....\n");
        //    string metPath = Path.Combine(currDir, "map_encount_table.csv");
        //    List<List<string>> metData = CsvHandling.CsvReadData(metPath);


        //    foreach (List<string> row in metData)
        //    {
        //        if (row[0] == "0") continue; // skip first test row
        //        // There are 6 pairs of encounter sets per row
        //        // If the second value in the pair is -1, skip
        //        // Starts at index 3
        //        for (int i = 3; i < 15; i += 2)
        //        {
        //            if (row[i+1] != "-1")
        //            {
        //                // Double the value in the first value in the pair
        //                row[i] = (Int32.Parse(row[i]) * 2).ToString();
        //            }
        //        }
        //    }
        //    CsvHandling.CsvWriteDataAddHeadRow(metPath, metData, 15);
        //}
    }
}
