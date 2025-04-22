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
    }
}
