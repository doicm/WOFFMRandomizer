using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace WOFFRandomizer.Dependencies
{
    internal class MonMapAndLog
    {
        private static List<List<string>> addToNonRandomizedAreas(List<List<string>> nonRandomizedAreas,
            List<List<string>> MPoutput, int start, int end)
        {
            int i = start;
            while (i < end)
            {
                nonRandomizedAreas.Add(MPoutput[i]);
                i++;
            }
            return nonRandomizedAreas;
        }

        private static void bestiaryTraversalPostShuffle(string currDir, List<List<string>> EGLoutput, int start, int end)
        {
            List<int> exceptionLineList = [156,158,262,381,406,407];
            int eglLine = start;
            string areaname = EGLoutput[start][1];
            int i = 0;
            int j = 4;
            List<string> miragesInArea = new List<string>();
            List<string> row = new List<string>();
            string toWrite = "";
            while (eglLine <= end)
            {
                if (exceptionLineList.Contains(eglLine))
                {
                    eglLine++;
                    continue;
                }
                i = 0;
                row = EGLoutput[eglLine];
                while (i <= 5)
                {
                    
                    if (row[i*4 + j] != "-1")
                    {
                        if (!miragesInArea.Contains(row[i*4 + j]))
                        {
                            miragesInArea.Add(row[i*4 + j]);
                        }
                    }
                    i++;
                }
                eglLine++;
            }
            // for each mirage on the area list, add them to be written in the log
            foreach (string mirage in miragesInArea)
            {
                toWrite += areaname.Substring(0,8) + ": " + mirage + "\n";
            }
            File.AppendAllText(Path.GetFullPath(currDir + "/logs/monster_log.txt"), toWrite);

        }
        private static void randomEncountersPostShuffle(string currDir, List<List<string>> EGLoutput)
        {
            // Wellspring Woods 
            // EGL lines are 85-87
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 85, 87);

            // Nether Nebula
            // lines are 88-102.
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 88, 102);

            // Watchplains
            // lines 114-132
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 114, 132);

            // Pyreglow Forest
            // lines 134-147
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 134, 147);

            // Icicle Ridge
            // lines 154-172, exclude 156, 158
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 154, 172);

            // Saronia Docks
            // lines 174-187
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 174, 187);

            // Dragon's Scars
            // lines 194-205
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 194, 205);

            // Valley Seven
            // lines 214-227
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 214, 227);

            // Windswept Mire
            // lines 234-242
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 234, 242);

            // Phantom Sands
            // lines 254-266, exception for 262
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 254, 266);

            //// Underground Prison
            //// this area is ignored, but I'll account for it above so that it gets in the data. lines 274-278
            //bestiaryTraversalPostShuffle(currDir, EGLoutput, 274, 278);

            // Mako Reactor 0
            // lines 284-294
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 284, 294);

            // Big Bridge
            // lines 304-311, 317-318
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 304, 311);
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 317, 318);

            // Train Graveyard
            // lines 324-338
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 324, 338);

            // Sunken Temple
            // lines 344-355
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 344, 355);

            // Crystal Tower
            // lines 364-383, excluding 381
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 364, 383);

            // Chainroad
            // lines 404-416, excluding 406, 407
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 404, 416);

            // Castle Exnine
            // lines 424-449
            bestiaryTraversalPostShuffle(currDir, EGLoutput, 424, 449);
        }
        public static List<List<string>> modifyMonsterPlace (List<List<string>> MPoutput, 
            List<List<string>> EGLoutput, RichTextBox log, string currDir, bool enemShuffle)
        {
            log.AppendText("Modifying mirage maps and monster log...\n");
            
            // list of monsters to ignore, including Bahamutian Soldiers, so that they don't appear in the mirage list on the map
            List<string> monsToIgnoreOnMap = ["7193", "7194", "7195"];
            // list of monsters to re-include into certain areas that were excluded from randomization
            List<List<string>> monsToReinclude = [["0100", "7068"], ["0500", "7054"], ["2100", "7044"]];

            // have data on hand for each area from monster_place.csv
            List<List<string>> monster_place_csv_data = [
                ["ParallelWorldGrove", "異世界の林", "1", "1"],
                ["NebraCave", "ネブラの洞窟", "5", "5"],
                ["MarchPlain", "進撃の平原", "6", "6"],
                ["LightForest", "灯の森", "7", "7"],
                ["IcicleValley", "氷柱の谷フロア", "12", "11"],
                ["SaloniaHarbor", "サロニアの港", "14", "13"],
                // ["SewerArea", "下海・入口", "16", "15"], //sewer area will be ignored, since I don"t randomize this set yet
                ["DragonValley", "竜の渓谷", "19", "18"],
                ["BombVolcano", "セブンスバリー", "23", "21"],
                ["WindWetlandsBands", "風吹く湿地帯", "24", "22"],
                ["PhantomDesert", "幻の砂漠", "27", "25"],
                // ["DeepGround", "ディープグラウンド", "29", "28"], // this area doesn"t get randomized either
                ["MakoFireplaceInside", "零番魔晄炉", "30", "29"],
                ["big_bridge_", "ビックブリッジ", "32", "31"],
                ["resshahakaba_", "列車墓場", "35", "34"],
                ["SeabedTemple_", "海底神殿", "41", "40"],
                ["CrystalTower_", "クリスタルタワー", "43", "42"],
                ["ChainLoad_", "鎖の道", "47", "45"],
                ["ExNineCastle_", "エクスナイン城", "49", "47"]
            ];

            // Store then clear the text of monster_log.txt to append back later after traversing through random encounters
            string tempLog = File.ReadAllText(Path.GetFullPath(currDir + "/logs/monster_log.txt"), Encoding.UTF8);
            File.WriteAllText(Path.GetFullPath(currDir + "/logs/monster_log.txt"), "");

            randomEncountersPostShuffle(currDir, EGLoutput);

            File.AppendAllText(Path.GetFullPath(currDir + "/logs/monster_log.txt"), tempLog);

            // get the areas that aren't being randomized
            List<List<string>> nonRandomizedAreas = new List<List<string>>();
            nonRandomizedAreas = addToNonRandomizedAreas(nonRandomizedAreas, MPoutput, 42, 50);
            nonRandomizedAreas = addToNonRandomizedAreas(nonRandomizedAreas, MPoutput, 84, 87);

            // write for each area
            List<List<string>> newMPoutput = new List<List<string>>();

            // read monster log file and get data for each area's maps
            string[] f = File.ReadAllLines(Path.GetFullPath(currDir + "/logs/monster_log.txt"), Encoding.UTF8);
            int id = 0;
            int mpcdIter = 0;
            string currLine = "0000";
            string prevLine = "0000";
            int areaIter = 1;
            int mtrIter = 0;
            foreach (string line in f)
            {
                currLine = line.Substring(4, 4);
                // if the next line is a new area, then go to the next area in monster_place_csv_data
                if (currLine != prevLine)
                {
                    // if currLine = particular line that were excepted, then put in unrandomized data
                    if (currLine == "0600")
                    {
                        int j = 0;
                        while (j < 8)
                        {
                            if (nonRandomizedAreas[j][1].Substring(0,5) != "Sewer") break;
                            newMPoutput.Add([id.ToString(), nonRandomizedAreas[j][1], nonRandomizedAreas[j][2],
                            nonRandomizedAreas[j][3],nonRandomizedAreas[j][4],nonRandomizedAreas[j][5],
                            nonRandomizedAreas[j][6]]);
                            j++;
                            id++;
                        }
                    }
                    if (currLine == "1100")
                    {
                        int j = 8;
                        while (j < nonRandomizedAreas.Count)
                        {
                            if (nonRandomizedAreas[j][1].Substring(0, 5) != "DeepG") break;
                            newMPoutput.Add([id.ToString(), nonRandomizedAreas[j][1], nonRandomizedAreas[j][2],
                                nonRandomizedAreas[j][3],nonRandomizedAreas[j][4],nonRandomizedAreas[j][5],
                                nonRandomizedAreas[j][6]]);
                            j++;
                            id++;
                        }
                    }
                    prevLine = currLine;
                    mpcdIter++;
                    areaIter = 1;
                    if (mpcdIter == monster_place_csv_data.Count) break;
                    // check for monsToReinclude
                    if (mtrIter < monsToReinclude.Count)
                    {
                        if (currLine == monsToReinclude[mtrIter][0])
                        {
                            newMPoutput.Add([id.ToString(), monster_place_csv_data[mpcdIter][0] + areaIter.ToString(),
                                monster_place_csv_data[mpcdIter][1],monster_place_csv_data[mpcdIter][2], monsToReinclude[mtrIter][1],
                                monster_place_csv_data[mpcdIter][3],"0"]);
                            id++;
                            areaIter++;
                            mtrIter++;
                        }
                    }
                }
                if (monsToIgnoreOnMap.Contains(line.Substring(10))) continue;
                if (mpcdIter == monster_place_csv_data.Count()) break;
                newMPoutput.Add([id.ToString(), monster_place_csv_data[mpcdIter][0] + areaIter.ToString(),
                    monster_place_csv_data[mpcdIter][1],monster_place_csv_data[mpcdIter][2], line.Substring(10),
                    monster_place_csv_data[mpcdIter][3],"0"]);
                // unfortunately, game bugs out when there's more than 12 mons per area on map. there is a log, at least
                if (areaIter > 12) 
                {
                    newMPoutput.RemoveAt(newMPoutput.Count() - 1);
                }
                areaIter++;
                id++;
            }

            modifyMonsterLog(monsToIgnoreOnMap, monsToReinclude, currDir, enemShuffle);

            return newMPoutput;
        }

        private static void modifyMonsterLog(List<string> monsToIgnoreOnMap, 
            List<List<string>> monsToReinclude, string currDir, bool enemShuffle)
        {
            List<List<string>> MPoutput = new List<List<string>>();
            string[] f = File.ReadAllLines(Path.GetFullPath(currDir + "/logs/monster_log.txt"), Encoding.UTF8);
            int mtrIter = 0;
            foreach (string line in f)
            {
                // first entry is areaID. second entry is mirageID
                if (monsToIgnoreOnMap.Contains(line.Substring(10))) continue;
                // if current area is same as monsToReinclude area, sneak it in
                if (monsToReinclude.Count > 0)
                {
                    if (mtrIter < monsToReinclude.Count)
                    {
                        if (line.Substring(4, 4) == monsToReinclude[mtrIter][0])
                        {
                            MPoutput.Add([monsToReinclude[mtrIter][0], monsToReinclude[mtrIter][1]]);
                            mtrIter++;
                        }
                    }
                    
                }
                MPoutput.Add([line.Substring(4, 4), line.Substring(10)]);
            }
            string[] linesArea = File.ReadAllLines(Path.GetFullPath(
                currDir + "/database/areas.txt"), Encoding.UTF8);
            string[] linesEnemy = File.ReadAllLines(Path.GetFullPath(
                currDir + "/database/enemy_names.txt"), Encoding.UTF8);

            // rewrite monster log
            string toWrite = "";
            bool raresStarted = false; bool bossesStarted = false;
            if (enemShuffle) toWrite += "Random encounters: \n";
            foreach (List<string> row in MPoutput)
            {
                // find area first
                // if random encounters are not randomized, skip the first part
                if (enemShuffle)   
                {
                    foreach (string areaString in linesArea)
                    {
                        string temp = areaString.Substring(0, 4);
                        if (areaString.Substring(0, 4) == row[0])
                        {
                            toWrite += areaString.Substring(8) + ": ";
                            // then find the enemy
                            foreach (string enemyString in linesEnemy)
                            {
                                if (enemyString.Substring(0, 4) == row[1])
                                {
                                    toWrite += enemyString.Substring(5) + "\n";
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                

                // then find rare monsters to append if chapter enemy
                if (row[0].Contains("_e"))
                {
                    if (!raresStarted)
                    {
                        raresStarted = true;
                        if (toWrite.Length > 0) toWrite += "---\n";
                        toWrite += "Rare monsters: \n";
                    }
                    foreach (string enemyString in linesEnemy)
                    {
                        if (enemyString.Substring(0, 4) == row[1])
                        {
                            toWrite += "Chapter " + row[0].Substring(0, 2) + ": " +
                                enemyString.Substring(5) + "\n";
                            break;
                        }
                    }
                }
                
                // then find bosses to append
                else if (row[0].Contains("0406") | row[0].Contains("_0"))
                {
                    if (!bossesStarted)
                    {
                        bossesStarted = true;
                        if (toWrite.Length > 0) toWrite += "---\n";
                        toWrite += "Boss fights: \n";
                    }
                    foreach (string enemyString in linesEnemy)
                    {
                        if (enemyString.Substring(0, 4) == row[1])
                        {
                            toWrite += "Chapter " + row[0].Substring(0, 2) + ": " +
                                enemyString.Substring(5) + "\n";
                            break;
                        }
                    }
                }
            }
            File.WriteAllText(Path.GetFullPath(currDir + "/logs/monster_log.txt"), toWrite);
        }
    }
}
