using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

// TODO
// Identify each ability and don't make more generic abilities exclusive, possibly

namespace WOFFRandomizer.Dependencies
{

    
    internal class Mirageboard
    {
        private static void enemyRandoSetMethod(List<List<string>> listListCsv)
        {
            // Critical values are the following for Tama and Sylph:
            var enemyRandoSpecialMirages = new List<(string, string)>
            {
                ("68", "2000"),
                ("4749", "2001"),
                ("4751", "2006")
            };

            int eRSMIter = 0;
            // navigate to each row and place the values back in
            foreach (List<string> row in listListCsv)
            {
                int i = listListCsv.FindIndex(str => str == row);

                if (row[0] == enemyRandoSpecialMirages[eRSMIter].Item1)
                {
                    listListCsv[i][7] = "8";
                    listListCsv[i][8] = enemyRandoSpecialMirages[eRSMIter].Item2;
                    listListCsv[i][11] = "1";
                    eRSMIter += 1;
                    if (eRSMIter == enemyRandoSpecialMirages.Count) break;
                }
            }

        }

        // May need a revisit
        public static void modifyForEnemyRandoOnly(string currDir)
        {
            // Get filename to edit
            string csvfilename = Path.Combine(currDir, "mirageboard_data.csv");

            // Put all lines of the file into a list to edit
            
            List<List<string>> listListCsv = CsvHandling.CsvReadData(csvfilename);
            enemyRandoSetMethod(listListCsv);

            // Write over the file
            CsvHandling.CsvWriteDataAddHeadRow(csvfilename, listListCsv, 20);
        }

        private static bool ignoreRows(List<string> row)
        {
            List<string> skipRows = new List<string>();
            List<string> skipAbilities = new List<string>();
            List<string> skipEnemyRandoRows = new List<string>();
            List<string> skipMirages = new List<string>();
            // ch 1 chocochick must learn stroll (992)
            // pre ch 6 black nakk must learn sizzle (1262)
            // pre ch 6 mythril giant must learn smash (2251)
            // ch 8 floating eye must learn flutter (4261)
            // pre ch 11 quachacho must learn chill (1592)
            // pre ch 15 searcher must learn zap (5402)
            // Skip to get Tama's Foxfire in case you don't have a fire solution for Sharqual's weight scale issue in Saronia Harbor
            // Skip Bahamut*'s Megaflare. It's too powerful to be on anything but Bahamut*.
            skipRows.AddMany("62", "992", "1262", "2251", "4261", "1592", "5402", "6596");
            // For use with skipping smash and joyride, as well as other possible abilities
            skipAbilities.AddMany("2003", "2010");
            // For use with enemy randos
            skipEnemyRandoRows.AddMany("68", "4749", "4751");
            // For use with skipping specific mirages, mostly those that don't actually exist as mirages
            skipMirages.AddMany("7027", "7041", "7080", "7086", "7113", "7134", "7162", "8006", "7172", "7191", "7192", "7193", "7194", "7195", "8010",
                "8017", "8018", "8025");

            // if the category is not in between 1-9, excluding 6 (prismariums), skip it
            // categories: 1) abilities, 2) ?, 3) passives, 4) blank space, 5) mirajewel, 7) unique abilities, 8) joyride/stroll/etc, 9) unique passives
            int category = int.Parse(row[7]);
            if (category < 1 | category > 9 | category == 6)
            {
                return true;
            }
            // if it's in the skipRows column (for completion's sake), skip it
            else if (skipRows.Contains(row[0]))
            {
                return true;
            }
            // if the 8 column has smash or joyride, skip it. smash softlocks when used by another, and joyride belongs to the joyriders
            else if (skipAbilities.Contains(row[8]))
            {
                return true;
            }
            else if (skipMirages.Contains(row[4]))
            {
                return true;
            }

                return false;
        }

