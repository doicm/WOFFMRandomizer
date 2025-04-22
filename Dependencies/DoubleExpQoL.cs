using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOFFRandomizer.Dependencies
{
    internal class DoubleExpQoL
    {
        private static string MultiplyStrByTwo(string value)
        {
            int doubledInt = Int32.Parse(value) * 2;
            return doubledInt.ToString();
        }
        public static void DoubleExpGil (string currDir, RichTextBox log)
        {
            log.AppendText("Doubling experience and gil rewards...\n");

            string ceslPath = Path.Combine(currDir, "character_enemy_status_list.csv");
            
            // Read the data in

            List<List<string>> csvData = CsvHandling.CsvReadData(ceslPath);

            // Double the exp and gil datas. 79, 80, 81, and 82 are the columns, both for NG and NG+
            foreach(var row in csvData)
            {
                row[79] = MultiplyStrByTwo(row[79]);
                row[80] = MultiplyStrByTwo(row[80]);
                row[81] = MultiplyStrByTwo(row[81]);
                row[82] = MultiplyStrByTwo(row[82]);
            }

            // Write the data back in
            CsvHandling.CsvWriteDataAddHeadRow(ceslPath, csvData, 84);
        }
    }
}
