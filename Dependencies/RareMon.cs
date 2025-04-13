using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WOFFRandomizer.Dependencies
{
    internal class RareMon
    {
        private static (List<List<string>>, List<List<string>>) GetEglDataAndlGEXPData(string eglPath, string ceslPath, List<string> eglRareIDs)
        {
            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);
            List<List<string>> lGEXPData = new List<List<string>>();

            List<List<string>> eglRareData = new List<List<string>>();
            List<string> ceslIDs = new List<string>();

            foreach (var row in eglData)
            {
                // If it's a rare monster row, add it to the rare monster data
                if (eglRareIDs.Contains(row[0]))
                {
                    eglRareData.Add(row[3..]);
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
            List<string> skipRareIDs = ["237", "575"]; // These are duplicate entries in terms of lGEXP
            List<List<string>> ceslData = CsvHandling.CsvReadData(ceslPath);
            foreach (var row in ceslData)
            {
                if (ceslIDs.Contains(row[0]) && !skipRareIDs.Contains(row[0]))
                {
                    List<string> lGEXPRow = [row[3], row[79], row[80], row[81], row[82]];
                    lGEXPData.Add(lGEXPRow);
                }
            }

            return (eglRareData, lGEXPData);
        }

        private static List<(string, List<string>)> InsertEglData(string eglPath, List<List<string>> eglRareData, List<string> eglRareIDs,
            List<List<string>> lGEXPData)
        {
            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);
            int eglRareDataIter = 0;
            List<(string, List<string>)> shuffledCeslIDsWithlGEXPData = new List<(string, List<string>)>();
            List<string> shuffledCeslIDs = new List<string>();
            List<string> skipRareIDs = ["237", "575"]; // These are duplicate entries in terms of lGEXP

            int lGEXPIter = 0;
            foreach (var row in eglData)
            {
                if (eglRareIDs.Contains(row[0]))
                {
                    int i = 0;
                    while (i < row.Count - 3)
                    {
                        row[i + 3] = eglRareData[eglRareDataIter][i];
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
                        if (ceslID != "-1" && !shuffledCeslIDs.Contains(ceslID) && !skipRareIDs.Contains(ceslID))
                        {
                            shuffledCeslIDs.Add(ceslID);
                            shuffledCeslIDsWithlGEXPData.Add((ceslID, lGEXPData[lGEXPIter++]));
                        }
                        j++;
                    }
                    eglRareDataIter++;
                }
            }
            CsvHandling.CsvWriteData(eglPath, eglData);

            return shuffledCeslIDsWithlGEXPData;
        }

        private static void InsertlGEXPData(string ceslPath, List<(string, List<string>)> pairedCeslIDsWithlGEXP)
        {
            List<List<string>> ceslData = CsvHandling.CsvReadData(ceslPath);
            // If ID matches in a grouping, apply same lGEXPData
            List<List<string>> rareGroupings = [["236", "237"], ["574", "575"]];
            // Discrepancy between Malboro Menace and Princess Flan is pretty high (50 to 38). Went with lower value for accessibility.

            // Pair the groupings with the lGEXPdata
            foreach (var group in rareGroupings)
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
                    List<string> lGEXP = pairedCeslIDsWithlGEXP[iter++].Item2;
                    if (lGEXP[0] == "28") row[3] = "18"; // Set Dragon Scars rare monster level to set level rather than 28 for accessible situations
                    else row[3] = lGEXP[0];
                    row[79] = lGEXP[1];
                    row[80] = lGEXP[2];
                    row[81] = lGEXP[3];
                    row[82] = lGEXP[4];
                }
            }
            CsvHandling.CsvWriteData(ceslPath, ceslData);
        }

        private static void AppendToMonsterLog(string currDir, List<string> eglRareIDs, string eglPath)
        {
            string logPath = Path.Combine(currDir, "logs", "monster_log.txt");
            List<string> charsDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "enemy_names.txt"))];

            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);

            string currentText = File.ReadAllText(logPath);

            bool broken = false;
            int ebIDIter = 0;

            if (currentText.Count() > 0) currentText += "---\n";
            currentText += "Rare Mirages:\n";
            foreach (var row in eglData)
            {
                if (broken) break;
                if (eglRareIDs.Contains(row[0]))
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
                                chapName = "Chapter " + Int32.Parse(chapName.Substring(4, 2));
                                int charID = charsDB.FindIndex(x => x.Split("\t")[0] == row[j + k * 4]);
                                string charName = charsDB[charID].Split("\t")[1];
                                string toAdd = chapName + ": " + charName + Environment.NewLine;
                                currentText += toAdd;
                            }
                        }
                        k++;
                    }
                    ebIDIter++;
                    if (ebIDIter == eglRareIDs.Count())
                    {
                        broken = true;
                    }
                }
            }
            File.WriteAllText(logPath, currentText);
        }

        public static void ShuffleRareMonsters(string sV, RichTextBox log, string currDir)
        {
            log.AppendText("Shuffling rare monsters...\n");

            string eglPath = Path.Combine(currDir, "enemy_group_list.csv");
            string ceslPath = Path.Combine(currDir, "character_enemy_status_list.csv");
            //list of rare monsters based on EGL value
            List<string> eglRareIDs = ["570", "572", "576", "582", "595", "599", "606", "613", "617", "620", "624", "636", "642", "645"];
            List<List<string>> eglRareData = new List<List<string>>();
            List<List<string>> lGEXPData = new List<List<string>>();

            // Doing both at once to simplify the process
            (eglRareData, lGEXPData) = GetEglDataAndlGEXPData(eglPath, ceslPath, eglRareIDs);

            // Next, I need to shuffle the egl rows
            eglRareData.Shuffle(Shuffle.ConsistentStringHash(sV));

            // Then I reinsert the egl data back into the file
            List<(string, List<string>)> pairedCeslIDsWithlGEXP = InsertEglData(eglPath, eglRareData, eglRareIDs, lGEXPData);

            // I also modify the cesl data to reflect the correct lGEXP
            InsertlGEXPData(ceslPath, pairedCeslIDsWithlGEXP);

            // Append to the monster log with levels for each murkrift. Try to get locations, if possible
            AppendToMonsterLog(currDir, eglRareIDs, eglPath);
        }
    }
}
