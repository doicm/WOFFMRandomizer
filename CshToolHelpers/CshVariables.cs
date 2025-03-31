namespace CshToolHelpers
{
    internal class CshVariables
    {
        public string CmpMagic = "hsc";
        public uint CmpUnkVal = 2;
        public uint CmpUnkVal2 = 13;
        public uint CmpUnkVal3 = 2;
        public uint CmpUnkVal4 = 1;
        public string CmpZLIBMagic = "BILZ";
        public uint DcmpSize;
        public uint CmpSize;
        public uint CmpUnkVal5 = 7;

        public uint DcmpMagic = 0;        
        public uint FieldCount;
        public uint RowsCount;

        public uint FirstTableReserved = 0;
        public byte FirstTableUnkVal = 0xA0;
        public byte[] FirstTableReservedArray = new byte[3] { 0x00, 0x00, 0x00 };

        public uint EntryDataOffset;
        public byte EntryDataType;
        public byte EntryReserved = 0x00;
        public ushort EntryDataSizeMultiplier;

        public List<byte> EntryData = new();
        public string? EntryStringVal;
        public int EntryIntValue;
        public float EntryFloatValue;
        public string? EntryOnCSV;
    }
}