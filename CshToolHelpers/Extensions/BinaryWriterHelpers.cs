internal static class BinaryWriterHelpers
{
    public static void WriteBytesInt16(this BinaryWriter writerName, short valueToWrite, bool isBigEndian)
    {
        var writeValueBuffer = BitConverter.GetBytes(valueToWrite);
        ReverseIfBigEndian(isBigEndian, writeValueBuffer);

        writerName.Write(writeValueBuffer);
    }


    public static void WriteBytesUInt16(this BinaryWriter writerName, ushort valueToWrite, bool isBigEndian)
    {
        var writeValueBuffer = BitConverter.GetBytes(valueToWrite);
        ReverseIfBigEndian(isBigEndian, writeValueBuffer);

        writerName.Write(writeValueBuffer);
    }


    public static void WriteBytesInt32(this BinaryWriter writerName, int valueToWrite, bool isBigEndian)
    {
        var writeValueBuffer = BitConverter.GetBytes(valueToWrite);
        ReverseIfBigEndian(isBigEndian, writeValueBuffer);

        writerName.Write(writeValueBuffer);
    }


    public static void WriteBytesUInt32(this BinaryWriter writerName, uint valueToWrite, bool isBigEndian)
    {
        var writeValueBuffer = BitConverter.GetBytes(valueToWrite);
        ReverseIfBigEndian(isBigEndian, writeValueBuffer);

        writerName.Write(writeValueBuffer);
    }


    public static void WriteBytesInt64(this BinaryWriter writerName, long valueToWrite, bool isBigEndian)
    {
        var writeValueBuffer = BitConverter.GetBytes(valueToWrite);
        ReverseIfBigEndian(isBigEndian, writeValueBuffer);

        writerName.Write(writeValueBuffer);
    }


    public static void WriteBytesUInt64(this BinaryWriter writerName, ulong valueToWrite, bool isBigEndian)
    {
        var writeValueBuffer = BitConverter.GetBytes(valueToWrite);
        ReverseIfBigEndian(isBigEndian, writeValueBuffer);

        writerName.Write(writeValueBuffer);
    }


    public static void WriteBytesFloat(this BinaryWriter writerName, float valueToWrite, bool isBigEndian)
    {
        var writeValueBuffer = BitConverter.GetBytes(valueToWrite);
        ReverseIfBigEndian(isBigEndian, writeValueBuffer);

        writerName.Write(writeValueBuffer);
    }


    public static void WriteBytesDouble(this BinaryWriter writerName, double valueToWrite, bool isBigEndian)
    {
        var writeValueBuffer = BitConverter.GetBytes(valueToWrite);
        ReverseIfBigEndian(isBigEndian, writeValueBuffer);

        writerName.Write(writeValueBuffer);
    }


    static void ReverseIfBigEndian(bool isBigEndian, byte[] writeValueBuffer)
    {
        if (isBigEndian)
        {
            Array.Reverse(writeValueBuffer);
        }
    }
}