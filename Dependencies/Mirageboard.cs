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

    public static class ListExtensions // https://discussions.unity.com/t/c-adding-multiple-elements-to-a-list-on-one-line/80117/2
    {
        public static void AddMany<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
        }

        public static void Shuffle<T>(this IList<T> list, int seed)
        {
            var rng = new Random(seed);
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
    internal class Mirageboard
    {
        private static void enemyRandoSetMethod(List<List<string>> listListCsv)
        {
            // Critical values are the following for Tama, Chocochick, and Sylph:
            var enemyRandoSpecialMirages = new List<(string, string)>
            {
                ("68", "2000"),
                ("997", "2002"),
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

        public static void modifyForEnemyRandoOnly(string currDir)
        {
            // Get filename to edit
            string csvfilename = Path.Combine(currDir, "mirageboard_data.csv");

            // Put all lines of the file into a list to edit
            var csvFile = File.ReadAllLines(csvfilename, Encoding.UTF8);
            var output = new List<string>(csvFile);
            List<List<string>> listListCsv = new List<List<string>>();
            foreach (var row in output)
            {
                List<string> listCsv = row.Split(",").ToList();
                listListCsv.Add(listCsv);
            }
            enemyRandoSetMethod(listListCsv);

            // convert List<List<string>>listListCsv back to List<string> output
            output = new List<string>();
            foreach (List<string> row in listListCsv)
            {
                output.Add(string.Join(",", row));
            }
            // Write over the file
            string newFileOutput = "";
            foreach (var item in output)
            {
                newFileOutput += string.Join(",", item) + Environment.NewLine;
            }

            File.WriteAllText(currDir + "/mirageboard_data.csv", newFileOutput);
        }

        private static bool ignoreRows(List<string> row)
        {
            List<string> skipRows = new List<string>();
            List<string> skipAbilities = new List<string>();
            List<string> skipEnemyRandoRows = new List<string>();
            // ch 1 chocochick must learn joyride (992)
            // pre ch 6 black nakk must learn sizzle (1262)
            // pre ch 6 mythril giant must learn smash (2251)
            // ch 8 floating eye must learn flutter (4261)
            // pre ch 11 quachacho must learn chill (1592)
            // pre ch 15 searcher must learn zap (5402)
            skipRows.AddMany("992", "1262", "2251", "4261", "1592", "5402");
            // For use with skipping smash and joyride, as well as other possible abilities
            skipAbilities.AddMany("2003", "2010");
            // For use with enemy randos
            skipEnemyRandoRows.AddMany("68", "997", "4749", "4751");

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

            return false;
        }
        private static List<string> modifyBoards(List<string> output, string seedvalue)
        {
            // statTuples to store each set of three datas that will be randomized
            var statTuples = new List<Tuple<string, string, string>>();
            int startData = 60; // start data, not including Lann and Reynn
            int endData = 7620; // max count of data
            // iterate through each used node and extract the needed values that will be randomized
            // Also convert each row string to a row list
            List<List<string>> listListCsv = new List<List<string>>();
            foreach (var row in output)
            {
                List<string> listCsv = row.Split(",").ToList();
                listListCsv.Add(listCsv);
            }
            foreach (List<string> row in listListCsv)
            {

                int i = listListCsv.FindIndex(str => str == row);
                if (i >= startData && i <= endData)
                {
                    if (ignoreRows(listListCsv[i]))
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
            foreach (List<string> row in listListCsv)
            {
                int i = listListCsv.FindIndex(str => str == row);
                if (i >= startData && i <= endData)
                {
                    if (ignoreRows(listListCsv[i]))
                    {
                        continue;
                    }
                    listListCsv[i][7] = statTuples[sTIter].Item1;
                    listListCsv[i][8] = statTuples[sTIter].Item2;
                    listListCsv[i][11] = statTuples[sTIter].Item3;
                    sTIter += 1;

                }
            }

            // convert List<List<string>>listListCsv back to List<string> output
            output = new List<string>();
            foreach (List<string> row in listListCsv)
            {
                output.Add(string.Join(",", row));
            }

            return output;
        }

        public static void mirageboard_dataWriteCsv(string currDir, string seedvalue, RichTextBox log)
        {
            // Get filename to edit
            string csvfilename = Path.Combine(currDir, "mirageboard_data.csv");

            // Put all lines of the file into a list to edit
            var csvFile = File.ReadAllLines(csvfilename, Encoding.UTF8);
            var output = new List<string>(csvFile);

            // Edit the list
            output = modifyBoards(output, seedvalue);

            // Write over the file
            string newFileOutput = "";
            foreach (var item in output)
            {
                newFileOutput += string.Join(",", item) + Environment.NewLine;
            }

            File.WriteAllText(currDir + "/mirageboard_data.csv", newFileOutput);


        }
    }
}