        private static void WriteToMirageboardLog(List<List<string>> listListCsv, string currDir)
        {
            // Convert what's read in the file to a list to read in of the database files
            List<string> abilitiesDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "abilities.txt"))];
            List<string> charsDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "enemy_names.txt"))];
            List<string> mirajewelsDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "mirajewels.txt"))];

            List<string> skipMirages = ["7027", "7041", "7080", "7086", "7113", "7134", "7162", "8006", "7172", "7191", "7192", 
                "7193", "7194", "7195", "8010", "8012", "8017", "8018", "8025"];

            using (var sw = new StreamWriter(Path.Combine(currDir, "logs", "mirageboard_log.txt")))
            {
                foreach (var row in listListCsv)
                {
                    if (skipMirages.Contains(row[4])) continue;
                    // Exclude rows that don't have ability information.
                    // 7th column has ability category. Needs to be between 1 and 9 to be read
                    // Don't read first 59 columns, as they are Lann and Reynn.
                    int category = Int32.Parse(row[7]);
                    if (Int32.Parse(row[0]) > 59 && category > 0 && category < 10)
                    {
                        // Get mirage name
                        int charID = charsDB.FindIndex(x => x.Split("\t")[0] == row[4]);
                        string charName;
                        if (charID == -1) charName = row[4];
                        else charName = charsDB[charID].Split("\t")[1];
                        string categoryString = "";
                        if (category == 4) categoryString = "Blank Space";
                        else if (category == 5)
                        {
                            string searchString = row[8];
                            while (searchString.Length < 3)
                            {
                                searchString = searchString.PadLeft(searchString.Length + 1, '0');
                            }
                            searchString = "Stone" + searchString;
                            int mirajewelID = mirajewelsDB.FindIndex(x => x.Split("\t")[0] == searchString);
                            categoryString = mirajewelsDB[mirajewelID].Split("\t")[1];
                        }
                        else if (category == 6) categoryString = "Prism";
                        else
                        {
                            int abilityID = abilitiesDB.FindIndex(x => x.Split("\t")[0] == row[8]);
                            categoryString = abilitiesDB[abilityID].Split("\t")[1]; // do stuff later for ability string
                        }

                            sw.WriteLine(charName + ": " + categoryString + " (" + row[11] + ")");
                    }
                }
            }
        }
        private static List<List<string>> modifyBoards(List<List<string>> csvData, string seedvalue, string currDir)
        {
            // statTuples to store each set of three datas that will be randomized
            var statTuples = new List<Tuple<string, string, string>>();
            int startData = 60; // start data, not including Lann and Reynn
            int endData = 7620; // max count of data
            // iterate through each used node and extract the needed values that will be randomized
            // Also convert each row string to a row list
            foreach (List<string> row in csvData)
            {

                int i = csvData.FindIndex(str => str == row);
                if (i >= startData && i <= endData)
                {
                    if (ignoreRows(csvData[i]))
                    {
                        continue;
                    }
                    statTuples.Add(new Tuple<string, string, string>(row[7], row[8], row[11]));
                }
            }

            // randomize the order of the nodes
            statTuples.Shuffle(Shuffle.ConsistentStringHash(seedvalue));

            int sTIter = 0;
            // iterate through each row and place the values back in
            foreach (List<string> row in csvData)
            {
                int i = csvData.FindIndex(str => str == row);
                if (i >= startData && i <= endData)
                {
                    if (ignoreRows(csvData[i]))
                    {
                        continue;
                    }
                    csvData[i][7] = statTuples[sTIter].Item1;
                    csvData[i][8] = statTuples[sTIter].Item2;
                    csvData[i][11] = statTuples[sTIter].Item3;
                    sTIter += 1;

                }
            }
            
            // Write to mirageboard_log.txt
            WriteToMirageboardLog(csvData, currDir);

            return csvData;
        }

        private static List<List<string>> ModifyCapList(List<List<string>> capData)
        {
            // Col 47 is the AP cost
            foreach (var row in capData)
            {
                if (Int32.Parse(row[47]) > 12)
                {
                    row[47] = "12";
                }
            }

            return capData;
        }

        public static void mirageboard_dataWriteCsv(string currDir, string seedvalue, RichTextBox log)
        {
            log.AppendText("Shuffling mirageboard nodes....\n");
            // Get filename to edit
            string csvfilename = Path.Combine(currDir, "mirageboard_data.csv");
            string capPath = Path.Combine(currDir, "command_ability_param.csv");

            // Put all lines of the file into a list to edit
            List<List<string>> mbData = CsvHandling.CsvReadData(csvfilename);

            List<List<string>> capData = CsvHandling.CsvReadData(capPath);

            // Edit the list
            mbData = modifyBoards(mbData, seedvalue, currDir);

            // Reduce costs of AP for abilities above 12 to 12 (only applies to 4 abilities, I think)
            capData = ModifyCapList(capData);

            // Write over the file
            CsvHandling.CsvWriteDataAddHeadRow(csvfilename,mbData, 20);

            // Write over the file
            CsvHandling.CsvWriteDataAddHeadRow(capPath, capData, 158);


        }
    }
}
