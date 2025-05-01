using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WOFFRandomizer.Dependencies
{
    internal class Shop
    {
        public static void putEldboxInShops(string currDir, RichTextBox log)
        {
            log.AppendText("Adding eldboxes to shops...\n");
            // Get filename
            string csvfilename = Path.Combine(currDir, "shop_list.csv");

            // Convert each row string to a row list
            List<List<string>> listListCsv = CsvHandling.CsvReadData(csvfilename);

            // Eldbox is item ID 520. Put that into shops that don't have it
            foreach (var row in listListCsv)
            {
                if (row.Contains("520"))
                {
                    continue;
                }
                int i = 0;
                foreach (var element in row)
                {
                    if (element == "-1")
                    {
                        row[i] = "520";
                        break;
                    }
                    i += 1;
                }
            }

            CsvHandling.CsvWriteDataAddHeadRow(csvfilename, listListCsv, 121);
        }

        public static void RemoveT2AttackItems(string currDir, RichTextBox log)
        {
            log.AppendText("Removing Tier 2 attack items from shop...\n");
            string slPath = Path.Combine(currDir, "shop_list.csv");
            List<List<string>> slData = CsvHandling.CsvReadData(slPath);

            // Remove T2 attack item data if it meets the criteria
            // Bomb Core = 25
            // Lightning Marble = 28
            // Solid Frigicite = 31
            // Dragon Scale = 34
            // Dragon Wing = 37
            // Earth Hammer = 40
            List<string> T2List = ["25", "28", "31", "34", "37", "40"];
            foreach (List<string> row in slData)
            {
                for (int i = 3; i < row.Count; i++)
                {
                    if (T2List.Contains(row[i]))
                    {
                        row[i] = "-1";
                    }
                }
            }


            CsvHandling.CsvWriteDataAddHeadRow(slPath, slData, 121);
        }
    }
}
