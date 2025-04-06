using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xaml.Permissions;
using System.Xml.Linq;
using CshToolHelpers;

namespace WOFFRandomizer.Dependencies
{
    internal class Sizes
    {
        private static List<((string, string), List<string>)> CollectSizes(string crlPath)
        {
            // Format is List<((crlID, mirageName), List<string, which contains sizeID and balance values>)>. SizeIDs are 0, 1, and 2 for small, medium, and large.
            // There's also a size 3, or large large. That's reserved for Ultros, apparently.
            List<((string, string), List<string>)> sizesList = new List<((string, string), List<string>)>();

            // Remember to exclude Tama
            // Starts at crlID 124, since we're excluding ID 123 for Tama
            // The sizes seem to have accompanying balance values for each mirage, which I'll include as a List of strings

            // First, put everything into a list of csvs
            List<List<string>> csvData = new List<List<string>>();
            var csvFile = File.ReadAllLines(crlPath, Encoding.UTF8);
            var output = new List<string>(csvFile);
            foreach (var row in output)
            {
                List<string> listCsv = row.Split(",").ToList();
                csvData.Add(listCsv);
            }

            // Iterate through each row of data, inserting into sizesList the needed information, starting at row 124
            int crlIDIter = 124;
            // List of rows to skip if ID matches something that's invalid, usually a boss or XL
            List<int> exceptionList = [150, 161, 164, 167, 180, 188, 196, 208, 211, 212, 226, 240, 253, 254, 262, 284, 292, 297, 300, 301, 305, 308, 309, 
            316, 319, 329, 330, 331, 332, 333, 334, 348, 349, 350, 351, 361, 362, 363, 364, 365, 383, 387];
            while (crlIDIter < csvData.Count)
            {
                if (exceptionList.Contains(crlIDIter))
                {
                    crlIDIter++;
                    continue;
                }
                List<string> csvRow = csvData[crlIDIter];
                // Insert original data into sizesList
                sizesList.Add(((csvRow[0], csvRow[2]), [csvRow[19], csvRow[20]]));
                crlIDIter++;
            }

            return sizesList;
        }

        private static void InsertShuffledSizesList(List<((string, string), List<string>)> shuffledSizesList, string crlPath)
        {
            // First, put everything into a list of csvs
            List<List<string>> csvData = new List<List<string>>();
            var csvFile = File.ReadAllLines(crlPath, Encoding.UTF8);
            var output = new List<string>(csvFile);
            foreach (var row in output)
            {
                List<string> listCsv = row.Split(",").ToList();
                csvData.Add(listCsv);
            }

            // Iterate through each row of data, inserting back into the data the needed information, starting at row 124
            int crlIDIter = 124;
            int sSLIter = 0;
            // List of rows to skip if ID matches something that's invalid, usually a boss or XL
            List<int> exceptionList = [150, 161, 164, 167, 180, 188, 196, 208, 211, 212, 226, 240, 253, 254, 262, 284, 292, 297, 300, 301, 305, 308, 309,
            316, 319, 329, 330, 331, 332, 333, 334, 348, 349, 350, 351, 361, 362, 363, 364, 365, 383, 387];

            string toWrite = "";
            for (int i = 0; i < csvData.Count; i++)
            {

                if (i < crlIDIter)
                {
                    toWrite += string.Join(",", csvData[i]) + Environment.NewLine;
                    continue;
                }
                if (exceptionList.Contains(crlIDIter))
                {
                    toWrite += string.Join(",", csvData[i]) + Environment.NewLine;
                    crlIDIter++;
                    continue;
                }
                csvData[i][0] = shuffledSizesList[sSLIter].Item1.Item1;
                csvData[i][2] = shuffledSizesList[sSLIter].Item1.Item2;
                csvData[i][19] = shuffledSizesList[sSLIter].Item2[0];
                //csvData[i][20] = shuffledSizesList[sSLIter].Item2[1];
                toWrite += string.Join(",", csvData[i]) + Environment.NewLine;
                sSLIter++;
                crlIDIter++;
            }
            File.WriteAllText(crlPath, toWrite);
        }

        private static void RemoveComplexTowerAnimations(string capPath, string basepath)
        {
            // First, put everything into a list of csvs
            List<List<string>> csvData = new List<List<string>>();
            var csvFile = File.ReadAllLines(capPath, Encoding.UTF8);
            var output = new List<string>(csvFile);
            foreach (var row in output)
            {
                List<string> listCsv = row.Split(",").ToList();
                csvData.Add(listCsv);
            }

            // rewrite ability_list.csv to have complex L tower abilities be different
            string toWrite = "";
            for (int i = 0; i < csvData.Count; i++)
            {
                if (csvData[i][2].Contains("Lタワー")) // if row is a special stack ability
                {
                    csvData[i][5] = "-1";
                    csvData[i][8] = "-1";
                    csvData[i][26] = "0";
                }
                toWrite += string.Join(",", csvData[i]) + Environment.NewLine;
            }
            File.WriteAllText(capPath, toWrite);

        }
        public static void SizesShuffle(string currDir, string basepath, string sV, RichTextBox log)
        {
            log.AppendText("Shuffling mirage sizes...\n");
            // Only one data file and one column (I hope) to deal with for mirage size: character_resource_list
            string crlPath = Path.Combine(currDir, "character_resource_list.csv");

            List<((string, string), List<string>)> sizesList = CollectSizes(crlPath);

            // Shuffle the sizes and balance values
            List<(string, string)> idsAndNamesList = new List<(string, string)>();
            List<List<string>> valuesListToShuffle = new List<List<string>>();
            foreach (var x in sizesList)
            {
                idsAndNamesList.Add(x.Item1);
                valuesListToShuffle.Add(x.Item2);
            }

            valuesListToShuffle.Shuffle(Shuffle.ConsistentStringHash(sV));

            List<((string, string), List<string>)> shuffledSizesList = new List<((string, string), List<string>)>();

            int vLTSIter = 0;
            foreach (var x in idsAndNamesList)
            {
                shuffledSizesList.Add(((x.Item1, x.Item2), valuesListToShuffle[vLTSIter]));
                vLTSIter++;
            }

            InsertShuffledSizesList(shuffledSizesList, crlPath);

            // The game crashes if left like this because unique tower ability animations crash the game.
            // reassigning animations seems to do the trick
            // This may need more testing, but I think this is all I need to do.
            // I don't need to shuffle anything, so it's just a simple read and write
            string capPath = Path.Combine(currDir, "command_ability_param.csv");
            RemoveComplexTowerAnimations(capPath, basepath);
        }
    }
}
