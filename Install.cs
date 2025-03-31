using CshToolHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOFFRandomizer
{
    internal class Install
    {
        // Method for verifying CSH files, opening them and copying them to backup location for working on
        private static void verifyOpenAndCopy(string basepath, string currDir, string cshName, RichTextBox log)
        {
            // Make backup of file. Verify the file is there first.
            string sourceCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + cshName + ".csh");
            string backupCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + cshName + "_original.csh");
            if (!File.Exists(sourceCSH))
            {
                log.AppendText("Cannot locate file /resource/finalizedCommon/mithril/system/csv/" + cshName + ".csh\n");   
            }

            string destinationToCopyTo = Path.GetFullPath(currDir + "/" + cshName + ".csh");
            // If original data exists already as a backup (if running the randomizer twice or more in a row), get the original data
            if (File.Exists(backupCSH))
            {
                // Create a copy locally for easy management
                File.Copy(backupCSH, destinationToCopyTo, true);
            }
            else
            {
                File.Copy(sourceCSH, backupCSH, true);
                // Create a copy locally for easy management
                File.Copy(sourceCSH, destinationToCopyTo, true);
            }
        }

        private static void copyBackAndDelete(string basepath, string currDir, string name)
        {
            string sourceCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + name + ".csh");
            string csvThatWasEdited = Path.GetFullPath(currDir + "/" + name + ".csv");
            string cshThatWasProduced = Path.GetFullPath(currDir + "/" + name + ".csh");

            File.Copy(cshThatWasProduced, sourceCSH, true);
            File.Delete(csvThatWasEdited);
            File.Delete(cshThatWasProduced);
        }
        public static void Run(string basepath, string sV, RichTextBox log, bool mbShuffle, bool enemShuffle, bool bossShuffle, bool itemShuffle)
        {
            string currDir = Directory.GetCurrentDirectory();

            // clear the logs before starting
            Uninstall.clearLogs();
            
            // Verify, open, and copy the files to the proper locations for installation
            if (mbShuffle | enemShuffle) verifyOpenAndCopy(basepath, currDir, "mirageboard_data", log);
            if (mbShuffle | enemShuffle) verifyOpenAndCopy(basepath, currDir, "enemy_group_list", log);
            if (mbShuffle | enemShuffle) verifyOpenAndCopy(basepath, currDir, "character_enemy_status_list", log);
            if (mbShuffle | enemShuffle) verifyOpenAndCopy(basepath, currDir, "shop_list", log);
            if (mbShuffle | enemShuffle) verifyOpenAndCopy(basepath, currDir, "monster_place", log);

            //if input is blank, get current time in unix
            if (sV.Length == 0)
            {
                sV = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            }
            log.AppendText("Started generating seed " + sV + "...\n");

            // Write to txt file with seed name in it for reference
            System.IO.File.WriteAllText(currDir + "/logs/seed.txt", sV);

            // Run the WoFFCshTool by Surihia twice, one to decompress csh and convert to csv, and one to do the reverse after writing the values
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "mirageboard_data.csh"));
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "enemy_group_list.csh")); ;
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "character_enemy_status_list.csh"));;
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "shop_list.csh")); ;
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "monster_place.csh")); ;

            // Write the values
            if (mbShuffle) Mirageboard.mirageboard_dataWriteCsv(currDir, sV, log);
            if (enemShuffle) Shop.putEldboxInShops(currDir, log);
            if (enemShuffle) Enemy.mirageEncsWriteCsv(currDir, sV, log, bossShuffle);
            if (enemShuffle) Mirageboard.modifyForEnemyRandoOnly(currDir);
            if (itemShuffle) Item.treasureShuffle(currDir, sV, log);

            // Second WoFFCshTool run
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "mirageboard_data.csv"));
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "enemy_group_list.csv"));
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "character_enemy_status_list.csv"));
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "shop_list.csv"));
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "monster_place.csv"));

            if (mbShuffle | enemShuffle) copyBackAndDelete(basepath, currDir, "mirageboard_data");
            if (mbShuffle | enemShuffle) copyBackAndDelete(basepath, currDir, "enemy_group_list");
            if (mbShuffle | enemShuffle) copyBackAndDelete(basepath, currDir, "character_enemy_status_list");
            if (mbShuffle | enemShuffle) copyBackAndDelete(basepath, currDir, "shop_list");
            if (mbShuffle | enemShuffle) copyBackAndDelete(basepath, currDir, "monster_place");

            log.AppendText("Finished generating seed " + sV + "!\n");
            
        }
    }
}
