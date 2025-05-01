using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Logging;

namespace WOFFRandomizer.Dependencies
{
    internal class Transfig
    {
        private static List<List<string>> HandleDLCMirages(List<List<string>> mbdData, List<List<string>> DLCMBRows, List<string> skipMirages)
        {
            List<string> DLCMirages = ["7052", "7078", "7116", "7133", "7144", "7180", "8001", "8002", "8003", "8004", "8005"];

            foreach(List<string> row in mbdData)
            {
                if (skipMirages.Contains(row[4])) continue;

                // Remove Cactus Johnny from the accessible mirages, since that is technically not accessible DLC
                // Set values to make it inaccessible
                if (row[8] == "8000")
                {
                    row[7] = "12";
                    row[8] = "-1";
                    row[14] = "-1";
                    row[15] = "-1";
                    continue;
                }

                // if the condition unlock id is 8, store the row, then add it to the list
                if (row[14] == "8" | (DLCMirages.Contains(row[4]) && row[7] == "11"))
                {
                    List<string> rowToAdd = JsonSerializer.Deserialize<List<string>>(JsonSerializer.Serialize(row));
                    DLCMBRows.Add(rowToAdd);

                    // Afterwards, set the row to default values so that it's not caught up in the shuffle. Will reset the values after
                    row[7] = "13"; // Temporary, invalid category for easier finding later
                    row[8] = "-1";
                    row[14] = "-1";
                    row[15] = "-1";
                }

            }

            return DLCMBRows;
        }
        private static (List<List<string>>, List<List<string>>) CollectBaseData(List<List<string>> mbdData,
            List<List<string>> transfigRows, List<List<string>> mbUnlockRows, List<string> skipMirages, List<string> mbExclusionList)
        {
            foreach (List<string> row in mbdData)
            {
                if (skipMirages.Contains(row[4])) continue;
                if (row[9] != "-1") transfigRows.Add(row);
                else if (row[7] == "11")
                {
                    
                    // Remove link to mirageboard unlock, UNLESS it's on the exclusion list (otherwise the mirage is inaccessible)
                    if (!mbExclusionList.Contains(row[4])) {
                        row[7] = "12";
                        row[8] = "-1";
                        row[13] = "";
                        row[14] = "-1";
                        row[15] = "-1";
                        row[16] = "0";
                        row[17] = "-1";
                        continue;
                    }
                    mbUnlockRows.Add(row);
                }
            }
            return (transfigRows, mbUnlockRows);
        }

