using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOFFRandomizer.Dependencies
{
    internal class Stats
    {
        private static float CheckFloatOrIntAndConvert (string s)
        {
            // If it's ends with an f, return it as a float
            if (s.EndsWith("f")) return float.Parse(s.Substring(0, s.Length - 1));
            else // If it doesn't end with an f, it's an int. Convert to a float
            {
                int i = Int32.Parse(s);
                return (float) i;
            }
        }

        //private static int GetHPRank (string s)
        //{
        //    float f = ConvertStrToFloat(s);
        //    // Range is between 100 to 190 for S to L, with 69 as outlier for Skull Eater
        //    // Will make separate class for XL
        //    float min = 100;
        //    float minMaxDiff = 190 - min;
        //    if (f < min + (minMaxDiff / 5)) return 1;
        //    else if (f < min + 2 * (minMaxDiff / 5)) return 2;
        //    else if (f < min + 3 * (minMaxDiff / 5)) return 3;
        //    else if (f < min + 4 * (minMaxDiff / 5)) return 4;
        //    else return 5;
        //}

        //private static int GetStrRank(string s)
        //{
        //    float f = ConvertStrToFloat(s);
        //    // Range is between 11 to 20 for S to XL
        //    float min = 11;
        //    float minMaxDiff = 20 - min;
        //    if (f < min + (minMaxDiff / 5)) return 1;
        //    else if (f < min + 2 * (minMaxDiff / 5)) return 2;
        //    else if (f < min + 3 * (minMaxDiff / 5)) return 3;
        //    else if (f < min + 4 * (minMaxDiff / 5)) return 4;
        //    else return 5;
        //}

        //private static int GetDefRank(string s)
        //{
        //    float f = ConvertStrToFloat(s);
        //    // Range is between 12 to 22.5 for S to XL
        //    float min = 12;
        //    float minMaxDiff = (float)(19 - min);
        //    if (f < min + (minMaxDiff / 5)) return 1;
        //    else if (f < min + 2 * (minMaxDiff / 5)) return 2;
        //    else if (f < min + 3 * (minMaxDiff / 5)) return 3;
        //    else if (f < min + 4 * (minMaxDiff / 5)) return 4;
        //    else return 5;
        //}

        //private static int GetMagRank(string s)
        //{
        //    float f = ConvertStrToFloat(s);
        //    // Range is between 11.5 to 20.5 for S to XL
        //    float min = (float)11;
        //    float minMaxDiff = (float)(20.0 - min);
        //    if (f < min + (minMaxDiff / 5)) return 1;
        //    else if (f < min + 2 * (minMaxDiff / 5)) return 2;
        //    else if (f < min + 3 * (minMaxDiff / 5)) return 3;
        //    else if (f < min + 4 * (minMaxDiff / 5)) return 4;
        //    else return 5;
        //}

        //private static int GetMDefRank(string s)
        //{
        //    float f = ConvertStrToFloat(s);
        //    // Range is between 11.5 to 20.5 for S to XL
        //    float min = (float)12;
        //    float minMaxDiff = (float)(19 - min);
        //    if (f < min + (minMaxDiff / 5)) return 1;
        //    else if (f < min + 2 * (minMaxDiff / 5)) return 2;
        //    else if (f < min + 3 * (minMaxDiff / 5)) return 3;
        //    else if (f < min + 4 * (minMaxDiff / 5)) return 4;
        //    else return 5;
        //}

        //private static int GetAgiRank(string s)
        //{
        //    float f = ConvertStrToFloat(s);
        //    // Range is between 11.5 to 20.5 for S to XL
        //    float min = (float)0.09;
        //    float minMaxDiff = (float)(0.16 - min);
        //    if (f < min + (minMaxDiff / 5)) return 1;
        //    else if (f < min + 2 * (minMaxDiff / 5)) return 2;
        //    else if (f < min + 3 * (minMaxDiff / 5)) return 3;
        //    else if (f < min + 4 * (minMaxDiff / 5)) return 4;
        //    else return 5;
        //}
        private static List<string> SetStats(List<string> row, Random fixRand)
        {
            // Get stats from each column. It's growths of HP, Str, Def, Mag, MDef, and Agi
            // They are represented as floats with an "f" at the end. For now, I'll store as strings
            // ...sometimes it's a float, and sometimes it's an int
            float hpGrowth = CheckFloatOrIntAndConvert(row[22]);
            float strGrowth = CheckFloatOrIntAndConvert(row[23]);
            float defGrowth = CheckFloatOrIntAndConvert(row[24]);
            float magGrowth = CheckFloatOrIntAndConvert(row[25]);
            float mdefGrowth = CheckFloatOrIntAndConvert(row[26]);
            float agiGrowth = CheckFloatOrIntAndConvert(row[27]);

            // make exceptions for XL, flans, skull eater
            List<string> XLList = ["7038", "7044", "7056", "7070", "7082", "7085", "7126", "7154", "7175", "7176", 
                "7190", "7213", "7214", "7215", "7216"];
            List<string> flanList = ["7112", "7114", "7115", "7116"];
            string skullEater = "7074";

            // Get "BST." BST is based on some sort of average that needs to be calculated based on
            // each stat's average
            float hpMin, hpMax, strMin, strMax, defMin, defMax;
            float magMin, magMax, mdefMin, mdefMax, agiMin, agiMax;
            if (row[0] == skullEater)
            {
                hpMin = (float)69; hpMax = (float)190;
            }
            else if (XLList.Contains(row[0]))
            {
                hpMin = (float)870; hpMax = (float)999;
            }
            else
            {
                hpMin = (float)100; hpMax = (float)190;
            }
            strMin = (float) 11; strMax = (float)20;
            defMin = (float) 12; defMax = (float)19;
            magMin = (float) 11; magMax = (float)20;
            if (flanList.Contains(row[0]))
            {
                mdefMin = (float)7.5; mdefMax = (float)19;
            }
            else
            {
                mdefMin = (float)12; mdefMax = (float)19;
            }
            agiMin = (float) 0.09; agiMax = (float)0.17;

            double hpAverage = (hpGrowth - hpMin) / (hpMax - hpMin);
            double strAverage = (strGrowth - strMin) / (strMax - strMin);
            double defAverage = (defGrowth - defMin) / (defMax - defMin);
            double magAverage = (magGrowth - magMin) / (magMax - magMin);
            double mdefAverage = (mdefGrowth - mdefMin) / (mdefMax - mdefMin);
            double agiAverage = (agiGrowth - agiMin) / (agiMax - agiMin);
            double targetBSTAve = hpAverage + strAverage + defAverage + magAverage + mdefAverage + agiAverage;

            // Generate 6 random doubles to make weightings
            double hpW = fixRand.NextDouble();
            double strW = fixRand.NextDouble();
            double defW = fixRand.NextDouble();
            double magW = fixRand.NextDouble();
            double mdefW = fixRand.NextDouble();
            double agiW = fixRand.NextDouble();

            double totW = hpW + strW + defW + magW + mdefW + agiW;

            // targetBST * difference * (statWeight / totalWeight) + statMinimum...?
            hpGrowth = (float) (targetBSTAve * (hpMax-hpMin) * (hpW / totW) + hpMin); // Skull Eater 69 outlier not acknowledged
            strGrowth = (float) (targetBSTAve * (strMax-strMin) * (strW / totW) + strMin);
            defGrowth = (float) (targetBSTAve * (defMax-defMin) * (defW / totW) + defMin);
            magGrowth = (float) (targetBSTAve * (magMax-magMin) * (magW / totW) + magMin);
            mdefGrowth = (float) (targetBSTAve * (mdefMax-mdefMin) * (mdefW / totW) + mdefMin); // Flan 7.5 outlier not acknowledged yet
            agiGrowth = (float) (targetBSTAve * (agiMax-agiMin) * (agiW / totW) + agiMin);

            row[22] = hpGrowth.ToString() + "f";
            row[23] = strGrowth.ToString() + "f";
            row[24] = defGrowth.ToString() + "f";
            row[25] = magGrowth.ToString() + "f";
            row[26] = mdefGrowth.ToString() + "f";
            row[27] = agiGrowth.ToString() + "f";

            // For reference, the maximum amounts are 40 * all growths except agility.
            // Agility's "max" (?) is 0.135f, so it's agiGrowth * some value

            row[31] = Math.Round(40 * hpGrowth).ToString();
            row[32] = Math.Round(40 * strGrowth).ToString();
            row[33] = Math.Round(40 * defGrowth).ToString();
            row[34] = Math.Round(40 * magGrowth).ToString();
            row[35] = Math.Round(40 * mdefGrowth).ToString();
            // agility stays the same

            return row;
        }
        public static void RandomizeMirageStats(string currDir, string sV, RichTextBox log)
        {
            log.AppendText("Randomizing mirage stats....\n");
            
            string clPath = Path.Combine(currDir, "character_list.csv");
            // mirageIDs start at 7000. Some need to be avoided to prevent affecting boss fights.
            List<string> exceptionList = ["7027", "7041", "7080", "7086", "7113", "7134", "7162",
                "7168", "7172", "7191", "7192", "7193", "7194", "7195", "8000", "8006", "8010", 
                "8017", "8018", "8025"];
            // 8028 is the last mirage to deal with

            // Use the pokemon BST method, I guess
            List<List<string>> clData = CsvHandling.CsvReadData(clPath);

            // Set random based on the seed value
            Random fixRand = new Random(Shuffle.ConsistentStringHash(sV));

            for (int i = 0; i < clData.Count; i++)
            {
                string clID = clData[i][0];
                // Need to eventually make a different setup for XL mirages
                if (!exceptionList.Contains(clID) && Int32.Parse(clID) >= 7000 && Int32.Parse(clID) <= 8028)
                {
                    clData[i] = SetStats(clData[i], fixRand);
                }
            }
            CsvHandling.CsvWriteData(clPath, clData);

        }
    }
}
