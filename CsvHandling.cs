using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOFFRandomizer
{
    internal class CsvHandling
    {
        public static List<List<string>> CsvReadData(string path)
        {
            List<List<string>> csvData = new List<List<string>>();
            var csvFile = File.ReadAllLines(path, Encoding.UTF8);
            var output = new List<string>(csvFile);
            foreach (var row in output)
            {
                List<string> listCsv = row.Split(",").ToList();
                csvData.Add(listCsv);
            }

            return csvData;
        }
        public static void CsvWriteData(string path, List<List<string>> csvData)
        {
            string toWrite = "";
            for (int i = 0; i < csvData.Count; i++)
            {
                toWrite += string.Join(",", csvData[i]) + Environment.NewLine;
            }
            File.WriteAllText(path, toWrite);
        }
    }
}
