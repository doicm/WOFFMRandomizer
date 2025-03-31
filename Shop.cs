using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WOFFRandomizer
{
    internal class Shop
    {
        public static void putEldboxInShops(string currDir, RichTextBox log)
        {
            log.AppendText("Adding eldboxes to shops...\n");
            // Get filename
            string csvfilename = Path.Combine(currDir, "shop_list.csv");

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
            // Convert List<List<string>> back to List<string> output
            output = new List<string>();
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
            System.IO.File.WriteAllText(currDir + "/shop_list.csv", newFileOutput);
        }
    }
}
