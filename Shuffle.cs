using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOFFRandomizer
{
    internal class Shuffle
    {
        public static int ConsistentStringHash(string value)
        {
            var bytes = System.Text.Encoding.Default.GetBytes(value);
            int stableHash = bytes.Aggregate<byte, int>(23, (acc, val) => acc * 17 + val);
            return stableHash;
        }
    }
}