        private static List<List<string>> ReinsertShuffledData(List<List<string>> mbdData,
            List<List<string>> transfigRows, List<List<string>> mbUnlockRows, List<string> skipMirages, List<string> mbExclusionList, RichTextBox log)
        {
            int tRowIter = 0;
            int mUnlIter = 0;
            // Make a list of T1 (linked) and T2 (linked?) mirages. If the T3 mirage doesn't link to a T2 mirage, move to the next,
            // If the T2 doesn't link to a T1 mirage, move to the next. Hopefully this works out okay...
            List<string> T1Mirages = ["7000", "7002", "7004", "7008", "7011", "7015", "7018", "7022", "7025", "7029", "7031", "7034", "7036", "7039", 
                "7042", "7045", "7047", "7050", "7054", "7057", "7059", "7062", "7065", "7068", "7071", "7073", "7076", "7079", "7083", "7084", "7088", 
                "7090", "7092", "7095", "7098", "7101", "7104", "7111", "7115", "7117", "7119", "7121", "7123", "7125", "7128", "7131", "7135", "7137",
                "7139", "7142", "7145", "7147", "7149", "7151", "7153", "7156", "7159", "7163", "7165", "7170", "7177", "7183", "7185", "7187", "8020",
                "8023"];
            List<string> T2Mirages = ["7005", "7009", "7012", "7016", "7019", "7023", "7026", "7032", "7037", "7043", "7055", "7060", "7063", "7069",
                "7074", "7096", "7100", "7109", "7112", "7140", "7157", "7171", "7178", "8022", "8024"];

            List<string> TopLevelT3 = ["7006", "7007", "7010", "7013", "7014", "7017", "7020", "7021", "7024", "7028", "7033", "7038", "7044", "7056",
                "7061", "7064", "7070", "7075", "7097", "7102", "7110", "7114", "7141", "7158", "7173", "7179", "8021", "8027"];
            List<string> TopLevelT2 = ["7001", "7003", "7030", "7035", "7040", "7046", "7048", "7051", "7058", "7066", "7072", "7077", "7081", "7082", "7085",
                "7089", "7091", "7093", "7099", "7103", "7118", "7120", "7122", "7124", "7126", "7129", "7132", "7136", "7138", "7143", "7146", "7148", "7150",
                "7152", "7154", "7160", "7161", "7164", "7184", "7186", "7188", "8026"];
            List<string> T2MiragesWithTwoTransfigs = ["7005", "7012", "7019"];

            List<string> TrioBoardMirages = ["7065", "7067", "7145", "7147", "7166", "7167", "7170", "7177", "7181", "7182", "7196", "7197", "7198", "7199",
                "7200"];
            List<string> DuoBoardMirages = ["7047", "7049", "7051", "7053", "7084", "7087", "7092", "7094", "7105", "7106", "7107", "7108", "7125", "7127",
                "7153", "7155", "7187", "7189"];

            // Cactuar/Cactrot and Baby Paleberry/Baby Tonberry I'll have to handle later as they are linked to dlc mirages
            DuoBoardMirages.AddMany("7004", "7008", "7011", "7015", "7018", "7022", "7025", "7029", "7031", "7034", "7036", "7039", "7042", "7045",
              "7054", "7057", "7068", "7071", "7073", "7076", "7079", "7083", "7088", "7090", "7095", "7098", "7101", "7104", "7117", "7119",
              "7121", "7123", "7128", "7131", "7135", "7137", "7139", "7142", "7156", "7159", "7163", "7165");

            bool brokenT = false;
            // Each mirage has a group of 30 nodes, a mix of active and inactive nodes.
            // Just need to keep that in mind for this to work, I think
            try
            {
                for (int i = 60; i < mbdData.Count; i += 30)
                {
                    if (skipMirages.Contains(mbdData[i][4])) continue;
                    if (T1Mirages.Contains(mbdData[i][4])) continue;
                    // Check first row if it links to a previous mirage board, which is mbdData[i];
                    // If it has a set value besides -1, replace it with a randomized value
                    if (mbdData[i][9] != "-1")
                    {
                        if (TopLevelT3.Contains(mbdData[i][4]))
                        {
                            while (!(TopLevelT3.Contains(mbdData[i][4]) && T2Mirages.Contains(transfigRows[tRowIter][9])))
                            {
                                tRowIter++;
                                if (tRowIter == transfigRows.Count)
                                {
                                    brokenT = true;
                                    break;
                                }
                            }
                        }
                        else if (TopLevelT2.Contains(mbdData[i][4]))
                        {
                            while (!(TopLevelT2.Contains(mbdData[i][4]) && T1Mirages.Contains(transfigRows[tRowIter][9])))
                            {
                                tRowIter++;
                                if (tRowIter == transfigRows.Count)
                                {
                                    brokenT = true;
                                    break;
                                }
                            }
                        }
                        // Worst-case scenario: T1 --> T3
                        if (brokenT)
                        {
                            tRowIter = 0;
                            while (!(TopLevelT3.Contains(mbdData[i][4]) && T1Mirages.Contains(transfigRows[tRowIter][9])))
                            {
                                tRowIter++;
                            }
                        }


                        mbdData[i][9] = transfigRows[tRowIter][9];

                        // If it does have a set value, then it'll also have a node_category of 10 in i+27, i+28, or i+29
                        if (mbdData[i + 7][7] == "10") mbdData[i + 7][8] = transfigRows[tRowIter][9];
                        else if (mbdData[i + 9][7] == "10") mbdData[i + 9][8] = transfigRows[tRowIter][9];
                        else if (mbdData[i + 27][7] == "10") mbdData[i + 27][8] = transfigRows[tRowIter][9];
                        else if (mbdData[i + 28][7] == "10") mbdData[i + 28][8] = transfigRows[tRowIter][9];
                        else if (mbdData[i + 29][7] == "10") mbdData[i + 29][8] = transfigRows[tRowIter][9];

                        transfigRows.RemoveAt(tRowIter);
                        tRowIter = 0;
                        brokenT = false;
                    }
                }

                // After that, need to go through the data again, specifically the T1 and T2 lists, to make sure
                // that the transfig link ids are correct
                foreach (string mirage in T1Mirages)
                {
                    // Get the linkID
                    string linkID = mbdData[mbdData.FindIndex(x => x[4] == mirage && x[7] == "12")][6];
                    // Get the row that you need to put the linkID in
                    int rowID = mbdData.FindIndex(x => x[9] == mirage);
                    // Replace with the new linkID
                    mbdData[rowID][10] = linkID;
                }
                foreach (string mirage in T2Mirages)
                {
                    // Get the linkID
                    string linkID = mbdData[mbdData.FindIndex(x => x[4] == mirage && x[7] == "12")][6];
                    // Get the row that you need to put the linkID in
                    int rowID = mbdData.FindIndex(x => x[9] == mirage);
                    // Replace with the new linkID
                    mbdData[rowID][10] = linkID;
                }
                // Special case to check for three T2 elemental folks
                foreach (string mirage in T2MiragesWithTwoTransfigs)
                {
                    string linkID = mbdData[mbdData.FindLastIndex(x => x[4] == mirage && x[7] == "12")][6];
                    int rowID = mbdData.FindLastIndex(x => x[9] == mirage);
                    mbdData[rowID][10] = linkID;
                }

                // Write each row with node_category 11 with the shuffled mirageboard
                // Going to take them in pairs and pair them up. for mirages with two mirageboard unlocks, need to consider that
                bool duo = false, trio = false, broken = false;
                int doublesCounter = 0, triplesCounter = 0;
                while (mbUnlockRows.Count > 0)
                {
                    if (broken)
                    {
                        break;
                    }
                    // Get the pair of mirages
                    string mirageOne = mbUnlockRows[0][4];
                    if (DuoBoardMirages.Contains(mirageOne)) duo = true;
                    else trio = true;
                    string mirageTwo = mbUnlockRows[1][4];
                    // Case if duo board
                    int i = 0;
                    if (duo)
                    {
                        if (mbUnlockRows.Count < 2)
                        {
                            broken = true;
                            break;
                        }
                        while (!DuoBoardMirages.Contains(mirageTwo))
                        {
                            mirageTwo = mbUnlockRows[2 + i][4];
                            i++;
                            if (i == mbUnlockRows.Count - 2)
                            {
                                broken = true;
                                break;
                            }
                        }
                        // Find the first row in the mirageboard_data and write the shuffled mirage (mirageTwo) into it
                        int firstRowID = mbdData.FindIndex(x => x[4] == mirageOne && x[7] == "11");
                        if (firstRowID == -1)
                        {
                            break; // throw if invalid
                        }
                        mbdData[firstRowID][8] = mirageTwo;
                        // Get the second row and put the first mirage in there
                        int secondRowID = mbdData.FindIndex(x => x[4] == mirageTwo && x[7] == "11");
                        if (secondRowID == -1)
                        {
                            break; // throw if invalid
                        }
                        mbdData[secondRowID][8] = mirageOne;

                        // Delete the rows from mbUnlockRows to prevent duplication
                        mbUnlockRows.RemoveAt(0);
                        int mbID = mbUnlockRows.FindIndex(x => x[4] == mirageTwo);
                        if (mbID == -1)
                        {
                            break; // throw if invalid
                        }
                        mbUnlockRows.RemoveAt(mbID);
                        doublesCounter++;
                    }

                    // Case if trio board
                    if (trio)
                    {
                        if (mbUnlockRows.Count < 3)
                        {
                            broken = true;
                            break;
                        }
                        // Do stuff for trio boards...that'll be tough
                        while (!TrioBoardMirages.Contains(mirageTwo) && mirageTwo != mirageOne)
                        {
                            mirageTwo = mbUnlockRows[2 + i][4];
                            i++;
                            if (i == mbUnlockRows.Count - 2)
                            {
                                broken = true;
                                break;
                            }
                        }
                        string mirageThree = mbUnlockRows[2 + i][4];
                        while (!TrioBoardMirages.Contains(mirageThree) && mirageThree != mirageTwo && mirageThree != mirageOne)
                        {
                            mirageThree = mbUnlockRows[2 + i][4];
                            i++;
                            if (i == mbUnlockRows.Count - 2)
                            {
                                broken = true;
                                break;
                            }
                        }
                        // Handle this in three pairs
                        // First pair
                        // Find the first row in the mirageboard_data and write the shuffled mirage (mirageTwo) into it
                        int firstRowIDA = mbdData.FindIndex(x => x[4] == mirageOne && x[7] == "11");
                        if (firstRowIDA == -1)
                        {
                            break; // throw if invalid
                        }
                        mbdData[firstRowIDA][8] = mirageTwo;
                        // Get the second row and put the first mirage in there
                        int secondRowIDA = mbdData.FindIndex(x => x[4] == mirageTwo && x[7] == "11");
                        if (secondRowIDA == -1)
                        {
                            break; // throw if invalid
                        }
                        mbdData[secondRowIDA][8] = mirageOne;

                        // Second pair
                        int firstRowIDB = mbdData.FindLastIndex(x => x[4] == mirageOne && x[7] == "11");
                        if (firstRowIDB == -1)
                        {
                            break; // throw if invalid
                        }
                        mbdData[firstRowIDB][8] = mirageThree;
                        int thirdRowIDA = mbdData.FindIndex(x => x[4] == mirageThree && x[7] == "11");
                        if (thirdRowIDA == -1)
                        {
                            break; // throw if invalid
                        }
                        mbdData[thirdRowIDA][8] = mirageOne;

                        // Third pair
                        int secondRowIDB = mbdData.FindLastIndex(x => x[4] == mirageTwo && x[7] == "11");
                        if (secondRowIDB == -1)
                        {
                            break; // throw if invalid
                        }
                        mbdData[secondRowIDB][8] = mirageThree;
                        int thirdRowIDB = mbdData.FindLastIndex(x => x[4] == mirageThree && x[7] == "11");
                        if (thirdRowIDB == -1)
                        {
                            break; // throw if invalid
                        }
                        mbdData[thirdRowIDB][8] = mirageTwo;

                        // Delete the rows from mbUnlockRows to prevent duplication
                        mbUnlockRows.RemoveAt(0);
                        int mbIDA = mbUnlockRows.FindIndex(x => x[4] == mirageTwo);
                        if (mbIDA == -1)
                        {
                            break; // throw if invalid
                        }
                        mbUnlockRows.RemoveAt(mbIDA);
                        int mbIDB = mbUnlockRows.FindIndex(x => x[4] == mirageThree);
                        if (mbIDB == -1)
                        {
                            break; // throw if invalid
                        }
                        mbUnlockRows.RemoveAt(mbIDB);
                        // There's two sets of rows for each one, so I think I need to delete both to be accurate
                        mbIDA = mbUnlockRows.FindIndex(x => x[4] == mirageOne);
                        if (mbIDA != -1)
                        {
                            mbUnlockRows.RemoveAt(mbIDA);
                        }

                        mbIDB = mbUnlockRows.FindIndex(x => x[4] == mirageTwo);
                        if (mbIDB != -1)
                        {
                            mbUnlockRows.RemoveAt(mbIDB);
                        }

                        int mbIDC = mbUnlockRows.FindIndex(x => x[4] == mirageThree);
                        if (mbIDC != -1)
                        {
                            mbUnlockRows.RemoveAt(mbIDC);
                        }
                        triplesCounter++;
                    }
                    // Reset bools
                    duo = false; trio = false;

                }

                // After going through the trio MB unlocks, I want to remove duplicate entries, if there are any, since that is a possibility
                foreach (string mirage in TrioBoardMirages)
                {
                    List<List<string>> elevenRows = mbdData.FindAll(x => x[4] == mirage && x[7] == "11");
                    if (elevenRows.Count > 1) // If this passes, there should only be two. It should pass also, but putting a check is safe
                    {
                        // If the two rows are equal in mirage ID for mirageboard unlock, it's a duplicate. Remove one of the duplicates
                        // by rewriting the values of the latter
                        if (elevenRows[0][8] == elevenRows[1][8])
                        {
                            int index = mbdData.FindLastIndex(x => x[4] == mirage && x[7] == "11");

                            mbdData[index][7] = "12";
                            mbdData[index][8] = "-1";
                            mbdData[index][13] = "";
                            mbdData[index][14] = "-1";
                            mbdData[index][15] = "-1";
                            mbdData[index][16] = "0";
                            mbdData[index][17] = "-1";
                        }

                        // Check if a mirageboard is trying to unlock itself. Check for each one.
                        foreach (List<string> row in elevenRows)
                        {
                            if (row[4] == row[8])
                            {
                                row[7] = "12"; 
                                row[8] = "-1";
                                row[13] = "";
                                row[14] = "-1";
                                row[15] = "-1";
                                row[16] = "0";
                                row[17] = "-1";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.AppendText("Unsuccessful in shuffling transfigs/mirageboard unlocks...\n");
            }
            
            

            return mbdData;
        }

        private static List<List<string>> ReinsertDLCData(List<List<string>> mbdData, List<List<string>> DLCMBRows)
        {
            foreach(List<string> row in DLCMBRows)
            {
                int index = mbdData.FindIndex(x => x[4] == row[4] && x[7] == "13");
                mbdData[index] = row;
            }

            return mbdData;
        }

        private static void CreateTransfigMBLog(string currDir, List<List<string>> mbdData, List<string> skipMirages)
        {
            string logPath = Path.Combine(currDir, "logs", "transfig_log.txt");
            List<string> charsDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "enemy_names.txt"))];

            string toWrite = "";
            // First write the transfigurations
            toWrite += "Transfigs:" + Environment.NewLine;
            // Go through the data twice, first for transfigs then second time for mirageboard unlocks
            foreach (List<string> row in mbdData)
            {
                if (skipMirages.Contains(row[4])) continue;
                if (row[9] != "-1")
                {
                    // Get base mirage
                    int origMirageID = charsDB.FindIndex(x => x.Split("\t")[0] == row[9]);
                    string origMirageName = charsDB[origMirageID].Split("\t")[1];
                    // Get transfig mirage
                    int transMirageID = charsDB.FindIndex(x => x.Split("\t")[0] == row[4]);
                    string transMirageName = charsDB[transMirageID].Split("\t")[1];

                    toWrite += origMirageName + "-->" + transMirageName + Environment.NewLine;
                }
                
            }
            // Then the mirageboards
            toWrite += Environment.NewLine + "Mirageboard Unlocks:" + Environment.NewLine;
            foreach (List<string> row in mbdData)
            {
                if (skipMirages.Contains(row[4])) continue;
                if (row[7] == "11")
                {
                    // Get base mirage
                    int origMirageID = charsDB.FindIndex(x => x.Split("\t")[0] == row[4]);
                    string origMirageName = charsDB[origMirageID].Split("\t")[1];
                    // Get mirageboard unlock mirage
                    int mbunlockMirageID = charsDB.FindIndex(x => x.Split("\t")[0] == row[8]);
                    string mbunlockMirageName = charsDB[mbunlockMirageID].Split("\t")[1];

                    toWrite += origMirageName + "<->" + mbunlockMirageName + Environment.NewLine;
                }
            }

            File.WriteAllText(logPath, toWrite);
        }
        public static void TransfigMBShuffle(string currDir, string sV, RichTextBox log)
        {
            log.AppendText("Shuffling transfiguration....\n");

            string mbdPath = Path.Combine(currDir, "mirageboard_data.csv");
            List<List<string>> mbdData = CsvHandling.CsvReadData(mbdPath);

            // Exclusion list of mirages to ignore when iterating through data
            List<string> skipMirages = ["7027", "7041", "7080", "7086", "7113", "7134", "7162", "8006", "7172", "7191", "7192",
                "7193", "7194", "7195", "8000", "8010", "8017", "8018", "8025"];

            // Create an exclusion list for mirageboard unlocks, for normally inaccessible mirages other than mirageboard unlocks
            // will also exclude mirages that aren't part of transfigs
            List<string> mbExclusionList = ["7051", "7053", "7065", "7067", "7125", "7127", "7166", "7167",
                "7181", "7182", "7187", "7189", "7196", "7197", "7198", "7199", "7200", "7170", "7177", "7145", "7147",
                "7107", "7108", "7105", "7106", "7049", "7087", "7084", "7047", "7094", "7092", "7153",
                "7155"];
            // Coliseum extra mirages that keep screwing up my mirageboards until I can figure it out, so will put in separate list here
            //mbExclusionList.AddMany("7052", "7078", "7116", "7130", "7133", "7144", "7174", "7180", "8001", "8002", "8003", "8004", "8005", "8007");
            // Cactuar/Cactrot and Baby Paleberry/Baby Tonberry I'll have to handle later as they are linked to dlc mirages
            mbExclusionList.AddMany("7004", "7008", "7011", "7015", "7018", "7022", "7025", "7029", "7031", "7034", "7036", "7039", "7042", "7045",
              "7054", "7057", "7068", "7071", "7073", "7076", "7079", "7083", "7088", "7090", "7095", "7098", "7101", "7104", "7117", "7119",
              "7121", "7123", "7128", "7131", "7135", "7137", "7139", "7142", "7156", "7159", "7163", "7165");

            // Need to find rows where transfig and mirageboard unlocks are, separately,
            // then need to shuffle them.
            List<List<string>> transfigRows = new List<List<string>>();
            List<List<string>> mbUnlockRows = new List<List<string>>();

            // Keep a list of DLC mirageboard unlocks that go one-way to preserve that data and also make it null for now
            List<List<string>> DLCMBRows = new List<List<string>>();

            DLCMBRows = HandleDLCMirages(mbdData, DLCMBRows, skipMirages);

            (transfigRows, mbUnlockRows) = CollectBaseData(mbdData, transfigRows, mbUnlockRows, skipMirages, mbExclusionList);

            // Randomize each list
            transfigRows.Shuffle(Shuffle.ConsistentStringHash(sV));
            mbUnlockRows.Shuffle(Shuffle.ConsistentStringHash(sV));

            // Make deep copies of these Lists, as these are shallow copies
            List<List<string>> deepTRows = JsonSerializer.Deserialize<List<List<string>>>(JsonSerializer.Serialize(transfigRows));
            List<List<string>> deepMRows = JsonSerializer.Deserialize<List<List<string>>>(JsonSerializer.Serialize(mbUnlockRows));

            mbdData = ReinsertShuffledData(mbdData, deepTRows, deepMRows, skipMirages, mbExclusionList, log);

            mbdData = ReinsertDLCData(mbdData, DLCMBRows);

            CreateTransfigMBLog(currDir, mbdData, skipMirages);

            CsvHandling.CsvWriteDataAddHeadRow(mbdPath, mbdData, 20);
        }
    }
}
