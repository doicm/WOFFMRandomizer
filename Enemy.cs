using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace WOFFRandomizer
{
    internal class Enemy
    {
        private static List<List<string>> readCsv(string path)
        {
            // Get filename
            string csvfilename = Path.GetFullPath(path);
            // Put all lines of the file into a list to edit
            var csvFile = File.ReadAllLines(csvfilename, Encoding.UTF8);
            var output = new List<string>(csvFile);
            // Convert each row string to a row list
            List<List<string>> listListCsv = new List<List<string>>();
            foreach (var row in output)
            {
                List<string> listCsv = row.Split(",").ToList();
                listListCsv.Add(listCsv);
            }
            return listListCsv;
        }

        private static void writeCsv(string path, List<List<string>> listListCsv)
        {
            // convert List<List<string>>listListCsv back to List<string> output
            List<string> output = new List<string>();
            foreach (List<string> row in listListCsv)
            {
                output.Add(String.Join(",", row));
            }
            // Write over the file
            string newFileOutput = "";
            foreach (var item in output)
            {
                newFileOutput += String.Join(",", item) + Environment.NewLine;
            }
            System.IO.File.WriteAllText(Path.GetFullPath(path), newFileOutput);
        }

        private static (Dictionary<string, List<string>>, List<List<string>>, List<Tuple<string, string, string, string, string>>)
            bestiaryTraversal(List<List<string>> EGLoutput, List<List<string>> CESLoutput, Dictionary<string, List<string>> enemiesDict,
            List<List<string>> ceslRowData, List<Tuple<string, string, string, string, string>> levelsGEXP, int start, int end, string currDir)
        {
            // Function for each set of lines for the bestiary
            List<int> exceptionLineList = [123, 144, 169, 188, 189, 210, 231, 262, 281, 282, 334, 342, 343, 345, 374, 376, 377, 387]; // (need to minus 1 from enemies on this list)
            int ceslLine = start - 1;
            while (ceslLine <= end - 1)
            {
                // if line is exception, ignore it and move to the next
                if (exceptionLineList.Contains(ceslLine))
                {
                    ceslLine++;
                    continue;
                }
                // add all of the row data minus the variantID
                int lineIter = 1;
                List<string> CESLrow = new List<string>();
                while (lineIter < CESLoutput[ceslLine].Count)
                {
                    CESLrow.Add(CESLoutput[ceslLine][lineIter]);
                    lineIter++;
                }
                ceslRowData.Add(CESLrow);
                // make separate data for levels, gil and exp (including NG+ data)
                levelsGEXP.Add(new Tuple<string, string, string, string, string>
                    (CESLoutput[ceslLine][3], CESLoutput[ceslLine][79], CESLoutput[ceslLine][80], CESLoutput[ceslLine][81], CESLoutput[ceslLine][82]));
                enemiesDict[CESLoutput[ceslLine][0]] = [CESLoutput[ceslLine][2]];
                // search EGL for first instance of value in key value, then add to value array
                foreach (List<string> row in EGLoutput)
                {
                    if (row.Contains(CESLoutput[ceslLine][2]))
                    {
                        int idx = row.FindIndex(str => str == CESLoutput[ceslLine][2]);
                        enemiesDict[CESLoutput[ceslLine][0]].Add(row[idx - 1]);
                        break;
                    }
                }
                ceslLine++;
            }

            return (enemiesDict, ceslRowData, levelsGEXP);
        }

        // Function to collect random encounters for shuffling
        private static (Dictionary<string, List<string>>, List<List<string>>, List<Tuple<string, string, string, string, string>>) 
            collectRandomEncounters(List<List<string>> EGLoutput, List<List<string>> CESLoutput, Dictionary<string, List<string>> enemiesDict, 
            List<List<string>> ceslRowData, List<Tuple<string, string, string, string, string>> levelsGEXP, string currDir)
        {
            // Wellspring Woods 
            // variants are 117, 118
            // lines in CESL are 117-118, but it starts at 116. always minus 1 from the line, since they're 1 based and not 0 based. I'll account for that in the function
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 117, 118, currDir);

            // Nether Nebula
            // variants are 123-133. I need to make an exception for copper gnome 1 to allow for the scale to be weighed down. copper gnome is 124
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 123, 133, currDir);

            // Watchplains
            // variants are 137-144, 146-149. exception for 145
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 137, 149, currDir);

            // Pyreglow Forest
            // variants are 153-161
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 153, 161, currDir);

            // Icicle Ridge
            // 166-169,171-181. exception for 170
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 166, 181, currDir);

            // Saronia Docks
            // 188-194. 190 is Mimic, which will be excluded. 189 is Sharqual, which I will also exclude in case Murkrift is unavailable/gets defeated.
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 188, 194, currDir);

            // Dragon's Scars
            // 206-212, exception for 211 (mega red dragon)
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 206, 212, currDir);

            // Valley Seven
            // 216-223
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 216, 223, currDir);

            // Windswept Mire
            // 227-233, exception for 232
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 227, 233, currDir);

            // Phantom Sands
            // 238-246
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 238, 246, currDir);

            // Underground Prison
            // 251-252. Excluding both actually because all mirages are lost at this point anyway and it could be troublesome
            // (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 251, 252, currDir);

            // Mako Reactor 0
            // 260-267, exception for 263. 268 is Mimic Jackpot, which will be excluded as well
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 260, 267, currDir);

            // Big Bridge
            // 274-284, Excepting 282, 283 (events). I wonder how quest enemies are affected (281 is Minotaur).
            // 277 (Mythril Giant) static included. Imp (292) will be included with the next set.
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 274, 284, currDir);

            // Train Graveyard
            // 285-292
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 285, 292, currDir);

            // Sunken Temple
            // 303-312
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 303, 312, currDir);

            // Crystal Tower
            // 330-348, including 330 for Kuza Beast static, 345 for Kuza Kit; 
            // excluding 335 for Elasmos (no encounter set has that), 343 344 and 346 for Bahamutians (same) 
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 330, 348, currDir);

            // Chainroad
            // 361-370
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 361, 370, currDir);

            // Castle Exnine
            // 373-392
            // exclude 375 for Behemonster. Don't want to try XLs in rotation yet. 377-378,388 (no entry) 
            (enemiesDict, ceslRowData, levelsGEXP) = bestiaryTraversal(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, 373, 392, currDir);

            // Only going up to Castle Exnine for random encounters because post-game stuff is fine on its own (and may not be included in non-Maxima?)

            return (enemiesDict, ceslRowData, levelsGEXP);
        }

        private static List<List<string>> ceslOutputModify(List<List<string>> CESLoutput, Dictionary<string, List<string>> eDictShuffled,
            List<Tuple<string, string, string, string, string>> levelsGEXP)
        {
            List<string> eDictKeys = new List<string>(eDictShuffled.Keys);
            eDictKeys.Sort();
            int levelIter = 0;
            foreach (string key in eDictKeys)
            {
                // first iteration combines the data
                foreach (List<string> row in CESLoutput)
                {
                    if (key == row[0])
                    {
                        int colIter = 1;
                        int lenRow = row.Count;
                        // reinsert the stored randomized values into CESLoutput
                        while (colIter < lenRow)
                        {
                            row[colIter] = eDictShuffled[key][colIter + 1];
                            colIter++;
                        }
                        // reassign the base values of levels, exp, and gil
                        row[3] = levelsGEXP[levelIter].Item1;
                        row[79] = levelsGEXP[levelIter].Item2;
                        row[80] = levelsGEXP[levelIter].Item3;
                        row[81] = levelsGEXP[levelIter].Item4;
                        row[82] = levelsGEXP[levelIter].Item5;
                        levelIter++;
                        break;
                    }
                }
            }
            return CESLoutput;
        }

        private static List<List<string>> eglOutputModify(List<List<string>> EGLoutput, 
            Dictionary<string, List<string>> eDictShuffled, string currDir)
        {
            // I want to keep a txt log of enemies for each area to reference later for monster_place
            // I'll store it in a list first. i'll remove duplicates in each area
            Dictionary<string, List<string>> miragesByArea = new Dictionary<string, List<string>>();
            foreach (string key in eDictShuffled.Keys) 
            {
                foreach (List<string> row in EGLoutput)
                {
                    int j = 1;
                    while (j <= 6)
                    {
                        if (row[(4*j)+2] == key)
                        {
                            row[(4 * j) + 1] = eDictShuffled[key][0];
                            row[(4 * j)] = eDictShuffled[key][1];
                            if (row[1].Count() < 8)
                            {
                                j++;
                                continue;
                            }

                        }
                        j++;
                    }
                }
            }
            return EGLoutput;
        }
        
        private static (List<List<string>>, List<List<string>>) modifyEnemies(List<List<string>> EGLoutput, 
            List<List<string>> CESLoutput, string sV, RichTextBox log, string currDir)
        {
            // enemies dictionary to hold the final set of enemies
            Dictionary<string, List<string>> enemiesDict = new Dictionary<string, List<string>>();
            // ceslRowData list to hold the ceslRowData in order and to apply to the enemies dictionary after randomization
            List<List<string>> ceslRowData = new List<List<string>>();
            List<Tuple<string, string, string, string, string>> levelsGEXP = new List<Tuple<string, string, string, string, string>>();
            // the goal is to not randomize every single slot in the game, but randomize every unique slot up to final boss. 
            // avoid duplicates
            // for example, all Mu 1's <-> all Copper Gnome 2's with ceslRowData remaining
            // This is for random encounters
            log.AppendText("Collecting random encounters...\n");
            (enemiesDict, ceslRowData, levelsGEXP) = collectRandomEncounters(EGLoutput, CESLoutput, enemiesDict, ceslRowData, levelsGEXP, currDir);

            // Shuffle the keys (for random encounters) separate from the values and create a new dictionary from it
            // it's harder in c#...
            // create two lists, one to store original keys and another for shuffling dictionary values
            List<string> enemyKeys = new List<string>(enemiesDict.Keys);
            List<List<string>> enemyValues = new List<List<string>>(enemiesDict.Values);
            enemyValues.Shuffle(Shuffle.ConsistentStringHash(sV));
            // create new dictionary and put values into it
            Dictionary<string, List<string>> eDictShuffled = enemyKeys.Zip(enemyValues, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);
            // put the ceslRowData back into the dictionary
            int i = 0;
            int j = 0;
            while (i < enemyKeys.Count)
            {
                j = 0;
                while (j < ceslRowData[i].Count)
                {
                    eDictShuffled[enemyKeys[i]].Add(ceslRowData[i][j]);
                    j++;
                }
                i++;
            }
            
            log.AppendText("Modifying random encounters...\n");
            // Write to CESLoutput first, changing the ceslRowData
            CESLoutput = ceslOutputModify(CESLoutput, eDictShuffled, levelsGEXP); // this is the problem

            // Write to EGLoutput second, changing the other values
            EGLoutput = eglOutputModify(EGLoutput, eDictShuffled, currDir);

            // Next, I need to randomize the rare monster encounters.
            // Rare monsters are labeled with _ex### in enemy_group_list.csv
            (EGLoutput, CESLoutput) = RareMon.shuffleRareMonsters(EGLoutput, CESLoutput, sV, log, currDir);

            return (EGLoutput, CESLoutput);
        }
        public static void mirageEncsWriteCsv(string currDir, string sV, RichTextBox log, bool bossShuffle)
        {
            List<List<string>> EGLoutput = readCsv(currDir + "/enemy_group_list.csv");
            List<List<string>> CESLoutput = readCsv(currDir + "/character_enemy_status_list.csv");
            List<List<string>> MPoutput = readCsv(currDir + "/monster_place.csv");

            (EGLoutput, CESLoutput) = modifyEnemies(EGLoutput, CESLoutput, sV, log, currDir);
            if (bossShuffle) Boss.modifyBosses(EGLoutput, CESLoutput, sV, log, currDir);
            MPoutput = MonMapAndLog.modifyMonsterPlace(MPoutput, EGLoutput, log, currDir);

            writeCsv(currDir + "/enemy_group_list.csv", EGLoutput);
            writeCsv(currDir + "/character_enemy_status_list.csv", CESLoutput);
            writeCsv(currDir + "/monster_place.csv", MPoutput);
        }
    }
}
