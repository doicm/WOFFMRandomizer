using System.Text;

internal static class BinaryReaderHelpers
{
    public static short ReadBytesInt16(this BinaryReader reader, bool isBigEndian)
    {
        var readValueBuffer = reader.ReadBytes(2);
        ReverseIfBigEndian(isBigEndian, readValueBuffer);

        return BitConverter.ToInt16(readValueBuffer, 0);
    }


    public static ushort ReadBytesUInt16(this BinaryReader reader, bool isBigEndian)
    {
        var readValueBuffer = reader.ReadBytes(2);
        ReverseIfBigEndian(isBigEndian, readValueBuffer);

        return BitConverter.ToUInt16(readValueBuffer, 0);
    }


    public static int ReadBytesInt32(this BinaryReader reader, bool isBigEndian)
    {
        var readValueBuffer = reader.ReadBytes(4);
        ReverseIfBigEndian(isBigEndian, readValueBuffer);

        return BitConverter.ToInt32(readValueBuffer, 0);
    }


    public static uint ReadBytesUInt32(this BinaryReader reader, bool isBigEndian)
    {
        var readValueBuffer = reader.ReadBytes(4);
        ReverseIfBigEndian(isBigEndian, readValueBuffer);

        return BitConverter.ToUInt32(readValueBuffer, 0);
    }


    public static long ReadBytesInt64(this BinaryReader reader, bool isBigEndian)
    {
        var readValueBuffer = reader.ReadBytes(8);
        ReverseIfBigEndian(isBigEndian, readValueBuffer);

        return BitConverter.ToInt64(readValueBuffer, 0);
    }


    public static ulong ReadBytesUInt64(this BinaryReader reader, bool isBigEndian)
    {
        var readValueBuffer = reader.ReadBytes(8);
        ReverseIfBigEndian(isBigEndian, readValueBuffer);

        return BitConverter.ToUInt64(readValueBuffer, 0);
    }


    public static float ReadBytesFloat(this BinaryReader reader, bool isBigEndian)
    {
        var readValueBuffer = reader.ReadBytes(4);
        ReverseIfBigEndian(isBigEndian, readValueBuffer);

        return BitConverter.ToSingle(readValueBuffer, 0);
    }


    public static double ReadBytesDouble(this BinaryReader reader, bool isBigEndian)
    {
        var readValueBuffer = reader.ReadBytes(8);
        ReverseIfBigEndian(isBigEndian, readValueBuffer);

        return BitConverter.ToDouble(readValueBuffer, 0);
    }


    public static string ReadBytesString(this BinaryReader reader, int readCount, bool shouldReverse)
    {
        var readValueBuffer = reader.ReadBytes(readCount);
        ReverseIfBigEndian(shouldReverse, readValueBuffer);

        return Encoding.UTF8.GetString(readValueBuffer).Replace("\0", "");
    }


    public static string ReadStringTillNull(this BinaryReader reader)
    {
        var sb = new StringBuilder();
        char chars;
        while ((chars = reader.ReadChar()) != default)
        {
            sb.Append(chars);
        }
        return sb.ToString();
    }


    public static List<byte> ReadBytesTillNull(this BinaryReader reader)
    {
        var byteList = new List<byte>();
        byte currentValue;
        while ((currentValue = reader.ReadByte()) != default)
        {
            byteList.Add(currentValue);
        }

        return byteList;
    }


    static void ReverseIfBigEndian(bool isBigEndian, byte[] readValueBuffer)
    {
        if (isBigEndian)
        {
            Array.Reverse(readValueBuffer);
        }
    }
}