using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Logging;

namespace WOFFRandomizer
{
    internal class Item
    {
        
        private static List<(uint, uint)> CreateEntryTableList(BinaryReader barcReader, uint fileCount)
        {
            List<(uint, uint)> entryTableList = new List<(uint, uint)> ();

            for (int i = 0; i < fileCount; i++)
            {
                // skip storing hash and unk values
                _ = barcReader.ReadBytes(8);

                // Get position and size offsets
                uint position = barcReader.ReadUInt32();
                uint size = barcReader.ReadUInt32();

                // Store them both in the entry list
                entryTableList.Add((position, size));
            }

            return entryTableList;
        }

        // https://stackoverflow.com/questions/283456/byte-array-pattern-search
        static private List<int> SearchBytePattern(byte[] pattern, byte[] bytes)
        {
            List<int> positions = new List<int>();
            int patternLength = pattern.Length;
            int totalLength = bytes.Length;
            byte firstMatchByte = pattern[0];
            for (int i = 0; i < totalLength; i++)
            {
                if (firstMatchByte == bytes[i] && totalLength - i >= patternLength)
                {
                    byte[] match = new byte[patternLength];
                    Array.Copy(bytes, i, match, 0, patternLength);
                    if (match.SequenceEqual<byte>(pattern))
                    {
                        positions.Add(i);
                        i += patternLength - 1;
                    }
                }
            }
            return positions;
        }

        private static string GetTreasureChestID(byte[] luaFileData)
        {
            // instead of regex, I'm going to use byte searching based on what's 19 spots after " ItemFlag"
            byte[] byteToSearch = new byte[] { 0, 73, 116, 101, 109, 70, 108, 97, 103 };

            List<int> positions = SearchBytePattern(byteToSearch, luaFileData);
            if (positions.Count == 0) return "";
            
            // I just need one of the positions, and then 19 offset from that to get the name of the treasure chest id until 0
            int start = positions[0] + 19; int byteArrIter = positions[0] + 19;
            int tcIDLength = 0;
            while (luaFileData[byteArrIter] != 0)
            {
                byteArrIter++;
                tcIDLength++;
            }
            byte[] tcIDBytes = luaFileData[start..byteArrIter];

            return System.Text.Encoding.UTF8.GetString(tcIDBytes);
        }

        private static (byte[], string) GetItemID(byte[] luaFileData)
        {
            // instead of regex, I'm going to use byte searching based on what's 19 spots after " ItemList"
            byte[] byteToSearch = new byte[] { 0, 73, 116, 101, 109, 76, 105, 115, 116 };
            uint byteSelection = 0;

            // need a byte search for " MagicStone" as well
            byte[] byteToSearchStone = new byte[] { 0, 77, 97, 103, 105, 99, 83, 116, 111, 110, 101 };

            List<int> positions = SearchBytePattern(byteToSearch, luaFileData);
            // if failed, try the other one
            if (positions.Count == 0)
            {
                positions = SearchBytePattern(byteToSearchStone, luaFileData);
                byteSelection = 1;
            }
            if (positions.Count == 0) return (new byte[0], "");

            // I just need one of the positions, and then 19 offset from that to get the ID of the item until 0
            // this is if the byteToSearch is the first one.
            // if it's the second one for Stone, then it's a 21 offset
            int start; int byteArrIter;
            if (byteSelection == 0)
            {
                start = positions[0] + 19; 
                byteArrIter = positions[0] + 19;
            }
            else
            {
                start = positions[0] + 21;
                byteArrIter = positions[0] + 21;
            }
            int itemIDLength = 0;
            while (luaFileData[byteArrIter] != 0)
            {
                byteArrIter++;
                itemIDLength++;
            }
            byte[] itemIDBytes = luaFileData[start..byteArrIter];

            return (itemIDBytes, System.Text.Encoding.UTF8.GetString(itemIDBytes));
        }

