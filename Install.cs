using CshToolHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOFFRandomizer.Dependencies;

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

        private static void verifyOpenAndCopyOtherFile(string basepath, string currDir, string path, string name, RichTextBox log)
        {
            // Make backup of file. Verify the file is there first.
            string source = Path.GetFullPath(basepath + path + name);
            string backup = Path.GetFullPath(basepath + path + name + "_original");
            if (!File.Exists(source))
            {
                log.AppendText("Cannot locate file " + path + name + "\n");
            }

            string destinationToCopyTo = Path.GetFullPath(currDir + name);
            // If original data exists already as a backup (if running the randomizer twice or more in a row), get the original data
            if (File.Exists(backup))
            {
                // Create a copy locally for easy management
                File.Copy(backup, destinationToCopyTo, true);
            }
            else
            {
                File.Copy(source, backup, true);
                // Create a copy locally for easy management
                File.Copy(source, destinationToCopyTo, true);
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

        private static void copyBackAndDeleteOtherFile(string basepath, string currDir, string path, string name, RichTextBox log)
        {
            string source = Path.GetFullPath(basepath + path + name);
            string fileThatWasEdited = Path.GetFullPath(currDir + name);

            File.Copy(fileThatWasEdited, source, true);
            File.Delete(fileThatWasEdited);
        }

        private static void WriteToSeedLog(string currDir, string sV, bool mbShuffle, bool enemShuffle, bool bossShuffle, bool itemShuffle, bool rareShuffle,
            bool sizesShuffle, bool quPrizesShuffle, bool murkShuffle, bool doubleExpBool)
        {
            string toWrite = sV + Environment.NewLine;
            toWrite += Environment.NewLine;
            if (mbShuffle) toWrite += "Mirageboard nodes randomized...." + Environment.NewLine;
            if (enemShuffle) toWrite += "Random encounters randomized...." + Environment.NewLine;
            if (rareShuffle) toWrite += "Rare mirages randomized...." + Environment.NewLine;
            if (bossShuffle) toWrite += "Bosses randomized...." + Environment.NewLine;
            if (murkShuffle) toWrite += "Murkrifts randomized...." + Environment.NewLine;
            if (itemShuffle) toWrite += "Treasure chests randomized...." + Environment.NewLine;
            if (sizesShuffle) toWrite += "Mirage sizes randomized...." + Environment.NewLine;
            if (quPrizesShuffle) toWrite += "Intervention quest/coliseum rewards randomized...." + Environment.NewLine;
            if (doubleExpBool) toWrite += "Experience and gil doubled...." + Environment.NewLine;

            System.IO.File.WriteAllText(currDir + "/logs/seed.txt", toWrite);
        }
        public static void Run(string basepath, string sV, RichTextBox log, bool mbShuffle, bool enemShuffle, bool bossShuffle, bool itemShuffle, bool rareShuffle,
            bool sizesShuffle, bool quPrizesShuffle, bool murkShuffle, bool doubleExpBool, Button button1, Button button2, Button button3)
        {
            string currDir = Directory.GetCurrentDirectory();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            // clear the logs before starting
            Uninstall.clearLogs();

            // Verify, open, and copy the files to the proper locations for installation
            // In case other randomizer checkboxes are disabled, want to run on all of these
            verifyOpenAndCopy(basepath, currDir, "mirageboard_data", log);
            verifyOpenAndCopy(basepath, currDir, "enemy_group_list", log);
            verifyOpenAndCopy(basepath, currDir, "character_enemy_status_list", log);
            verifyOpenAndCopy(basepath, currDir, "shop_list", log);
            verifyOpenAndCopy(basepath, currDir, "monster_place", log);
            verifyOpenAndCopyOtherFile(basepath, currDir, "/resource", "/script64.bin", log);
            verifyOpenAndCopy(basepath, currDir, "character_resource_list", log);
            verifyOpenAndCopy(basepath, currDir, "command_ability_param", log);
            verifyOpenAndCopy(basepath, currDir, "arena_reward_table_list", log);
            verifyOpenAndCopy(basepath, currDir, "quest_data_sub_reward_table_list", log);

            //if input is blank, get current time in unix
            if (sV.Length == 0)
            {
                sV = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            }
            log.AppendText("Started generating seed " + sV + "...\n");

            // Write to txt file with seed name in it for reference
            WriteToSeedLog(currDir, sV, mbShuffle, enemShuffle, bossShuffle, itemShuffle, rareShuffle, sizesShuffle, quPrizesShuffle, murkShuffle, doubleExpBool);

            // Run the WoFFCshTool by Surihia twice, one to decompress csh and convert to csv, and one to do the reverse after writing the values
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "mirageboard_data.csh"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | murkShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "enemy_group_list.csh"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | doubleExpBool | murkShuffle ) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "character_enemy_status_list.csh"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "shop_list.csh"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "monster_place.csh"));
            if (sizesShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "character_resource_list.csh"));
            if (sizesShuffle | mbShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "command_ability_param.csh"));
            if (quPrizesShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "arena_reward_table_list.csh"));
            if (quPrizesShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "quest_data_sub_reward_table_list.csh"));

            // Write the values
            if (mbShuffle) Mirageboard.mirageboard_dataWriteCsv(currDir, sV, log);
            if (enemShuffle) Shop.putEldboxInShops(currDir, log);
            if (enemShuffle|rareShuffle|bossShuffle) Enemy.mirageEncsWriteCsv(currDir, sV, log, enemShuffle, bossShuffle, rareShuffle);
            if (murkShuffle) Murkrift.MurkriftShuffle(currDir, sV, log);
            if (enemShuffle) Mirageboard.modifyForEnemyRandoOnly(currDir);
            if (itemShuffle) Item.TreasureShuffle(currDir, sV, log);
            if (sizesShuffle) Sizes.SizesShuffle(currDir, basepath, sV, log);
            if (quPrizesShuffle) QuOrArenaPrizes.PrizesShuffle(currDir, sV, log);

            // Apply post-QoL adjustments
            if (doubleExpBool) DoubleExpQoL.DoubleExpGil(currDir, log);

            // Second WoFFCshTool run
            if (mbShuffle | enemShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "mirageboard_data.csv"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | murkShuffle ) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "enemy_group_list.csv"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | doubleExpBool | murkShuffle ) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "character_enemy_status_list.csv"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "shop_list.csv"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "monster_place.csv"));
            if (sizesShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "character_resource_list.csv"));
            if (sizesShuffle | mbShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "command_ability_param.csv"));
            if (quPrizesShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "arena_reward_table_list.csv"));
            if (quPrizesShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "quest_data_sub_reward_table_list.csv"));

            // In case other randomizer checkboxes are disabled, want to run on all of these
            copyBackAndDelete(basepath, currDir, "mirageboard_data");
            copyBackAndDelete(basepath, currDir, "enemy_group_list");
            copyBackAndDelete(basepath, currDir, "character_enemy_status_list");
            copyBackAndDelete(basepath, currDir, "shop_list");
            copyBackAndDelete(basepath, currDir, "monster_place");
            copyBackAndDeleteOtherFile(basepath, currDir, "/resource", "/script64.bin", log);
            copyBackAndDelete(basepath, currDir, "character_resource_list");
            copyBackAndDelete(basepath, currDir, "command_ability_param");
            copyBackAndDelete(basepath, currDir, "arena_reward_table_list");
            copyBackAndDelete(basepath, currDir, "quest_data_sub_reward_table_list");

            log.AppendText("Finished generating seed " + sV + "!\n");
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;

        }
    }
}
