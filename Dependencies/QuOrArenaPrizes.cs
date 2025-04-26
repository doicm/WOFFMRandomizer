using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace WOFFRandomizer.Dependencies
{
    internal class QuOrArenaPrizes
    {
        private static List<(string, List<(string, string)>)> CollectRewardsList (string interventionPath, string arenaPath)
        {
            List<(string, List<(string, string)>)> rewardsList = new List<(string, List<(string, string)>)> ();
            
            // First, put everything into a list of csvs. Start with interventionPath, then do arenaPath
            // it's going to be easier to put them into two separate lists
            List<List<string>> csvDataIntervention = CsvHandling.CsvReadData(interventionPath);
            List<List<string>> csvDataArena = CsvHandling.CsvReadData(arenaPath);

            List<int> interventionException = [16, 17, 26, 27, 30, 31, 36, 38];
            int interventionIter = 0;
            // Go up to 49 for intervention quests. After that, it's either repeats or NPC quests
            while (interventionIter < 50)
            {
                if (interventionException.Contains(interventionIter))
                {
                    interventionIter++;
                    continue;
                }
                List<string> row = csvDataIntervention[interventionIter];
                string name = row[1];
                List<(string, string)> itemList = new List<(string, string)> ();
                for (int itemIter = 0; itemIter < 3; itemIter++)
                {
                    if (row[3 + (4 * itemIter)] == "-1") continue; // if item slot is unused, move on
                    (string, string) item = (row[4 + (4 * itemIter)], row[5 + (4 * itemIter)]);
                    itemList.Add(item);
                }
                rewardsList.Add((name, itemList));
                interventionIter++;
            }

            List<int> arenaInclusion = [1,3,5,13,17,23,27,29,35,71,73,95,99,198,199,200,201,202,203,204,205,206,207,208,209,
            210,211,212,213,214,215,216,217,218,219,220,221,222,224,226,228,230];
            int arenaIter = 0;
            // Can go over whole list for arena. Just going to include rather than exclude for arena, since there's a large
            // number for arena list and not many comparatively included.
            while (arenaIter < csvDataArena.Count)
            {
                if (!arenaInclusion.Contains(arenaIter))
                {
                    arenaIter++;
                    continue;
                }
                List<string> row = csvDataArena[arenaIter];
                string name = row[1];
                (string, string) item = (row[3], row[4]);
                List<(string, string)> itemList = new List<(string, string)>();
                itemList.Add(item);
                rewardsList.Add((name, itemList));
                arenaIter++;
            }
            return rewardsList;
        }

        private static List<(string, string)> GetIndividualRewards(List<(string, List<(string, string)>)> rewardsList)
        {
            List<(string, string)> allIndividualRewards = new List<(string , string)>();

            foreach (var reward in rewardsList)
            {
                foreach (var prize in reward.Item2)
                {
                    allIndividualRewards.Add(prize);
                }
            }
            return allIndividualRewards;
        }

        private static void ShuffleRewardsList(string currDir, string interventionPath, string arenaPath, List<(string, string)> allIndividualRewards)
        {
            // First, put everything into a list of csvs. Start with interventionPath, then do arenaPath
            // it's going to be easier to put them into two separate lists
            List<List<string>> csvDataIntervention = CsvHandling.CsvReadData(interventionPath);
            List<List<string>> csvDataArena = CsvHandling.CsvReadData(arenaPath);
            // iterator for list allIndividualRewards
            int aIRIter = 0;

            List<int> interventionException = [16, 17, 26, 27, 30, 31, 36, 38];
            // New cshtoolhelpers adds a row at the beginning. Get rid of that first row for both read rows
            int interventionIter = 0;
            // Go up to 49 for intervention quests. After that, it's either repeats or NPC quests
            while (interventionIter < 50)
            {
                if (interventionException.Contains(interventionIter))
                {
                    interventionIter++;
                    continue;
                }
                // write the new information in
                for (int itemIter = 0; itemIter < 3; itemIter++)
                {
                    if (csvDataIntervention[interventionIter][3 + (4 * itemIter)] == "-1") continue; // if item slot is unused, move on
                    (csvDataIntervention[interventionIter][4 + (4 * itemIter)], csvDataIntervention[interventionIter][5 + (4 * itemIter)])
                        = (allIndividualRewards[aIRIter].Item1, allIndividualRewards[aIRIter].Item2);
                    csvDataIntervention[interventionIter][6 + (4 * itemIter)] = "0"; // Set items, even mementos, to unhidden
                    aIRIter++;
                }
                interventionIter++;
            }

            List<int> arenaInclusion = [1,3,5,13,17,23,27,29,35,71,73,95,99,198,199,200,201,202,203,204,205,206,207,208,209,
            210,211,212,213,214,215,216,217,218,219,220,221,222,224,226,228,230];
            int arenaIter = 0;
            // Can go over whole list for arena. Just going to include rather than exclude for arena, since there's a large
            // number for arena list and not many comparatively included.
            while (arenaIter < csvDataArena.Count)
            {
                if (!arenaInclusion.Contains(arenaIter))
                {
                    arenaIter++;
                    continue;
                }
                (csvDataArena[arenaIter][3], csvDataArena[arenaIter][4]) = (allIndividualRewards[aIRIter].Item1, allIndividualRewards[aIRIter].Item2);
                aIRIter++;
                arenaIter++;
            }

            // Write the new data to the csv files.
            CsvHandling.CsvWriteDataAddHeadRow(interventionPath, csvDataIntervention, 15);
            CsvHandling.CsvWriteDataAddHeadRow(arenaPath, csvDataArena, 11);

            // Write data to a log
            WriteToQuestLog(currDir, csvDataIntervention, csvDataArena);
        }

        private static void WriteToQuestLog(string currDir, List<List<string>> csvDataIntervention, List<List<string>> csvDataArena)
        {
            // Convert what's read in the file to a list to read in of the database files
            List<string> arenaDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "arena.txt"))];
            List<string> interventionDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "intervention.txt"))];
            List<string> itemsDB = [.. File.ReadAllLines(Path.Combine(currDir, "database", "items.txt"))];

            using (var sw = new StreamWriter(Path.Combine(currDir, "logs", "quest_log.txt")))
            {
                // Start with interventions. Only write the ones that are randomized
                sw.WriteLine("Interventions:");
                List<int> interventionException = [16, 17, 26, 27, 30, 31, 36, 38];
                int interventionIter = 0;
                // Go up to 49 for intervention quests. After that, it's either repeats or NPC quests
                while (interventionIter < 50)
                {
                    if (interventionException.Contains(interventionIter))
                    {
                        interventionIter++;
                        continue;
                    }
                    List<string> row = csvDataIntervention[interventionIter];
                    // Get name
                    int intervID = interventionDB.FindIndex(x => x.Split("\t")[0] == row[1]);
                    string intervName = interventionDB[intervID].Split("\t")[1];
                    // Get reward(s)
                    for (int itemIter = 0; itemIter < 3; itemIter++)
                    {
                        if (row[3 + (4 * itemIter)] == "-1") continue; // if item slot is unused, move on
                        int itemID = Int32.Parse(row[4 + (4 * itemIter)]);
                        string itemName = itemsDB[itemID].Split("\t")[1];
                        string itemCount = row[5 + (4 * itemIter)];
                        sw.WriteLine(intervName + ": " + itemName + " (" + itemCount + ")");
                    }
                    interventionIter++;
                }
                sw.WriteLine("---");
                // Go to Coliseum/Arena next
                sw.WriteLine("Coliseum Battles:");
                List<int> arenaInclusion = [1,3,5,13,17,23,27,29,35,71,73,95,99,198,199,200,201,202,203,204,205,206,207,208,209,
                    210,211,212,213,214,215,216,217,218,219,220,221,222,224,226,228,230];
                int arenaIter = 0;
                while (arenaIter < csvDataArena.Count)
                {
                    if (!arenaInclusion.Contains(arenaIter))
                    {
                        arenaIter++;
                        continue;
                    }
                    List<string> row = csvDataArena[arenaIter];
                    // Get name
                    int arenaID = arenaDB.FindIndex(x => x.Split("\t")[0] == row[1]);
                    string arenaName = arenaDB[arenaID].Split("\t")[1];
                    // Get reward
                    int itemID = Int32.Parse(row[3]);
                    string itemName = itemsDB[itemID].Split("\t")[1];
                    string itemCount = row[4];
                    sw.WriteLine(arenaName + ": " + itemName + " (" + itemCount + ")");

                    arenaIter++;
                }
            }
        }
        public static void PrizesShuffle(string currDir, string sV, RichTextBox log)
        {
            log.AppendText("Shuffling intervention and arena rewards...\n");
            // The goal is to take the accessible (up to level 60) rewards from each of the boards
            // and combine them, then shuffle them, then distribute them back into their reward tables
            // I won't do repeat quests, just the first time quests.
            string interventionPath = Path.Combine(currDir, "quest_data_sub_reward_table_list.csv");
            string arenaPath = Path.Combine(currDir, "arena_reward_table_list.csv");

            // This list will consist of a name to ID the intervention quest/arena name it came from and 
            // a list of the groups of prize/count pairs. Most lists will only have one item in them.
            List<(string, List<(string, string)>)> rewardsList = CollectRewardsList(interventionPath, arenaPath);

            // I realized that this will return the original structure of the rewards I need to shuffle
            // However, I want to shuffle all the rewards in them, not just the structures, so I need to collect
            // the individual rewards in them, so I'm going to iterate and collect all the rewards, then shuffle those,
            // then put them back in to their original structure. I think that'll be best
            List<(string, string)> allIndividualRewards = GetIndividualRewards(rewardsList);

            // Shuffle them, then write them back in based on structure
            // The first step might feel like a waste, but it was necessary to make sure I didn't miss necessary or
            // include unnecessary rows.
            allIndividualRewards.Shuffle(Shuffle.ConsistentStringHash(sV));

            // Write the shuffled results back in.
            ShuffleRewardsList(currDir, interventionPath, arenaPath, allIndividualRewards);
        }
    }
}
