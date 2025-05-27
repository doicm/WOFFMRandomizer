using System.Globalization;
using System.Text;

namespace CshToolHelpers
{
    internal class ConversionHelpers
    {
        #region -csv
        public static void ConvertToCsv(string cshFile)
        {
            var cshVars = new CshVariables();

            byte[] dcmpCshData;

            SupportMethods.LogMessage("Decompressing csh file....");
            SupportMethods.LogMessage("");

            using (var cmpCshReader = new BinaryReader(File.Open(cshFile, FileMode.Open, FileAccess.Read)))
            {
                // Read cmp header values and
                // do basic checks
                if (cmpCshReader.BaseStream.Length < 144)
                {
                    SupportMethods.ErrorExit("csh file is too small in size!");
                }

                _ = cmpCshReader.BaseStream.Position = 0;

                if (cmpCshReader.ReadBytesString(4, false) != cshVars.CmpMagic)
                {
                    SupportMethods.ErrorExit("Not a valid csh file!");
                }

                _ = cmpCshReader.BaseStream.Position += 124;

                if (cmpCshReader.ReadBytesString(4, false) != cshVars.CmpZLIBMagic)
                {
                    SupportMethods.ErrorExit("Not a valid csh file!");
                }

                cshVars.DcmpSize = cmpCshReader.ReadBytesUInt32(true);
                cshVars.CmpSize = cmpCshReader.ReadBytesUInt32(true);
                cshVars.CmpUnkVal5 = cmpCshReader.ReadBytesUInt32(true);

                var cmpData = new byte[cshVars.CmpSize];
                cmpData = cmpCshReader.ReadBytes((int)cshVars.CmpSize);

                using (var cmpStream = new MemoryStream())
                {
                    cmpStream.Write(cmpData, 0, (int)cshVars.CmpSize);
                    cmpStream.Seek(0, SeekOrigin.Begin);

                    dcmpCshData = SupportMethods.ZlibDecompressBuffer(cmpStream);
                }
            }

            using (var dcmpStream = new MemoryStream())
            {
                dcmpStream.Write(dcmpCshData, 0, (int)cshVars.DcmpSize);
                dcmpStream.Seek(0, SeekOrigin.Begin);

                SupportMethods.LogMessage("Reading csh entries....");
                SupportMethods.LogMessage("");

                using (var dcmpCshReader = new BinaryReader(dcmpStream))
                {
                    // Read dcmp header values
                    // and do basic checks
                    if (dcmpCshReader.BaseStream.Length < 12)
                    {
                        SupportMethods.ErrorExit("csh file is too small in size!");
                    }

                    _ = dcmpCshReader.BaseStream.Position = 0;

                    if (dcmpCshReader.ReadBytesUInt32(true) != cshVars.DcmpMagic)
                    {
                        SupportMethods.ErrorExit("Not a valid csh file!");
                    }

                    cshVars.FieldCount = dcmpCshReader.ReadBytesUInt32(true);
                    cshVars.RowsCount = dcmpCshReader.ReadBytesUInt32(true);

                    SupportMethods.LogMessage($"Field Count: {cshVars.FieldCount}");
                    SupportMethods.LogMessage($"Rows Count: {cshVars.RowsCount}");
                    SupportMethods.LogMessage("");

                    var entryTablePos = dcmpCshReader.BaseStream.Position;

                    // Read each entry and write to a csv file
                    var outCsvFile = Path.Combine(Path.GetDirectoryName(cshFile), Path.GetFileNameWithoutExtension(cshFile) + ".csv");

                    if (File.Exists(outCsvFile))
                    {
                        File.Delete(outCsvFile);
                    }

                    using (var csvWriter = new StreamWriter(outCsvFile, true, Encoding.UTF8))
                    {
                        var currentPos = entryTablePos;

                        for (int i = 0; i < cshVars.RowsCount; i++)
                        {
                            for (int j = 0; j < cshVars.FieldCount; j++)
                            {
                                _ = dcmpCshReader.BaseStream.Position = currentPos;

                                cshVars.EntryDataOffset = dcmpCshReader.ReadBytesUInt32(true);
                                cshVars.EntryDataType = dcmpCshReader.ReadByte();
                                _ = dcmpCshReader.ReadByte();
                                cshVars.EntryDataSizeMultiplier = dcmpCshReader.ReadBytesUInt16(true);

                                _ = dcmpCshReader.BaseStream.Position = cshVars.EntryDataOffset;
                                cshVars.EntryData.AddRange(dcmpCshReader.ReadBytes(cshVars.EntryDataSizeMultiplier * 4));

                                switch (cshVars.EntryDataType)
                                {
                                    // string
                                    case 0:
                                        var readStringValue = SupportMethods.FixStringFromCsh(Encoding.UTF8.GetString(cshVars.EntryData.ToArray()));
                                        cshVars.EntryOnCSV += readStringValue;
                                        break;

                                    // int
                                    case 64:
                                        if (cshVars.EntryDataSizeMultiplier * 4 == 4)
                                        {
                                            var readValueArray = cshVars.EntryData.ToArray();
                                            Array.Reverse(readValueArray);

                                            cshVars.EntryIntValue = BitConverter.ToInt32(readValueArray, 0);
                                            cshVars.EntryOnCSV += cshVars.EntryIntValue.ToString();
                                        }
                                        break;

                                    // float
                                    case 128:
                                        if (cshVars.EntryDataSizeMultiplier * 4 == 4)
                                        {
                                            var readValueArray = cshVars.EntryData.ToArray();
                                            Array.Reverse(readValueArray);

                                            cshVars.EntryFloatValue = BitConverter.ToSingle(readValueArray, 0);
                                            cshVars.EntryOnCSV += cshVars.EntryFloatValue.ToString() + "f";
                                        }
                                        break;

                                    // empty
                                    case 192:
                                        break;

                                    // unknown
                                    default:
                                        cshVars.EntryOnCSV += $"0x{cshVars.EntryDataType.ToString("X2")}";
                                        break;
                                }

                                if (j != cshVars.FieldCount - 1)
                                {
                                    cshVars.EntryOnCSV += cshVars.CSVDelimiter;
                                }

                                cshVars.EntryData.Clear();

                                currentPos += 8;
                            }

                            SupportMethods.LogMessage($"Entry_{i}: {cshVars.EntryOnCSV}");
                            csvWriter.WriteLine(cshVars.EntryOnCSV);
                            cshVars.EntryOnCSV = string.Empty;
                        }
                    }
                }
            }

            SupportMethods.LogMessage("");
            SupportMethods.LogMessage("");
            SupportMethods.LogMessage("Finished writing csh data to csv file");
        }
        #endregion


