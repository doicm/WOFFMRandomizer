using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace WOFFRandomizer.Dependencies
{
    internal class MonMap
    {
        private static List<List<string>> bestiaryTraversalPostShuffle(string mlogPath, List<List<string>> EGLoutput, int start, int end, 
            List<List<string>> randEncMPList)
        {
            List<int> exceptionLineList = [156,158,262,381,406,407];
            int eglLine = start;
            string areaname = EGLoutput[start][1];
            int i = 0;
            int j = 4;
            List<string> miragesInArea = new List<string>();
            List<string> row = new List<string>();
            //string toWrite = "";
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
                            randEncMPList.Add([row[(i * 4) + (j)], row[1]]);
                        }
                    }
                    i++;
                }
                eglLine++;
            }
            //// for each mirage on the area list, add them to be written in the log
            //foreach (string mirage in miragesInArea)
            //{
            //    toWrite += areaname.Substring(0,8) + ": " + mirage + "\n";
            //}
            //File.AppendAllText(mlogPath, toWrite);

            return randEncMPList;
        }
        private static List<List<string>> randomEncountersPostShuffle(string currDir, List<List<string>> EGLoutput,
            List<List<string>> randEncMPList)
        {
            // Wellspring Woods 
            // EGL lines are 85-87
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 85, 87, randEncMPList);

            // Nether Nebula
            // lines are 88-102.
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 88, 102, randEncMPList);

            // Watchplains
            // lines 114-132
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 114, 132, randEncMPList);

            // Pyreglow Forest
            // lines 134-147
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 134, 147, randEncMPList);

            // Icicle Ridge
            // lines 154-172, exclude 156, 158
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 154, 172, randEncMPList);

            // Saronia Docks
            // lines 174-187
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 174, 187, randEncMPList);

            // Dragon's Scars
            // lines 194-205
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 194, 205, randEncMPList);

            // Valley Seven
            // lines 214-227
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 214, 227, randEncMPList);

            // Windswept Mire
            // lines 234-242
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 234, 242, randEncMPList);

            // Phantom Sands
            // lines 254-266, exception for 262
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 254, 266, randEncMPList);

            //// Underground Prison
            //// this area is ignored, but I'll account for it above so that it gets in the data. lines 274-278
            //bestiaryTraversalPostShuffle(currDir, EGLoutput, 274, 278);

            // Mako Reactor 0
            // lines 284-294
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 284, 294, randEncMPList);

            // Big Bridge
            // lines 304-311, 317-318
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 304, 311, randEncMPList);
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 317, 318, randEncMPList);

            // Train Graveyard
            // lines 324-338
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 324, 338, randEncMPList);

            // Sunken Temple
            // lines 344-355
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 344, 355, randEncMPList);

            // Crystal Tower
            // lines 364-383, excluding 381
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 364, 383, randEncMPList);

            // Chainroad
            // lines 404-416, excluding 406, 407
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 404, 416, randEncMPList);

            // Castle Exnine
            // lines 424-449
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 424, 449, randEncMPList);

            // Coeurl and Lesser Coeurl
            // lines 1171, 1172. ...numbers are a bit off, but this adjustment should work?
            randEncMPList = bestiaryTraversalPostShuffle(currDir, EGLoutput, 1146, 1147, randEncMPList);

            return randEncMPList;
        }

        // Rewriting ModifyMonsterPlaceAndMonsterLog to go through egl rather than monster_log for monster_place
        public static void ModifyMonsterPlaceAndMonsterLog (string currDir)
        {
            // list of monsters to ignore, including Bahamutian Soldiers, so that they don't appear in the mirage list on the map
            List<string> monsToIgnoreOnMap = ["7193", "7194", "7195"];

            // have data on hand for each area from monster_place.csv
            List<List<string>> monster_place_csv_data = [
                ["ParallelWorldGrove", "異世界の林", "1", "1"],
                ["NebraCave", "ネブラの洞窟", "5", "5"],
                ["MarchPlain", "進撃の平原", "6", "6"],
                ["LightForest", "灯の森", "7", "7"],
                ["IcicleValley", "氷柱の谷フロア", "12", "11"],
                ["SaloniaHarbor", "サロニアの港", "14", "13"],
                //["SewerArea", "下海・入口", "16", "15"], //sewer area will be ignored, since I don"t randomize this set
                ["DragonValley", "竜の渓谷", "19", "18"],
                ["BombVolcano", "セブンスバリー", "23", "21"],
                ["WindWetlandsBands", "風吹く湿地帯", "24", "22"],
                ["PhantomDesert", "幻の砂漠", "27", "25"],
                //["DeepGround", "ディープグラウンド", "29", "28"], // this area doesn"t get randomized either, other than rare
                ["MakoFireplaceInside", "零番魔晄炉", "30", "29"],
                ["big_bridge_", "ビックブリッジ", "32", "31"],
                ["resshahakaba_", "列車墓場", "35", "34"],
                ["SeabedTemple_", "海底神殿", "41", "40"],
                ["CrystalTower_", "クリスタルタワー", "43", "42"],
                ["ChainLoad_", "鎖の道", "47", "45"],
                ["ExNineCastle_", "エクスナイン城", "49", "47"]
            ];

            string eglPath = Path.Combine(currDir, "enemy_group_list.csv");
            string mpPath = Path.Combine(currDir, "monster_place.csv");
            string mlogPath = Path.Combine(currDir, "logs", "monster_log.txt");

            List<List<string>> eglData = CsvHandling.CsvReadData(eglPath);
            List<List<string>> randEncMPList = new List<List<string>>();
            
            // As part of writing to a log, also add to a new MP list for random encounters
            randEncMPList = randomEncountersPostShuffle(mlogPath, eglData, randEncMPList);

            // Create a separate list for MP for rare encounters, whether or not shuffled
            // Get rare monsters from egl along with approx location
            List<List<string>> rareMons = new List<List<string>>();
            List<string> eglRareIDs = ["570", "572", "576", "582", "595", "599", "606", "613", "617", "620",
                "624", "636", "642", "645"];
            List<List<string>> rareChapterToAreaPairings = [["04","02"],["05","03"],["06","04"], ["08", "05"], ["10", "06"],
                ["11","07"],["12","08"],["15","11"],["16","13"],["17","15"],["19","18"],["21_00","20"],["21_01","21"]];

            foreach (var row in eglData)
            {
                if (eglRareIDs.Contains(row[0]))
                {
                    int i = 0;
                    int j = 4;
                    string charID;
                    while (i < 6)
                    {
                        charID = row[j + (i * 4)];
                        if (charID != "-1" && !rareMons.Select(x => x[0]).Contains(charID))
                        {
                            rareMons.Add([charID, row[1]]);
                        }
                        i++;
                    }
                }
            }

            List<List<string>> MPinput = new List<List<string>>();

            // Go through each area and input the values into MPinput
            // I need to manually go through SewerArea and DeepGround
            int mpID = 0;
            int areaIter = 1;
            int mpcdIter = 0; // This is for iterating through that big table above
            int rareIter = 0; // This is for iterating through the rareMons list
            int ctoaIter = 0;
            bool firstRow = true;
            string prevRowArea = "";
            foreach (var row in randEncMPList)
            {
                if (firstRow)
                {
                    firstRow = false;
                    MPinput.Add([mpID.ToString(), monster_place_csv_data[mpcdIter][0] + areaIter.ToString(),
                        monster_place_csv_data[mpcdIter][1], monster_place_csv_data[mpcdIter][2],
                        row[0], monster_place_csv_data[mpcdIter][3], "0"]);
                    prevRowArea = row[1].Substring(0,8);
                    mpID++;
                    areaIter++;
                    continue;
                }
                // If the current row's area doesn't match the previous row's area, a new area has emerged
                // Check for rare mirages
                if (prevRowArea != row[1].Substring(0,8))
                {
                    // Chapter name area doesn't match random encounter area. This'll be painful...
                    // Got some pairings set up to correlate the chapterID with the areaID
                    if (rareChapterToAreaPairings[ctoaIter][1] == prevRowArea.Substring(4,2))
                    {
                        // While it's unlikely to make it to the final list, need to make an exception 
                        // for chainroad and exnine castle rare mirages
                        while (rareChapterToAreaPairings[ctoaIter][0] == rareMons[rareIter][1].Substring(4, 2))
                        {
                            MPinput.Add([mpID.ToString(), monster_place_csv_data[mpcdIter][0] + areaIter.ToString(),
                                monster_place_csv_data[mpcdIter][1], monster_place_csv_data[mpcdIter][2],
                                rareMons[rareIter][0], monster_place_csv_data[mpcdIter][3], "0"]);
                            areaIter++;
                            rareIter++;
                            mpID++;
                        }
                        // if chainroad
                        if (rareChapterToAreaPairings[ctoaIter][0] == "21_00")
                        {
                            while (rareMons[rareIter][1].Substring(rareMons[rareIter][1].Length - 3) == "001")
                            {
                                MPinput.Add([mpID.ToString(), monster_place_csv_data[mpcdIter][0] + areaIter.ToString(),
                                    monster_place_csv_data[mpcdIter][1], monster_place_csv_data[mpcdIter][2],
                                    rareMons[rareIter][0], monster_place_csv_data[mpcdIter][3], "0"]);
                                areaIter++;
                                rareIter++;
                                mpID++;
                            }
                        }
                        // else if castle exnine
                        else if (rareChapterToAreaPairings[ctoaIter][0] == "21_01")
                        {
                            while (rareMons[rareIter][1].Substring(rareMons[rareIter][1].Length - 3) == "011")
                            {
                                MPinput.Add([mpID.ToString(), monster_place_csv_data[mpcdIter][0] + areaIter.ToString(),
                                    monster_place_csv_data[mpcdIter][1], monster_place_csv_data[mpcdIter][2],
                                    rareMons[rareIter][0], monster_place_csv_data[mpcdIter][3], "0"]);
                                areaIter++;
                                rareIter++;
                                mpID++;
                                // if end of rareIter, it'll cause issues, so break out
                                if (rareIter == rareMons.Count()) break;
                            }
                        }
                        ctoaIter++;
                    }
                    // Write in the exceptions for SewerArea and DeepGround
                    // This is actually for DragonValley, but we put SewerArea in here now
                    if (row[1].Substring(0,8) == "RE_d0600")
                    {
                        MPinput.Add([mpID.ToString(), "SewerArea1", "下海・入口", "16", "7055", "15", "0"]);
                        mpID++;
                        MPinput.Add([mpID.ToString(), "SewerArea2", "下海・入口", "16", "7084", "15", "0"]);
                        mpID++;
                        MPinput.Add([mpID.ToString(), "SewerArea3", "下海・入口", "16", "7079", "15", "0"]);
                        mpID++;
                        MPinput.Add([mpID.ToString(), "SewerArea4", "下海・入口", "16", "7085", "15", "10002000"]);
                        mpID++;
                        MPinput.Add([mpID.ToString(), "SewerArea5", "下海・入口", "17", "7055", "15", "0"]);
                        mpID++;
                        MPinput.Add([mpID.ToString(), "SewerArea6", "下海・入口", "17", "7084", "15", "0"]);
                        mpID++;
                        MPinput.Add([mpID.ToString(), "SewerArea7", "下海・入口", "17", "7079", "15", "0"]);
                        mpID++;
                        MPinput.Add([mpID.ToString(), "SewerArea8", "下海・入口", "17", "7085", "15", "10002000"]);
                        mpID++;
                    }
                    if (row[1].Substring(0,8) == "RE_d1100")
                    {
                        MPinput.Add([mpID.ToString(), "DeepGround1", "ディープグラウンド", "29", "7170", "28", "0"]);
                        mpID++;
                        MPinput.Add([mpID.ToString(), "DeepGround2", "ディープグラウンド", "29", "7171", "28", "0"]);
                        mpID++;
                        areaIter = 3;
                        // Get rare monster(s) for DeepGround while here
                        while (rareMons[rareIter][1].Substring(4, 2) == "14")
                        {
                            MPinput.Add([mpID.ToString(), "DeepGround" + areaIter.ToString(),
                                "ディープグラウンド", "29",
                                rareMons[rareIter][0], "28", "0"]);
                            areaIter++;
                            rareIter++;
                            mpID++;
                        }
                    }
                    // If there are more than 12 rows in an area, need to reduce it down back to 12
                    // to prevent it from breaking the game
                    while (areaIter > 13)
                    {
                        MPinput.RemoveAt(MPinput.Count() - 1);
                        areaIter--;
                    }
                    // Reset areaIter and set the mpcdIter to the next row on the big table above.
                    areaIter = 1;
                    mpcdIter++;
                }
                // If row goes to postgame, break
                if (row[1].Substring(0, 8) == "RE_d2200") break;
                // Otherwise, add a new row
                if (!monsToIgnoreOnMap.Contains(row[0]))
                {
                    MPinput.Add([mpID.ToString(), monster_place_csv_data[mpcdIter][0] + areaIter.ToString(),
                        monster_place_csv_data[mpcdIter][1], monster_place_csv_data[mpcdIter][2],
                        row[0], monster_place_csv_data[mpcdIter][3], "0"]);
                    areaIter++;
                    mpID++;
                }
                prevRowArea = row[1].Substring(0, 8);
                
            }
            CsvHandling.CsvWriteData(mpPath, MPinput);
        }
    }
}
