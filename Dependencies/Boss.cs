using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WOFFRandomizer.Dependencies
{
    internal class Boss
    {
        private static void ModifyYunaAndValeforToBeLoseable(string eglPath)
        {
            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);
            // Find row containing 05_EV_ユウナ
            int i = eglData.FindIndex(x => x.Contains("05_EV_ユウナ"));
            eglData[i][67] = "1"; // Set value of "losing is fine" condition to "game over will happen" condition
            
            CsvHandling.CsvWriteDataAddHeadRow(eglPath, eglData, 79);
        }
        private static (List<List<string>>, List<List<string>>) GetEglDataAndlGEXPData(string eglPath, string ceslPath, List<string> eglBossIDs)
        {
            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);
            List<List<string>> lGEXPData = new List<List<string>>();

            List<List<string>> eglBossData = new List<List<string>>();
            List<string> ceslIDs = new List<string>();

            foreach (var row in eglData)
            {
                // If it's a boss row, add it to the boss data
                if (eglBossIDs.Contains(row[0]))
                {
                    eglBossData.Add(row[3..]);
                    // Also get the ceslIDs from the row
                    int i = 0;
                    int j = 6;
                    string ceslID;
                    while (i < 6)
                    {
                        ceslID = row[j + (i * 4)];
                        if (ceslID != "-1" && !ceslIDs.Contains(ceslID))
                        {
                            ceslIDs.Add(ceslID);
                        }
                        i++;
                    }
                }
            }

            // Now get the lGEXPData. Make groupings of similar enemies
            List<string> skipBossIDs = ["151", "164", "185", "272", "301", "323", "324", "327", "328", "350",
                "351", "352", "354"]; // These are duplicate entries in terms of lGEXP
            List<List<string>> ceslData = CsvHandling.CsvReadData(ceslPath);
            foreach (var row in ceslData)
            {
                if (ceslIDs.Contains(row[0]) && !skipBossIDs.Contains(row[0]))
                {
                    List<string> lGEXPRow = [row[3], row[79], row[80], row[81], row[82]];
                    // Need to make exception for Vivi fight, since that gives no exp/gil normally and I want replacement
                    // to give exp/gil
                    if (row[0] == "196") lGEXPRow = [row[3], "4000", "1200", "46620", "1800"];
                    else if (row[0] == "271") lGEXPRow = [row[3], "20500", "1700", "150000", "1850"];
                    lGEXPData.Add(lGEXPRow);
                }
            }

            return (eglBossData, lGEXPData);
        }

        private static List<(string, List<string>)> InsertEglData(string eglPath, List<List<string>> eglBossData, List<string> eglBossIDs,
            List<List<string>> lGEXPData)
        {
            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);
            int eglBossDataIter = 0;
            List<(string, List<string>)> shuffledCeslIDsWithlGEXPData = new List<(string, List<string>)>();
            List<string> shuffledCeslIDs = new List<string>();
            List<string> skipBossIDs = ["151", "164", "185", "272", "301", "323", "324", "327", "328", "350",
                "351", "352", "354"]; // These are duplicate entries in terms of lGEXP

            int lGEXPIter = 0;
            foreach (var row in eglData)
            {
                if (eglBossIDs.Contains(row[0]))
                {
                    int i = 0;
                    while (i < row.Count - 3)
                    {
                        row[i + 3] = eglBossData[eglBossDataIter][i];
                        i++;
                    }
                    // Get the shuffledCeslIDs to take back and use for the next part
                    // Also get the ceslIDs from the row
                    int j = 0;
                    int k = 6;
                    string ceslID;
                    while (j < 6)
                    {
                        ceslID = row[k + (j * 4)];
                        if (ceslID != "-1" && !shuffledCeslIDs.Contains(ceslID) && !skipBossIDs.Contains(ceslID))
                        {
                            shuffledCeslIDs.Add(ceslID);
                            shuffledCeslIDsWithlGEXPData.Add((ceslID, lGEXPData[lGEXPIter++]));
                        }
                        j++;
                    }
                    eglBossDataIter++;
                }
            }
            CsvHandling.CsvWriteDataAddHeadRow(eglPath, eglData, 79);

            return shuffledCeslIDsWithlGEXPData;
        }

        private static void InsertlGEXPData(string ceslPath, List<(string, List<string>)> pairedCeslIDsWithlGEXP)
        {
            List<List<string>> ceslData = CsvHandling.CsvReadData(ceslPath);
            // If ID matches in a grouping, apply same lGEXPData
            List<List<string>> bossGroupings = [["152", "151"],["165", "164"], ["184", "185"], ["271", "272"], ["300", "301"], ["322", "323", "324"],
                ["326", "327", "328"], ["349", "350", "351"], ["353", "352"], ["355", "354"]];

            // Make two lists, one where exp/gil is cut by 1/4 (for adds in fights) and one where exp/gil is cut by 1/3 (for Kupirates). TODO
            List<string> exceptionListQuart = ["185", "301", "327", "328"];
            List<string> exceptionListThird = ["196"];

            // Pair the groupings with the lGEXPdata
            foreach (var group in bossGroupings)
            {
                string standard = group[0];
                List<string> standardlGEXP = pairedCeslIDsWithlGEXP.Find(x => x.Item1 == group[0]).Item2;
                for (int i = 1; i < group.Count; i++)
                {
                    pairedCeslIDsWithlGEXP.Add((group[i], standardlGEXP));
                }
            }

            // Sort the ceslIDsWithlGEXP data by ceslID so that I only have to go through the data once
            pairedCeslIDsWithlGEXP.Sort((x, y) => Int32.Parse(x.Item1).CompareTo(Int32.Parse(y.Item1)));
            int iter = 0;
            foreach (var row in ceslData)
            {
                string ceslID = pairedCeslIDsWithlGEXP.Find(x => x.Item1 == row[0]).Item1;
                if (ceslID != "-1" && ceslID != null)
                {
                    if (exceptionListQuart.Contains(ceslID))
                    {
                        List<string> lGEXP = pairedCeslIDsWithlGEXP[iter++].Item2;
                        row[3] = lGEXP[0];
                        row[79] = (Int32.Parse(lGEXP[1]) / 4).ToString();
                        row[80] = (Int32.Parse(lGEXP[2]) / 4).ToString();
                        row[81] = (Int32.Parse(lGEXP[3]) / 4).ToString();
                        row[82] = (Int32.Parse(lGEXP[4]) / 4).ToString();
                    }
                    else if (exceptionListThird.Contains(ceslID))
                    {
                        List<string> lGEXP = pairedCeslIDsWithlGEXP[iter++].Item2;
                        row[3] = lGEXP[0];
                        row[79] = (Int32.Parse(lGEXP[1]) / 3).ToString();
                        row[80] = (Int32.Parse(lGEXP[2]) / 3).ToString();
                        row[81] = (Int32.Parse(lGEXP[3]) / 3).ToString();
                        row[82] = (Int32.Parse(lGEXP[4]) / 3).ToString();
                    }
                    else
                    {
                        List<string> lGEXP = pairedCeslIDsWithlGEXP[iter++].Item2;
                        row[3] = lGEXP[0];
                        row[79] = lGEXP[1];
                        row[80] = lGEXP[2];
                        row[81] = lGEXP[3];
                        row[82] = lGEXP[4];
                    }
                        
                }
            }
            CsvHandling.CsvWriteData(ceslPath, ceslData);
        }

        private static void AppendToMonsterLog(string currDir, List<string> eglBossIDs, string eglPath)
        {
            string logPath = Path.Combine(currDir, "logs", "monster_log.txt");
            List<string> charsDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "enemy_names.txt"))];

            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);

            string currentText = File.ReadAllText(logPath);

            bool broken = false;
            int ebIDIter = 0;

            if (currentText.Count() > 0) currentText += "---\n";
            currentText += "Bosses:\n";
            foreach (var row in eglData)
            {
                if (broken) break;
                if (eglBossIDs.Contains(row[0]))
                {
                    int j = 4;
                    int k = 0;
                    while (k <= 6)
                    {
                        // account for duplicates in same row
                        if (row[j + k * 4] != "-1" && row[j + k * 4] != row[k * 4])
                        {
                            if (int.Parse(row[j + k * 4]) > 1)
                            {
                                // Do the searching and writing here.
                                string chapName = row[1];
                                if (chapName.Substring(3, 1) == "c") chapName = "Chapter " + Int32.Parse(chapName.Substring(4, 2));
                                else chapName = "Postscript";
                                int charID = charsDB.FindIndex(x => x.Split("\t")[0] == row[j + k * 4]);
                                string charName = charsDB[charID].Split("\t")[1];
                                string toAdd = chapName + ": " + charName + Environment.NewLine;
                                currentText += toAdd;
                            }
                        }
                        k++;
                    }
                    ebIDIter++;
                    if (ebIDIter == eglBossIDs.Count())
                    {
                        broken = true;
                    }
                }
            }
            File.WriteAllText(logPath, currentText);
        }

        public static void ModifyBosses(string sV, RichTextBox log, string currDir)
        {
            log.AppendText("Shuffling bosses...\n");

            string eglPath = Path.Combine(currDir, "enemy_group_list.csv");
            string ceslPath = Path.Combine(currDir, "character_enemy_status_list.csv");
            //list of bosses based on EGL value
            // excluding elemental trio, due to missables
            // excluding chapter 14, since that's a mess
            // excluding order of the circle fights. they're special :)
            List<string> eglBossIDs = ["569", "571", "573", "577", "578", "592", "598", "601", "607", "615",
                "618", "623", "626", "627", "629", "634", "635", "643", "692"];
            List<List<string>> eglBossData = new List<List<string>>();
            List<List<string>> lGEXPData = new List<List<string>>();

            // Doing both at once to simplify the process
            (eglBossData, lGEXPData) = GetEglDataAndlGEXPData(eglPath, ceslPath, eglBossIDs);

            // Next, I need to shuffle the egl rows
            eglBossData.Shuffle(Shuffle.ConsistentStringHash(sV));
            // Vivi in index 4 of eglBossData, which is the Kupirate fight, causes issues. Need to reshuffle if that happens
            int i = 0;
            while (eglBossData[4][1] == "1011")
            {
                eglBossData.Shuffle(Shuffle.ConsistentStringHash(sV + i.ToString()));
                i++;
            }
            

            // Then I reinsert the egl data back into the file
            List<(string, List<string>)> pairedCeslIDsWithlGEXP = InsertEglData(eglPath, eglBossData, eglBossIDs, lGEXPData);

            // I also modify the cesl data to reflect the correct lGEXP
            InsertlGEXPData(ceslPath, pairedCeslIDsWithlGEXP);

            // Make unloseable fight loseable (Yuna & Valefor)
            ModifyYunaAndValeforToBeLoseable(eglPath);

            // Append to the monster log with levels for each murkrift. Try to get locations, if possible
            AppendToMonsterLog(currDir, eglBossIDs, eglPath);

        }
    }
}
