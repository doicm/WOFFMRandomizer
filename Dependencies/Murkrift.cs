using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace WOFFRandomizer.Dependencies
{
    internal class Murkrift
    {
        private static (List<List<string>>, List<List<string>>) GetEglDataAndlGEXPData(string eglPath, string ceslPath, List<string> eglMurkIDs)
        {
            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);
            List<List<string>> lGEXPData = new List<List<string>>();

            List<List<string>> eglMurkData = new List<List<string>>();
            List<string> ceslIDs = new List<string>();

            foreach (var row in eglData)
            {
                // If it's a murkrift row, add it to the murkrift data
                if (eglMurkIDs.Contains(row[0]))
                {
                    eglMurkData.Add(row[3..]);
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
            //List<List<string>> murkGroupings = [["316", "317"], ["606", "607", "608", "609", "610", "611"], ["1138", "1139"], ["1140", "1141", "1142"]];
            List<string> skipMurkIDs = ["317", "607", "608", "609", "610", "611", "1139", "1141", "1142"]; // These are duplicate entries in terms of lGEXP
            List<List<string>> ceslData = CsvHandling.CsvReadData(ceslPath);
            foreach (var row in ceslData)
            {
                if (ceslIDs.Contains(row[0]) && !skipMurkIDs.Contains(row[0]))
                {
                    List<string> lGEXPRow = [row[3], row[79], row[80], row[81], row[82]];
                    lGEXPData.Add(lGEXPRow);
                }
            }

            return (eglMurkData, lGEXPData);
        }

        private static List<(string, List<string>)> InsertEglData(string eglPath, List<List<string>> eglMurkData, List<string> eglMurkIDs, 
            List<List<string>> lGEXPData)
        {
            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);
            int eglMurkDataIter = 0;
            List<(string, List<string>)> shuffledCeslIDsWithlGEXPData = new List<(string, List<string>)> ();
            List<string> shuffledCeslIDs = new List<string>();
            List<string> skipMurkIDs = ["317", "607", "608", "609", "610", "611", "1139", "1141", "1142"]; // These are duplicate entries in terms of lGEXP

            int lGEXPIter = 0;
            foreach (var row in eglData)
            {
                if (eglMurkIDs.Contains(row[0]))
                {
                    int i = 0;
                    while (i < row.Count - 3)
                    {
                        row[i + 3] = eglMurkData[eglMurkDataIter][i];
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
                        if (ceslID != "-1" && !shuffledCeslIDs.Contains(ceslID) && !skipMurkIDs.Contains(ceslID))
                        {
                            shuffledCeslIDs.Add(ceslID);
                            shuffledCeslIDsWithlGEXPData.Add((ceslID, lGEXPData[lGEXPIter++]));
                        }
                        j++;
                    }
                    eglMurkDataIter++;
                }
            }
            CsvHandling.CsvWriteDataAddHeadRow(eglPath, eglData, 79);

            return shuffledCeslIDsWithlGEXPData;
        }

        private static void InsertlGEXPData(string ceslPath, List<(string, List<string>)> pairedCeslIDsWithlGEXP)
        {
            List<List<string>> ceslData = CsvHandling.CsvReadData(ceslPath);
            // If ID matches in a grouping, apply same lGEXPData
            List<List<string>> murkGroupings = [["316", "317"], ["1138", "1139"], ["1140", "1141", "1142"]];

            // Pair the groupings with the lGEXPdata
            foreach (var group in murkGroupings)
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
                    row[3] = lGEXP[0];
                    row[79] = lGEXP[1];
                    row[80] = lGEXP[2];
                    row[81] = lGEXP[3];
                    row[82] = lGEXP[4];
                }
            }
            CsvHandling.CsvWriteDataAddHeadRow(ceslPath, ceslData, 84);
        }

        private static void AppendToMonsterLog(string currDir, List<string> eglMurkIDs, string eglPath)
        {
            string logPath = Path.Combine(currDir, "logs", "monster_log.txt");
            List<string> charsDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "enemy_names.txt"))];

            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);

            string currentText = File.ReadAllText(logPath);

            if (currentText.Count() > 0) currentText += "---\n";
            currentText += "Murkrifts:\n";
            foreach (var row in eglData)
            {
                if (eglMurkIDs.Contains(row[0]))
                {
                    // Find name
                    int charID = charsDB.FindIndex(x => x.Split("\t")[0] == row[4]);
                    string charName = charsDB[charID].Split("\t")[1];
                    // Find area/chapter
                    string areaName = "";
                    if (row[1].Substring(3,1) == "c")
                    {
                        areaName = "Chapter " + Int32.Parse(row[1].Substring(4, 2));
                    }
                    else if (row[1].Substring(3,5) == "world")
                    {
                        areaName = "Airship " + row[1].Substring(11);
                    }
                    else
                    {
                        // DLC is individualized, so need an specific area for each one
                        if (row[1] == "DC_reserve_001")
                        {
                            areaName = "Ice Region";
                        }
                        else if (row[1] == "DC_reserve_002")
                        {
                            areaName = "Rainbow Shore";
                        }
                        else if (row[1] == "DC_reserve_003")
                        {
                            areaName = "Phantom Sands";
                        }
                        else if (row[1] == "DC_reserve_004")
                        {
                            areaName = "Big Bridge";
                        }
                        else if (row[1] == "DC_reserve_005")
                        {
                            areaName = "Library of the Ancients";
                        }
                        else if (row[1] == "DC_reserve_006")
                        {
                            areaName = "Airship 7 (Postgame+)";
                        }
                    }

                    string toAdd = areaName + ": " + charName + Environment.NewLine;
                    currentText += toAdd;
                }
            }
            File.WriteAllText(logPath, currentText);



        }
        public static void MurkriftShuffle(string currDir, string sV, RichTextBox log)
        {
            log.AppendText("Shuffling murkrifts....\n");

            string eglPath = Path.Combine(currDir, "enemy_group_list.csv");
            string ceslPath = Path.Combine(currDir, "character_enemy_status_list.csv");

            // Get the list of murkrift fights that need to be shuffled
            // Behemoth fight at beginning is excluded
            // Had to remove "815", "816", "817", "818", "819", "820" because they share ceslIDs and can affect other murkrifts
            List<string> eglMurkIDs = ["789", "790", "791", "793", "795", "797", "798", "799", "800", "801", "803", "825", "826", "827", "828", "829", "830"];

            // First, I'll get the EGL data, shuffle it, then place the EGL data back in the slots
            // After that, I'll iterate through each ceslID in the egl data, assign it the level data,
            // then iterate through the cesl data and assign it the level, exp, and gil...I'll figure it out
            List<List<string>> eglMurkData = new List<List<string>>();
            List<List<string>> lGEXPData = new List<List<string>>();

            // Doing both at once to simplify the process
            (eglMurkData, lGEXPData) = GetEglDataAndlGEXPData(eglPath, ceslPath, eglMurkIDs);

            // Next, I need to shuffle the egl rows
            eglMurkData.Shuffle(Shuffle.ConsistentStringHash(sV));

            // Then I reinsert the egl data back into the file
            List<(string, List<string>)> pairedCeslIDsWithlGEXP= InsertEglData(eglPath, eglMurkData, eglMurkIDs, lGEXPData);

            // I also modify the cesl data to reflect the correct lGEXP
            InsertlGEXPData(ceslPath, pairedCeslIDsWithlGEXP);

            // Append to the monster log with levels for each murkrift. Try to get locations, if possible
            AppendToMonsterLog(currDir, eglMurkIDs, eglPath);

        }
    }
}