        #region -csh
        public static void ConvertToCsh(string csvFile)
        {
            var cshVars = new CshVariables();

            var csvData = File.ReadAllLines(csvFile);

            if (csvData.Length == 0)
            {
                SupportMethods.ErrorExit("Specified csv file is empty!");
            }

            cshVars.FieldCount = (uint)csvData[0].Split(cshVars.CSVDelimiter).Length;
            cshVars.RowsCount = (uint)csvData.Length;

            SupportMethods.LogMessage($"Field Count: {cshVars.FieldCount}");
            SupportMethods.LogMessage($"Entries Table Count: {cshVars.RowsCount}");
            SupportMethods.LogMessage("");
            SupportMethods.LogMessage("");

            SupportMethods.LogMessage("Generating csh header....");
            SupportMethods.LogMessage("");

            byte[] headerData;
            long headerSize;

            using (var preCmpHeaderStream = new MemoryStream())
            {
                using (var preCmpHeaderWriter = new BinaryWriter(preCmpHeaderStream))
                {
                    _ = preCmpHeaderWriter.BaseStream.Position = 0;
                    preCmpHeaderWriter.WriteBytesUInt32(cshVars.DcmpMagic, true);
                    preCmpHeaderWriter.WriteBytesUInt32(cshVars.FieldCount, true);
                    preCmpHeaderWriter.WriteBytesUInt32(cshVars.RowsCount, true);
                    headerSize = preCmpHeaderWriter.BaseStream.Length;

                    preCmpHeaderStream.Seek(0, SeekOrigin.Begin);
                    headerData = preCmpHeaderStream.ToArray();
                }
            }

            SupportMethods.LogMessage("Generating csh entry table....");
            SupportMethods.LogMessage("");

            byte[] entryTableData;
            var entryDataDict = new Dictionary<int, List<byte>>();

            using (var preCmpEntryTableStream = new MemoryStream())
            {
                using (var preCmpEntryTableWriter = new BinaryWriter(preCmpEntryTableStream))
                {
                    _ = preCmpEntryTableWriter.BaseStream.Position = 0;

                    long offset = headerSize + (cshVars.RowsCount * (cshVars.FieldCount * 8));
                    cshVars.EntryDataOffset = 0;

                    string[] currentEntryData;
                    int dictKey = 0;

                    foreach (var entryData in csvData)
                    {
                        currentEntryData = entryData.Split(cshVars.CSVDelimiter);

                        cshVars.EntryData = new List<byte>();

                        foreach (var entryField in currentEntryData)
                        {
                            byte[] currentEntryDataBytes = Array.Empty<byte>();

                            if (string.IsNullOrEmpty(entryField))
                            {
                                // empty
                                cshVars.EntryDataOffset = 0;
                                cshVars.EntryDataType = 192;
                                cshVars.EntryDataSizeMultiplier = 0;
                            }
                            else if (entryField.EndsWith("f") && float.TryParse(entryField.Replace("f", ""), NumberStyles.Float, CultureInfo.InvariantCulture, out cshVars.EntryFloatValue) == true)
                            {
                                // float
                                cshVars.EntryDataOffset = (uint)offset;
                                cshVars.EntryDataType = 128;
                                cshVars.EntryDataSizeMultiplier = 1;
                                currentEntryDataBytes = BitConverter.GetBytes(cshVars.EntryFloatValue);
                                Array.Reverse(currentEntryDataBytes);
                                offset += 4;
                            }
                            else if (int.TryParse(entryField, NumberStyles.Integer, CultureInfo.InvariantCulture, out cshVars.EntryIntValue) == true)
                            {
                                // int
                                cshVars.EntryDataOffset = (uint)offset;
                                cshVars.EntryDataType = 64;
                                cshVars.EntryDataSizeMultiplier = 1;
                                currentEntryDataBytes = BitConverter.GetBytes(cshVars.EntryIntValue);
                                Array.Reverse(currentEntryDataBytes);
                                offset += 4;
                            }
                            else
                            {
                                if (entryField.StartsWith("0x"))
                                {
                                    // unknown
                                    cshVars.EntryDataOffset = 0;
                                    cshVars.EntryDataType = Convert.ToByte(entryField.Substring(2), 16);
                                    cshVars.EntryDataSizeMultiplier = 0;
                                }
                                else
                                {
                                    // string
                                    cshVars.EntryDataOffset = (uint)offset;
                                    cshVars.EntryDataType = 0;

                                    var readStringVal = entryField + "\0";
                                    cshVars.EntryStringVal = SupportMethods.FixStringFromCsv(readStringVal);
                                    var currentEntryStringList = Encoding.UTF8.GetBytes(cshVars.EntryStringVal).ToList();
                                    var currentEntryStringValSize = currentEntryStringList.Count;

                                    if (currentEntryStringValSize % 4 != 0)
                                    {
                                        var remainder = currentEntryStringValSize % 4;
                                        var increaseAmount = 4 - remainder;
                                        currentEntryStringValSize += increaseAmount;

                                        for (int p = 0; p < increaseAmount; p++)
                                        {
                                            currentEntryStringList.Add(0x00);
                                        }

                                        cshVars.EntryDataSizeMultiplier = (ushort)(currentEntryStringValSize / 4);
                                    }
                                    else
                                    {
                                        cshVars.EntryDataSizeMultiplier = (ushort)(currentEntryStringValSize / 4);
                                    }

                                    offset += currentEntryStringValSize;
                                    currentEntryDataBytes = currentEntryStringList.ToArray();
                                }
                            }

                            preCmpEntryTableWriter.WriteBytesUInt32(cshVars.EntryDataOffset, true);
                            preCmpEntryTableWriter.Write(cshVars.EntryDataType);
                            preCmpEntryTableWriter.Write(cshVars.EntryReserved);
                            preCmpEntryTableWriter.WriteBytesUInt16(cshVars.EntryDataSizeMultiplier, true);

                            cshVars.EntryData.AddRange(currentEntryDataBytes);
                        }

                        entryDataDict.Add(dictKey, cshVars.EntryData);
                        dictKey++;
                    }

                    preCmpEntryTableStream.Seek(0, SeekOrigin.Begin);
                    entryTableData = preCmpEntryTableStream.ToArray();
                }
            }

            SupportMethods.LogMessage("Packing csv data....");
            SupportMethods.LogMessage("");

            byte[] csvEntriesData;

            using (var preCmpCsvEntriesDataStream = new MemoryStream())
            {
                using (var csvEntriesDataWriter = new BinaryWriter(preCmpCsvEntriesDataStream))
                {
                    _ = csvEntriesDataWriter.BaseStream.Position = 0;

                    foreach (var csvEntryData in entryDataDict.Values)
                    {
                        csvEntriesDataWriter.Write(csvEntryData.ToArray());
                    }

                    preCmpCsvEntriesDataStream.Seek(0, SeekOrigin.Begin);
                    csvEntriesData = preCmpCsvEntriesDataStream.ToArray();
                }
            }


            SupportMethods.LogMessage("Arranging and compressing csh data....");
            SupportMethods.LogMessage("");

            byte[] dataToCmp;

            using (var dataToCmpStream = new MemoryStream())
            {
                dataToCmpStream.Write(headerData);
                dataToCmpStream.Write(entryTableData);
                dataToCmpStream.Write(csvEntriesData);

                dataToCmpStream.Seek(0, SeekOrigin.Begin);
                dataToCmp = dataToCmpStream.ToArray();
                cshVars.DcmpSize = (uint)dataToCmpStream.Length;
            }

            var cmpData = SupportMethods.ZlibCompressBuffer(dataToCmp);
            cshVars.CmpSize = (uint)cmpData.Length;

            var outCshFile = Path.Combine(Path.GetDirectoryName(csvFile), Path.GetFileNameWithoutExtension(csvFile) + ".csh");

            if (File.Exists(outCshFile))
            {
                File.Delete(outCshFile);
            }

            SupportMethods.LogMessage("Building csh file....");
            SupportMethods.LogMessage("");

            using (var cshStreamWriter = new BinaryWriter(File.Open(outCshFile, FileMode.Append, FileAccess.Write)))
            {
                cshStreamWriter.Write(Encoding.UTF8.GetBytes("\0" + cshVars.CmpMagic));
                cshStreamWriter.WriteBytesUInt32(cshVars.CmpUnkVal, true);
                cshStreamWriter.WriteBytesUInt32(cshVars.CmpUnkVal2, true);
                cshStreamWriter.WriteBytesUInt32(cshVars.CmpUnkVal3, true);
                PadReservedBytes(cshStreamWriter, 6);

                cshStreamWriter.WriteBytesUInt32(cshVars.CmpUnkVal4, true);
                PadReservedBytes(cshStreamWriter, 21);

                cshStreamWriter.Write(Encoding.UTF8.GetBytes(cshVars.CmpZLIBMagic));
                cshStreamWriter.WriteBytesUInt32(cshVars.DcmpSize, true);
                cshStreamWriter.WriteBytesUInt32(cshVars.CmpSize, true);
                cshStreamWriter.WriteBytesUInt32(cshVars.CmpUnkVal5, true);

                cshStreamWriter.Write(cmpData);
            }

            SupportMethods.LogMessage("");
            SupportMethods.LogMessage("");
            SupportMethods.LogMessage("Finished writing csv data to csh file");
        }


        private static void PadReservedBytes(BinaryWriter cshStreamWriter, int padAmount)
        {
            for (int l = 0; l < padAmount; l++)
            {
                cshStreamWriter.WriteBytesUInt32(0, false);
            }
        }
        #endregion
    }
}