        private static void EditBinaryFile(uint position, uint size, byte[] shuffledBytes,
            FileStream barcStream, BinaryReader barcReader, RichTextBox log)
        {
            barcReader.BaseStream.Position = 0;
            barcStream.Position = position;
            byte[] data = new byte[size];
            _ = barcStream.Read(data, 0, data.Length);
            // find position of itemID
            byte[] byteToSearch = new byte[] { 0, 73, 116, 101, 109, 76, 105, 115, 116 };
            byte[] byteToSearchStone = new byte[] { 0, 77, 97, 103, 105, 99, 83, 116, 111, 110, 101 };
            List<int> positions = SearchBytePattern(byteToSearch, data);
            if (System.Text.Encoding.UTF8.GetString(shuffledBytes).StartsWith("Stone")) positions = SearchBytePattern(byteToSearchStone, data);
            barcStream.Position = position;
            // I just need the first position, and then 19 offset from that to get the ID of the item until 0
            // if it's a stone, i need 21 offset from that
            int positionToWriteTo = positions[0] + 19;
            if (System.Text.Encoding.UTF8.GetString(shuffledBytes).StartsWith("Stone")) positionToWriteTo = positions[0] + 21;
            barcStream.Position += positionToWriteTo;
            for (int j = 0; j < shuffledBytes.Length; j++)
            {
                barcStream.WriteByte(shuffledBytes[j]);
                // don't need to edit the last 4 bytes anymore. should be good now
                if (j == shuffledBytes.Length) break;
            }
        }

        private static (List<(string, (byte[], string))>, List<(string, (byte[], string))>, List<(string, (byte[], string))>) 
            CollectTreasureList(string scriptBinary, RichTextBox log)
        {
            List<(string, (byte[], string))> itemList = new List<(string, (byte[], string))>();
            List<(string, (byte[], string))> abList = new List<(string, (byte[], string))>();
            List<(string, (byte[], string))> stoneList = new List<(string, (byte[], string))>();
            int gimmick_itemboxCount = 0;
            // traverse binary file and grab only data in lua files that contain filename portion "gimmick_itembox", i think
            // if the next "gimmick_itembox" name matches the last one, that means they're from the same set. ignore it.
            // only changing the first item type in that set matters

            // many thanks to Surihix for helping with FileStream and BinaryReader, specifically
            using (FileStream barcStream = new FileStream(scriptBinary, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader barcReader = new BinaryReader(barcStream))
                {
                    barcReader.BaseStream.Position = 0;

                    // read header values
                    string barcMagic = System.Text.Encoding.UTF8.GetString(barcReader.ReadBytes(4));
                    if (barcMagic != "BARC")
                    {
                        log.AppendText("Invalid BARC file.\n");
                        return (itemList, abList, stoneList);
                    }

                    uint fileCount = barcReader.ReadUInt32();

                    // Create a list for the entry table
                    List<(uint, uint)>entryTableList =  CreateEntryTableList(barcReader, fileCount);

                    // Iterate through files, finding the ones with "gimmick_itembox" in filename
                    string searchString = "gimmick_itembox";

                    for (int i = 0; i < fileCount; i++)
                    {
                        uint position = entryTableList[i].Item1;
                        uint size = entryTableList[i].Item2;

                        barcStream.Position = position;
                        byte [] luaFileData = new byte[size];
                        _ = barcStream.Read(luaFileData, 0, luaFileData.Length);
                        // convert the byte data to a string to search for the byte data
                        string luaFileString = System.Text.Encoding.UTF8.GetString(luaFileData);

                        // if the filedata contains searchString, then it contains information about a treasure chest
                        // however, we want to only get the first instance of each treasure, so need to check for duplicates

                        // item type can vary, so will do another search for that

                        // Items and AbilityBooks have "ItemList" before it, while Stones have "MagicStone" before it.

                        if (luaFileString.Contains(searchString))
                        {
                            string treasureChestID = GetTreasureChestID(luaFileData);
                            byte[] itemBytes = new byte[] { };
                            string itemID = "";
                            (itemBytes, itemID) = GetItemID(luaFileData);
                            
                            if (treasureChestID == "" | itemID == "") continue;
                            // skip EX dungeon content, which is postgame content
                            else if (treasureChestID.StartsWith("EX")) continue;
                            // skip Deep Ground, or Underground Prison, content. i want that to be vanilla, since players may struggle there without vanilla items
                            else if (treasureChestID.StartsWith("d0010")) continue;
                            // I believe Despellstones are the Gimme Golem items. Not 100% sure, but based on how they're named in Japanese, I would guess so.
                            else if (itemID.StartsWith("Despell")) continue;
                            // if it's a duplicate lua file entry, skip it.
                            if (!itemList.Exists(item => item.Item1 == treasureChestID) && !abList.Exists(item => item.Item1 == treasureChestID)
                                && !stoneList.Exists(item => item.Item1 == treasureChestID))
                            {
                                if (itemID.StartsWith("Item")) itemList.Add((treasureChestID, (itemBytes, itemID)));
                                else if (itemID.StartsWith("Ability")) abList.Add((treasureChestID, (itemBytes, itemID)));
                                else if (itemID.StartsWith("Stone"))
                                {
                                    stoneList.Add((treasureChestID, (itemBytes, itemID)));
                                }
                            }
                            gimmick_itemboxCount++;
                        }
                    }
                }
            }
                return (itemList, abList, stoneList);
        }

