using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOFFRandomizer
{
    internal class CsvHandling
    {
        // NOTE: THIS IS FOR OLDER IMPLEMENTATIONS FOR CSHTOOLHELPERS.
        // YOU WILL NEED TO ACCOUNT FOR INSERTED ROW AT BEGINNING
        // WITH DIFFERENT FUNCTION
        public static List<List<string>> CsvReadData(string path)
        {
            List<List<string>> csvData = new List<List<string>>();
            var csvFile = File.ReadAllLines(path, Encoding.UTF8);
            var output = new List<string>(csvFile);
            foreach (var row in output)
            {
                List<string> listCsv = row.Split("|").ToList();
                csvData.Add(listCsv);
            }
            csvData.RemoveAt(0);

            return csvData;
        }

        public static List<List<string>> CsvReadDataIncHeadRow(string path)
        {
            List<List<string>> csvData = new List<List<string>>();
            var csvFile = File.ReadAllLines(path, Encoding.UTF8);
            var output = new List<string>(csvFile);
            foreach (var row in output)
            {
                List<string> listCsv = row.Split("|").ToList();
                csvData.Add(listCsv);
            }

            return csvData;
        }
        public static void CsvWriteData(string path, List<List<string>> csvData)
        {
            string toWrite = "";
            for (int i = 0; i < csvData.Count; i++)
            {
                toWrite += string.Join("|", csvData[i]) + Environment.NewLine;
            }
            File.WriteAllText(path, toWrite);
        }
        public static void CsvWriteDataAddHeadRow(string path, List<List<string>> csvData, int count)
        {
            string toWrite = "";
            // Add 0xA0 strings based on count of columns for first row
            for (int i = 0; i < count; i++)
            {
                toWrite += "0xA0";
                if (i == count - 1) toWrite += Environment.NewLine;
                else toWrite += "|";
            }
            for (int i = 0; i < csvData.Count; i++)
            {
                toWrite += string.Join("|", csvData[i]) + Environment.NewLine;
            }
            File.WriteAllText(path, toWrite);
        }

    }
}
