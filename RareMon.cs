using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WOFFRandomizer
{
    internal class RareMon
    {
        public static int ConsistentStringHash(string value)
        {
            var bytes = System.Text.Encoding.Default.GetBytes(value);
            int stableHash = bytes.Aggregate<byte, int>(23, (acc, val) => acc * 17 + val);
            return stableHash;
        }

        private static List<List<string>> dragonScarsSetLevel(List<List<string>> EGLoutput, List<List<string>> CESLoutput)
        {
            string dragonScarsLevelToSet = "18";
            List<string> ceslIDs = new List<string>();
            List<string> cerbRowData = EGLoutput[595];
            int j = 6;
            int i = 0;
            // get ceslIDs for each monster in group
            while (i <= 5)
            {
                if (cerbRowData[j + (i * 4)] != "-1")
                {
                    string rareMonID = cerbRowData[j + (i * 4)];
                    if (ceslIDs.Count() < 1)
                    {
                        ceslIDs.Add(rareMonID);
                    }
                    else
                    {
                        if (!ceslIDs.Contains(rareMonID))
                        {
                            ceslIDs.Add(rareMonID);

                        }
                    }
                }
                i++;
            }

            // change levels to the dragonScarsLevelToSet for each monster in group
            foreach (string id in ceslIDs)
            {
                foreach (List<string> row in CESLoutput)
                {
                    if (row[0] == id)
                    {
                        row[3] = dragonScarsLevelToSet;
                        break;
                    }
                }
            }
            return CESLoutput;
        }
        public static (List<List<string>>, List<List<string>>) shuffleRareMonsters(List<List<string>> EGLoutput, 
            List<List<string>> CESLoutput, string sV, RichTextBox log, string currDir)
        {
            log.AppendText("Shuffling rare monsters...\n");
            List<string> rareMonsterList = ["570", "572", "576", "582", "595", "599", "606", "613", "617", "620", "624", "636", "642", "645"];
            // malboro menace + princess flan kinda throw off the level balance, but oh well. same with the gigan cacti
            List<List<string>> rareMonsterData = new List<List<string>>();
            // iterate through EGLoutput first, grabbing the rare monster data
            foreach (List<string> row in EGLoutput)
            {
                if (rareMonsterList.Contains(row[0]))
                {
                    rareMonsterData.Add(row);
                }
            }
            // Get each monster in the rare encounter, if applicable
            List<string> eachRareMonster = new List<string>();
            foreach (List<string> row in rareMonsterData)
            {
                int j = 6;
                int i = 0;
                while (i <= 5)
                {
                    if (row[j + (i * 4)] != "-1")
                    {
                        string rareMonID = row[j + (i * 4)];
                        if (eachRareMonster.Count() < 1)
                        {
                            eachRareMonster.Add(rareMonID);
                        }
                        else
                        {
                            if (!eachRareMonster.Contains(rareMonID))
                            {
                                eachRareMonster.Add(rareMonID);
                            }
                        }
                    }
                    i++;
                }
            }
            // iterate through CESLoutput to grab level and gil/exp. this stays in the same order. also putting current cesl id
            List<Tuple<string, string, string, string, string, string>> levelsGEXP = new List<Tuple<string, string, string, string, string, string>>();
            foreach (List<string> row in CESLoutput)
            {
                foreach (string monster in eachRareMonster)
                {
                    if (monster == row[0])
                    {
                        // Last entry is blank for now; that'll hold the shuffled cesl id later
                        levelsGEXP.Add(new Tuple<string, string, string, string, string, string>
                            (monster, row[3], row[79], row[80], row[81], row[82]));
                        continue;
                    }
                }
            }
            // iterate through EGLoutput again, this time shuffling the rare monster data. store rare monsters in dictionary
            Dictionary<string, List<string>> rareDict = new Dictionary<string, List<string>>();
            int rmlIter = 0;
            while (rmlIter < rareMonsterList.Count)
            {
                rareDict[rareMonsterList[rmlIter]] = rareMonsterData[rmlIter];
                rmlIter++;
            }
            // create two lists, one to store original keys and another for shuffling dictionary values
            List<string> rareKeys = new List<string>(rareDict.Keys);
            List<List<string>> rareValues = new List<List<string>>(rareMonsterData);
            rareValues.Shuffle(ConsistentStringHash(sV));
            // get each rare monster listed after shuffling and remove duplicates
            List<string> shuffled_EachRareMonster = new List<string>();
            // create new dictionary and put values into it
            Dictionary<string, List<string>> sortedDict = rareKeys.Zip(rareValues, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);
            foreach (string key in rareKeys)
            {
                int j = 6;
                int i = 0;
                while (i <= 5)
                {
                    if (sortedDict[key][j + (i * 4)] != "-1")
                    {
                        string rareMonID = sortedDict[key][j + (i * 4)];
                        if (shuffled_EachRareMonster.Count() < 1)
                        {
                            shuffled_EachRareMonster.Add(rareMonID);
                        }
                        else
                        {
                            if (!shuffled_EachRareMonster.Contains(rareMonID))
                            {
                                shuffled_EachRareMonster.Add(rareMonID);
                            }
                        }
                    }
                    i++;
                }
            }
            // I need to put the previous cesl ids onto the levelsGEXP list
            // so that I can reference the levels/GEXP
            // In csharp, I need to create a new tuple for this to add to the list
            // since they're all strings, I'll just do a list of list of strings
            List<List<string>> levelsGEXPWithCESLID = new List<List<string>>();
            int ceslidIter = 0;
            foreach (Tuple<string, string, string, string, string, string> levelGEXP in levelsGEXP)
            {
                levelsGEXPWithCESLID.Add([levelGEXP.Item1, levelGEXP.Item2, levelGEXP.Item3,
                    levelGEXP.Item4, levelGEXP.Item5, levelGEXP.Item6, shuffled_EachRareMonster[ceslidIter]]);
                ceslidIter++;
            }
            int lGEXPWCESLIDIter = 0;
            bool broken1 = false;
            List<Tuple<string, List<string>>> eglList = new List<Tuple<string, List<string>>>();
            foreach (string key in sortedDict.Keys)
            {
                eglList.Add(new Tuple<string, List<string>>(key, sortedDict[key]));

                // iterate through CESL output again. the lGEXP needs to be assigned to its proper monster
                // use levelsGEXP for reference with previous and current values
                foreach (List<string> row in CESLoutput)
                {
                    if (broken1)
                    {
                        break;
                    }
                    if (levelsGEXPWithCESLID[lGEXPWCESLIDIter][6] == row[0])
                    {
                        row[3] = levelsGEXPWithCESLID[lGEXPWCESLIDIter][1];
                        row[79] = levelsGEXPWithCESLID[lGEXPWCESLIDIter][2];
                        row[80] = levelsGEXPWithCESLID[lGEXPWCESLIDIter][3];
                        row[81] = levelsGEXPWithCESLID[lGEXPWCESLIDIter][4];
                        row[82] = levelsGEXPWithCESLID[lGEXPWCESLIDIter][5];
                        lGEXPWCESLIDIter++;
                        if (lGEXPWCESLIDIter == (levelsGEXPWithCESLID.Count - 1))
                        {
                            broken1 = true;
                            break;
                        }
                        continue;
                    }
                }
            }
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
                        if ((row[j + (k * 4)] != "-1") && (row[j + (k * 4)] != row[k * 4]) && (row[j + (k * 4)] != "0"))
                        {
                            toWrite += row[1].Substring(0, 8) + ": " + row[j + (k * 4)] + Environment.NewLine;
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
            System.IO.File.AppendAllText(Path.GetFullPath(currDir + "/logs/monster_log.txt"), toWrite);

            // Making an exception for Dragon Scars rare encounter, which I want to set to level 18.
            // Some folks may not know the trick to running away and getting the required item
            // EGL ID for dragon scar encounter is 595. Be cautious if it's more than 1 enemy type
            dragonScarsSetLevel(EGLoutput, CESLoutput);

            return (EGLoutput, CESLoutput);
        }
    }
}
