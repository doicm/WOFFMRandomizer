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

        public static (List<List<string>>, List<Tuple<string, List<string>>>) fixOGLevelsAndGEXP(List<List<string>> CESLoutput,
            List<Tuple<string, List<string>>> eglList, List<List<string>> levelsGEXP, Dictionary<string, List<string>> sortedDict)
        {
            // build the EGLoutput. I only want to go through the data once though
            int i = 0;
            bool broken = false;

            // Make two lists, one where exp/gil is zero'd out (for adds in fights) and one where exp/gil is cut by 1/3 (for kupirates)
            List<string> exceptionListZero = ["185", "301", "327", "328"];
            List<string> exceptionListThird = ["196"];

            foreach (string key in sortedDict.Keys)
            {
                eglList.Add(new Tuple<string, List<string>>(key, sortedDict[key]));
                // iterate through CESL output again. the lGEXP needs to be assigned to it's proper monster
                // use levelsGEXP for reference with previous and current values
                foreach (List<string> row in CESLoutput)
                {
                    if (broken) break;
                    if (levelsGEXP[i][6] == row[0])
                    {
                        row[3] = levelsGEXP[i][1];
                        // Need to make some experience/gil adjustment exceptions for some fights, since some can give egregious amounts
                        if (exceptionListZero.Contains(row[0]))
                        {
                            row[79] = "0";
                            row[80] = "0";
                            row[81] = "0";
                            row[82] = "0";
                        }
                        else if (exceptionListThird.Contains(row[0]))
                        {
                            row[79] = (Int32.Parse(levelsGEXP[i][2]) / 3).ToString();
                            row[80] = (Int32.Parse(levelsGEXP[i][3]) / 3).ToString();
                            row[81] = (Int32.Parse(levelsGEXP[i][4]) / 3).ToString();
                            row[82] = (Int32.Parse(levelsGEXP[i][5]) / 3).ToString();
                        }
                        else
                        {
                            row[79] = levelsGEXP[i][2];
                            row[80] = levelsGEXP[i][3];
                            row[81] = levelsGEXP[i][4];
                            row[82] = levelsGEXP[i][5];
                        }
                        i++;
                        if (i == levelsGEXP.Count - 1)
                        {
                            broken = true; 
                            break;
                        }
                        continue;
                    }
                }
            }
            return (CESLoutput, eglList);
        }
        public static void modifyBosses(List<List<string>> EGLoutput, List<List<string>> CESLoutput,
            string sV, RichTextBox log, string currDir)
        {
            log.AppendText("Shuffling bosses...\n");
            // list of bosses based on EGL value
            // excluding elemental trio, due to missables
            // excluding chapter 14, since that's a mess
            // excluding order of the circle fights. they're special :)
            List<string> bossesList = ["569", "571", "573", "577", "578", "592", "598", "601", "607", "615",
                "618", "623", "626", "627", "629", "634", "635", "643", "692"];
            List<List<string>> bossData = new List<List<string>>();
            // iterate through EGLoutput first, grabbing the boss data
            foreach (List<string> row in EGLoutput)
            {
                if (bossesList.Contains(row[0]))
                {
                    bossData.Add(row);
                }
            }
            // Get each monster in the boss fight, if applicable
            List<string> eachBoss = new List<string>();
            foreach (List<string> row in bossData)
            {
                int j = 6;
                int i = 0;
                while (i <= 5)
                {
                    if (row[j + i * 4] != "-1")
                    {
                        string bossID = row[j + i * 4];
                        if (eachBoss.Count() < 1)
                        {
                            eachBoss.Add(bossID);
                        }
                        else
                        {
                            if (!eachBoss.Contains(bossID))
                            {
                                eachBoss.Add(bossID);
                            }
                        }
                    }
                    i++;
                }
            }
            // iterate through CESLoutput to grab level and gil/exp. this stays in the same order. also putting current cesl id
            // will probably need to revisit this at some point to balance levels better
            List<List<string>> levelsGEXP = new List<List<string>>();
            foreach (List<string> row in CESLoutput)
            {
                foreach (string monster in eachBoss)
                {
                    if (monster == row[0])
                    {
                        levelsGEXP.Add([monster, row[3], row[79], row[80], row[81], row[82]]);
                        continue;
                    }
                }
            }
            // iterate through EGLoutput again, this time shuffling the boss data. put the data in a dictionary to shuffle
            Dictionary<string, List<string>> bossDict = new Dictionary<string, List<string>>();
            int bLIter = 0;
            while (bLIter < bossesList.Count())
            {
                bossDict[bossesList[bLIter]] = bossData[bLIter];
                bLIter++;
            }
            List<string> bossKeys = new List<string>(bossDict.Keys);
            List<List<string>> bossValues = new List<List<string>>(bossData);
            bossValues.Shuffle(Shuffle.ConsistentStringHash(sV));
            // get each boss listed after shuffling and remove duplicates
            List<string> shuffled_eachBoss = new List<string>();
            Dictionary<string, List<string>> sortedDict = bossKeys.Zip(bossValues, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);
            foreach (string key in bossKeys)
            {
                int j = 6;
                int i = 0;
                while (i <= 5)
                {
                    if (sortedDict[key][j + i * 4] != "-1")
                    {
                        string rareMonID = sortedDict[key][j + i * 4];
                        if (shuffled_eachBoss.Count() < 1)
                        {
                            shuffled_eachBoss.Add(rareMonID);
                        }
                        else
                        {
                            if (!shuffled_eachBoss.Contains(rareMonID))
                            {
                                shuffled_eachBoss.Add(rareMonID);
                            }
                        }
                    }
                    i++;
                }
            }
            // i need to put the previous cesl ids onto the levelsGEXP list
            // so that I can reference the levels/GEXP
            int lGEXPIter = 0;
            while (lGEXPIter < levelsGEXP.Count())
            {
                levelsGEXP[lGEXPIter].Add(shuffled_eachBoss[lGEXPIter]);
                lGEXPIter++;
            }
            List<Tuple<string, List<string>>> eglList = new List<Tuple<string, List<string>>>();
            (CESLoutput, eglList) = fixOGLevelsAndGEXP(CESLoutput, eglList, levelsGEXP, sortedDict);
            // build the EGLoutput. I only want to go through the data once though
            int EGLLIter = 0;
            bool broken2 = false;
            // make a deep copy of eglList by serializing to json
            var jsonEglList = JsonSerializer.Serialize(eglList);
            var deepEglList = JsonSerializer.Deserialize<List<Tuple<string, List<string>>>>(jsonEglList);

            // while going through the eglList, put these values in monster_log.txt
            string toWrite = "";
            foreach (List<string> row in EGLoutput)
            {
                if (broken2) break;
                if (deepEglList[EGLLIter].Item1 == row[0])
                {
                    int rowIter = 3;
                    while (rowIter < row.Count())
                    {
                        row[rowIter] = deepEglList[EGLLIter].Item2[rowIter];
                        rowIter++;
                    }
                    // write values for each rare monster in line
                    int j = 4;
                    int k = 0;
                    while (k <= 6)
                    {
                        // account for duplicates in same row
                        if (row[j + k * 4] != "-1" && row[j + k * 4] != row[k * 4])
                        {
                            if (int.Parse(row[j + k * 4]) > 1)
                            toWrite += row[1] + ": " + row[j + k * 4] + Environment.NewLine;
                        }
                        k++;
                    }
                    EGLLIter++;
                    if (EGLLIter == deepEglList.Count())
                    {
                        broken2 = true;
                    }
                }
            }
            File.AppendAllText(Path.GetFullPath(currDir + "/logs/monster_log.txt"), toWrite);
        }
    }
}
