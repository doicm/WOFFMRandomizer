﻿using CshToolHelpers;
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
        private static void PutSeedAndVersionOnTitleScreen(string currDir, string sV, string version)
        {
            // Convert menu csh to csv
            ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "menu.csh"));

            string menuPath = Path.Combine(currDir, "menu.csv");
            List<List<string>> menuData = CsvHandling.CsvReadData(menuPath);

            // Add Randomizer version and seed value to title screen
            if (menuData[797][0] == "TITLE_GAME_VERSION")
            {
                menuData[797][6] = "Ver <NUM PARAM>[~]Randomizer Ver " + version + "[~]Seed: " + sV;
            }

            // Convert menu csv back to csh
            CsvHandling.CsvWriteDataAddHeadRow(menuPath, menuData, 9);
            ConversionHelpers.ConvertToCsh(menuPath);
        }

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

        private static void copyBackAndDeleteOtherCshCsv(string basepath, string currDir, string path, string name)
        {
            string sourceCSH = Path.GetFullPath(basepath + path + name + ".csh");
            string csvThatWasEdited = Path.GetFullPath(currDir + "/" + name + ".csv");
            string cshThatWasProduced = Path.GetFullPath(currDir + "/" + name + ".csh");

            File.Copy(cshThatWasProduced, sourceCSH, true);
            File.Delete(csvThatWasEdited);
            File.Delete(cshThatWasProduced);
        }

        private static void WriteToSeedLog(string currDir, string sV, bool mbShuffle, bool enemShuffle, bool bossShuffle, bool itemShuffle, 
            bool libraShuffle, bool dataseedsShuffle, bool datajewelsShuffle, bool readeritemsShuffle, bool rareShuffle, bool sizesShuffle, bool quPrizesShuffle, 
            bool murkShuffle, bool statShuffle, bool transfigShuffle, bool doubleExpBool, bool battleSpeedBool, bool movementBool, bool dialogueBool, 
            bool t2AttackItemsBool)
        {
            string toWrite = sV + Environment.NewLine;
            toWrite += Environment.NewLine;
            if (mbShuffle) toWrite += "Mirageboard nodes randomized...." + Environment.NewLine;
            if (enemShuffle) toWrite += "Random encounters randomized...." + Environment.NewLine;
            if (rareShuffle) toWrite += "Rare mirages randomized...." + Environment.NewLine;
            if (bossShuffle) toWrite += "Bosses randomized...." + Environment.NewLine;
            if (murkShuffle) toWrite += "Murkrifts randomized...." + Environment.NewLine;
            if (itemShuffle) toWrite += "Treasure chests randomized...." + Environment.NewLine;
            if (libraShuffle) toWrite += "Libra Mirajewel shuffled...." + Environment.NewLine;
            if (dataseedsShuffle) toWrite += "Ability seeds in chests replaced...." + Environment.NewLine;
            if (datajewelsShuffle) toWrite += "Mirajewels in chests replaced...." + Environment.NewLine;
            if (readeritemsShuffle) toWrite += "Reading items included in randomization...." + Environment.NewLine;
            if (sizesShuffle) toWrite += "Mirage sizes randomized...." + Environment.NewLine;
            if (quPrizesShuffle) toWrite += "Intervention quest/coliseum rewards randomized...." + Environment.NewLine;
            if (statShuffle) toWrite += "Mirage stats randomized...." + Environment.NewLine;
            if (transfigShuffle) toWrite += "Transfigurations/MBs randomized...." + Environment.NewLine;
            if (doubleExpBool) toWrite += "Experience and gil doubled...." + Environment.NewLine;
            if (battleSpeedBool) toWrite += "Battle speed increased...." + Environment.NewLine;
            if (movementBool) toWrite += "Movement speed increased...." + Environment.NewLine;
            if (dialogueBool) toWrite += "Dialogue in battles and field decreased...." + Environment.NewLine;
            if (t2AttackItemsBool) toWrite += "Removed some attack items from shop...." + Environment.NewLine;

            System.IO.File.WriteAllText(currDir + "/logs/seed.txt", toWrite);
        }

        private static void ChangeProgressStatus(int currChecks, int totalChecks, RichTextBox percentProgress)
        {
            float calc = (currChecks * 1000 / totalChecks);
            calc = calc / 10;
            percentProgress.Text = calc.ToString() + "%";
        }
        public static void Run(string basepath, string sV, string version, RichTextBox log, bool mbShuffle, bool enemShuffle, bool bossShuffle, bool itemShuffle, 
            bool libraShuffle, bool dataseedsShuffle, bool datajewelsShuffle, bool readeritemsShuffle, bool rareShuffle, bool sizesShuffle, bool quPrizesShuffle, bool murkShuffle, 
            bool statShuffle, bool transfigShuffle, bool doubleExpBool, bool battleSpeedBool, bool movementBool, bool dialogueBool,
            bool t2AttackItemsBool, Button button1, Button button2, Button button3, int totalChecks, RichTextBox percentProgress)
        {
            string currDir = Directory.GetCurrentDirectory();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            // clear the logs before starting
            Uninstall.clearLogs();

            // Verify, open, and copy the files to the proper locations for installation
            // In case other randomizer checkboxes are disabled, want to run on all of these
            verifyOpenAndCopyOtherFile(basepath, currDir, "/resource/finalizedCommon/mithril/system/csv/message/us", "/menu.csh", log);
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
            verifyOpenAndCopy(basepath, currDir, "character_list", log);
            verifyOpenAndCopy(basepath, currDir, "config_param", log);
            verifyOpenAndCopyOtherFile(basepath, currDir, "/resource/finalizedCommon/mithril/map/csv", "/map_move_param.csh", log);
            verifyOpenAndCopy(basepath, currDir, "mirage_library_list", log);

            //if input is blank, get current time in unix
            if (sV.Length == 0)
            {
                sV = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            }
            log.SelectionFont = new Font(log.Font, FontStyle.Regular);
            log.AppendText("Started generating seed " + sV + "...\n");

            // Write to txt file with seed name in it for reference
            WriteToSeedLog(currDir, sV, mbShuffle, enemShuffle, bossShuffle, itemShuffle, libraShuffle, dataseedsShuffle, datajewelsShuffle, readeritemsShuffle,
                rareShuffle, sizesShuffle, quPrizesShuffle, murkShuffle, statShuffle, transfigShuffle, doubleExpBool, battleSpeedBool, movementBool, dialogueBool,
                t2AttackItemsBool);

            // Put the seed name and version number on the title screen by editing menu.csv
            PutSeedAndVersionOnTitleScreen(currDir, sV, version);

            // Run the WoFFCshTool by Surihia twice, one to decompress csh and convert to csv, and one to do the reverse after writing the values
            ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "mirageboard_data.csh"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | murkShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "enemy_group_list.csh"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | doubleExpBool | murkShuffle ) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "character_enemy_status_list.csh"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | t2AttackItemsBool) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "shop_list.csh"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "monster_place.csh"));
            if (sizesShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "character_resource_list.csh"));
            if (sizesShuffle | mbShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "command_ability_param.csh"));
            if (quPrizesShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "arena_reward_table_list.csh"));
            if (quPrizesShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "quest_data_sub_reward_table_list.csh"));
            if (statShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "character_list.csh"));
            if (battleSpeedBool) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "config_param.csh"));
            if (movementBool) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "map_move_param.csh"));
            if (enemShuffle) ConversionHelpers.ConvertToCsv(Path.Combine(currDir, "mirage_library_list.csh"));

            // QoL to apply before writing values
            Mirageboard.MiragesGiveLureAndStealthStones(currDir);

            int currChecks = 0;

            percentProgress.Text = currChecks.ToString() + ".0%";
            percentProgress.ForeColor = Color.Green;

            // Write the values
            if (mbShuffle)
            {
                Mirageboard.mirageboard_dataWriteCsv(currDir, sV, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (enemShuffle) Shop.putEldboxInShops(currDir, log);
            if (enemShuffle | rareShuffle | bossShuffle)
            {
                Enemy.mirageEncsWriteCsv(currDir, sV, log, enemShuffle, bossShuffle, rareShuffle);
                if (enemShuffle) currChecks++;
                if (rareShuffle) currChecks++;
                if (bossShuffle) currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (murkShuffle)
            {
                Murkrift.MurkriftShuffle(currDir, sV, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (enemShuffle) Mirageboard.modifyForEnemyRandoOnly(currDir);
            if (itemShuffle)
            {
                Item.TreasureShuffle(currDir, sV, log, libraShuffle, dataseedsShuffle, datajewelsShuffle, readeritemsShuffle);
                currChecks++;
                if (libraShuffle) currChecks++;
                if (dataseedsShuffle) currChecks++;
                if (datajewelsShuffle) currChecks++;
                if (readeritemsShuffle) currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (sizesShuffle)
            {
                Sizes.SizesShuffle(currDir, basepath, sV, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (quPrizesShuffle)
            {
                QuOrArenaPrizes.PrizesShuffle(currDir, sV, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (statShuffle)
            {
                Stats.RandomizeMirageStats(currDir, sV, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (t2AttackItemsBool)
            {
                Shop.RemoveT2AttackItems(currDir, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (transfigShuffle)
            {
                Transfig.TransfigMBShuffle(currDir, sV, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }

            // Apply post-QoL adjustments
            QoL.AddCSSkipToIntroCutscene(currDir, log);
            QoL.AddCSSkipsToEnding(currDir, log);
            QoL.AddCSSkipToGigans(currDir, log);
            if (doubleExpBool)
            {
                DoubleExpQoL.DoubleExpGil(currDir, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (battleSpeedBool)
            {
                QoL.IncreaseBattleSpeed(currDir, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (movementBool)
            {
                QoL.DoubleMovementSpeed(currDir, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }
            if (dialogueBool)
            {
                QoL.DecreaseDialogue(basepath, log);
                currChecks++;
                ChangeProgressStatus(currChecks, totalChecks, percentProgress);
            }

            // Second WoFFCshTool run
            ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "mirageboard_data.csv"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | murkShuffle ) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "enemy_group_list.csv"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | doubleExpBool | murkShuffle ) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "character_enemy_status_list.csv"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle | t2AttackItemsBool) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "shop_list.csv"));
            if (mbShuffle | enemShuffle | rareShuffle | bossShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "monster_place.csv"));
            if (sizesShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "character_resource_list.csv"));
            if (sizesShuffle | mbShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "command_ability_param.csv"));
            if (quPrizesShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "arena_reward_table_list.csv"));
            if (quPrizesShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "quest_data_sub_reward_table_list.csv"));
            if (statShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "character_list.csv"));
            if (battleSpeedBool) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "config_param.csv"));
            if (movementBool) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "map_move_param.csv"));
            if (enemShuffle) ConversionHelpers.ConvertToCsh(Path.Combine(currDir, "mirage_library_list.csv"));

            // In case other randomizer checkboxes are disabled, want to run on all of these
            copyBackAndDeleteOtherCshCsv(basepath, currDir, "/resource/finalizedCommon/mithril/system/csv/message/us", "/menu");
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
            copyBackAndDelete(basepath, currDir, "character_list");
            copyBackAndDelete(basepath, currDir, "config_param");
            copyBackAndDeleteOtherCshCsv(basepath, currDir, "/resource/finalizedCommon/mithril/map/csv", "/map_move_param");
            copyBackAndDelete(basepath, currDir, "mirage_library_list");

            log.SelectionFont = new Font(log.Font, FontStyle.Bold);
            log.AppendText("Finished generating seed " + sV + "!\n");
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;

        }
    }
}