        private static void InsertShuffledTreasureList((List<(string, (byte[], string))>, List<(string, (byte[], string))>, List<(string, (byte[], string))>) 
            shuffledTreasureList, string scriptBinary, RichTextBox log)
        {
            // traverse binary file and grab only data in lua files that contain filename portion "gimmick_itembox", i think
            // if the next "gimmick_itembox" name matches the last one, that means they're from the same set. ignore it.
            // only changing the first item type in that set matters

            // many thanks to Surihix for helping with FileStream and BinaryReader, specifically
            using (FileStream barcStream = new FileStream(scriptBinary, FileMode.Open, FileAccess.ReadWrite))
            {
                using (BinaryReader barcReader = new BinaryReader(barcStream))
                {
                    barcReader.BaseStream.Position = 0;

                    // read header values
                    string barcMagic = System.Text.Encoding.UTF8.GetString(barcReader.ReadBytes(4));
                    if (barcMagic != "BARC")
                    {
                        log.AppendText("Invalid BARC file.\n");
                        return;
                    }

                    uint fileCount = barcReader.ReadUInt32();

                    // Create a list for the entry table
                    List<(uint, uint)> entryTableList = CreateEntryTableList(barcReader, fileCount);

                    // Iterate through files, finding the ones with "gimmick_itembox" in filename
                    string searchString = "gimmick_itembox";
                    int itemListIter = 0; int abListIter = 0; int stoneListIter = 0;

                    for (int i = 0; i < fileCount; i++)
                    {
                        uint position = entryTableList[i].Item1;
                        uint size = entryTableList[i].Item2;

                        barcStream.Position = position;
                        byte[] luaFileData = new byte[size];
                        _ = barcStream.Read(luaFileData, 0, luaFileData.Length);
                        // convert the byte data to a string to search for the byte data
                        string luaFileString = System.Text.Encoding.UTF8.GetString(luaFileData);

                        // if the filedata contains searchString, then it contains information about a treasure chest
                        // however, we want to only get the first instance of each treasure, so need to check for duplicates

                        // item type can vary, so will do another search for that
                        
                        if (luaFileString.Contains(searchString))
                        {
                            string treasureChestID = GetTreasureChestID(luaFileData);
                            byte[] itemBytes = new byte[] { };
                            string itemID = "";
                            (itemBytes, itemID) = GetItemID(luaFileData);

                            if (treasureChestID == "" | itemID == "") continue;
                            // skip EX dungeon content, which is postgame content
                            else if (treasureChestID.StartsWith("EX")) continue;
                            // skip Deep Ground, or Underground Prison, content. i want that to be vanilla, since players may struggle there without vanilla items
                            else if (treasureChestID.StartsWith("d0010")) continue;
                            // I believe Despellstones are the Gimme Golem items. Not 100% sure, but based on how they're named in Japanese, I would guess so.
                            else if (itemID.StartsWith("Despell")) continue;
                            // everything is mostly the same as the CollectTreasureList function except this
                            // i need to now replace the items carefully in the binary
                            // i have to randomize items with items/abilitybooks with abilitybooks
                            // forgot stones, or mirajewels. need to count those as well
                            if (itemListIter < shuffledTreasureList.Item1.Count)
                            {
                                if (treasureChestID == shuffledTreasureList.Item1[itemListIter].Item1)
                                {
                                    EditBinaryFile(position, size, shuffledTreasureList.Item1[itemListIter].Item2.Item1, barcStream, barcReader, log);
                                    
                                    itemListIter++;
                                }
                            }

                            if (abListIter < shuffledTreasureList.Item2.Count)
                            {
                                if (treasureChestID == shuffledTreasureList.Item2[abListIter].Item1)
                                {
                                    EditBinaryFile(position, size, shuffledTreasureList.Item2[abListIter].Item2.Item1, barcStream, barcReader, log);
                                    
                                    abListIter++;
                                }
                            }
                            if (stoneListIter < shuffledTreasureList.Item3.Count)
                            {
                                if (treasureChestID == shuffledTreasureList.Item3[stoneListIter].Item1)
                                {
                                    EditBinaryFile(position, size, shuffledTreasureList.Item3[stoneListIter].Item2.Item1, barcStream, barcReader, log);

                                    stoneListIter++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void createItemLog(string currDir, List<(string, (byte[], string))> shuffledItemList, 
            List<(string, (byte[], string))> shuffledABList, List<(string, (byte[], string))> shuffledStoneList)
        {
            string toWrite = "";
            // function for checking what items are getting put in the shuffled list
            List<(string, string)> combinedShuffledTreasureList = new List<(string, string)>();
            for (int i = 0; i < shuffledItemList.Count; i++)
            {
                combinedShuffledTreasureList.Add((shuffledItemList[i].Item1, System.Text.Encoding.UTF8.GetString(shuffledItemList[i].Item2.Item1)));
            }
            for (int i = 0; i < shuffledABList.Count; i++)
            {
                combinedShuffledTreasureList.Add((shuffledABList[i].Item1, System.Text.Encoding.UTF8.GetString(shuffledABList[i].Item2.Item1)));
            }
            for (int i = 0; i < shuffledStoneList.Count; i++)
            {
                combinedShuffledTreasureList.Add((shuffledStoneList[i].Item1, System.Text.Encoding.UTF8.GetString(shuffledStoneList[i].Item2.Item1)));
            }

            combinedShuffledTreasureList.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            for (int i = 0; i < combinedShuffledTreasureList.Count; i++)
            {
                toWrite += combinedShuffledTreasureList[i].Item1 + ": " + combinedShuffledTreasureList[i].Item2 + "\n";
            }

            System.IO.File.WriteAllText(Path.Combine(currDir, "logs", "item_log.txt"), toWrite);

            // todo - function for relabeling ITEM, STONE, and ABILITYBOOK with names from database
            // create master list from database/items.txt and database/mirajewels.txt to compare the item_log.txt with and replace
            List<(string, string)> masterList = new List<(string, string)>();
            using (var sr = new StreamReader(Path.Combine(currDir, "database", "items.txt")))
            {
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    var splitted = line.Split('\t', 2);
                    masterList.Add((splitted[0], splitted[1]));
                }
            }
            using (var sr = new StreamReader(Path.Combine(currDir, "database", "mirajewels.txt")))
            {
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    var splitted = line.Split('\t', 2);
                    masterList.Add((splitted[0], splitted[1]));
                }
            }
            // after creating master list, change variables in item_log.txt by doing a replace
            toWrite = "";
            using (var sr = new StreamReader(Path.Combine(currDir, "logs", "item_log.txt")))
            {
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    for (int i = 0; i < masterList.Count; i++)
                    {
                        if (line.Contains(masterList[i].Item1))
                        {
                            var splitted = line.Split(' ', 2);
                            line = line.Replace(splitted[1], masterList[i].Item2);
                            toWrite += line + Environment.NewLine;
                        }
                    }
                }
            }
            System.IO.File.WriteAllText(Path.Combine(currDir, "logs", "item_log.txt"), toWrite);
        }
        public static void TreasureShuffle(string currDir, string sV, RichTextBox log)
        {
            log.AppendText("Shuffling treasures...\n");
            // can only shuffle item types. counts correspond to the number of lua files in the bin
            // for example, 3 potions means three different lua files for one treasure chest
            // not going to compress to lua and back. will just modify the bin directly, if possible
            string scriptBinary = Path.Combine(currDir, "script64.bin");
            (List <(string, (byte[], string))>, List<(string, (byte[], string))>, List<(string, (byte[], string))>) 
                treasureList = CollectTreasureList(scriptBinary, log);

            // Shuffle just the items/abilitybooks, not the treasureIDs
            // split the list into two separate lists, then join them back up
            // I actually need to group the abilitybooks with abilitybooks and items with items
            List<string> treasureIDsForItems = new List<string>();
            List<string> treasureIDsForABs = new List<string>();
            List<string> treasureIDsForStones = new List<string>();
            List<(byte[], string)> itemIDs = new List<(byte[], string)>();
            List<(byte[], string)> abIDs = new List<(byte[], string)>();
            List<(byte[], string)> stoneIDs = new List<(byte[], string)>();
            foreach (var treasure in treasureList.Item1)
            {
                treasureIDsForItems.Add(treasure.Item1);
                itemIDs.Add(treasure.Item2);
            }
            foreach (var treasure in treasureList.Item2)
            {
                treasureIDsForABs.Add(treasure.Item1);
                abIDs.Add(treasure.Item2);
            }
            foreach (var treasure in treasureList.Item3)
            {
                treasureIDsForStones.Add(treasure.Item1);
                stoneIDs.Add(treasure.Item2);
            }
            itemIDs.Shuffle(Shuffle.ConsistentStringHash(sV));
            abIDs.Shuffle(Shuffle.ConsistentStringHash(sV));
            stoneIDs.Shuffle(Shuffle.ConsistentStringHash(sV));
            List<(string, (byte[], string))> shuffledItemList = new List<(string, (byte[], string))> ();
            List<(string, (byte[], string))> shuffledABList = new List<(string, (byte[], string))>();
            List<(string, (byte[], string))> shuffledStoneList = new List<(string, (byte[], string))>();

            for (int i = 0; i < itemIDs.Count; i++)
            {
                shuffledItemList.Add((treasureIDsForItems[i], itemIDs[i]));
            }
            for (int i = 0; i < abIDs.Count; i++)
            {
                shuffledABList.Add((treasureIDsForABs[i], abIDs[i]));
            }
            for (int i = 0; i < stoneIDs.Count; i++)
            {
                shuffledStoneList.Add((treasureIDsForStones[i], stoneIDs[i]));
            }

            (List<(string, (byte[], string))>, List<(string, (byte[], string))>, List<(string, (byte[], string))>) 
                shuffledTreasureList = (shuffledItemList, shuffledABList, shuffledStoneList);

            InsertShuffledTreasureList(shuffledTreasureList, scriptBinary, log);

            createItemLog(currDir, shuffledItemList, shuffledABList, shuffledStoneList);


        }
    }
}
