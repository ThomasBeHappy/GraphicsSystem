using System;
using System.Collections.Generic;
using System.Text;

namespace GifParser
{

    static class Compression
    {
        public static int CompressedSize
        {
            get;
            set;
        }
        public static int DeCompressedSize
        {
            get;
            set;
        }

        // TODO change this to use chars instead of strings
        //public byte[] decodeLZW(int[] op)
        //{
        //    Dictionary<int, byte[]> table = new Dictionary<int, byte[]>();
        //    for (int i = 0; i <= 255; i++)
        //    {
        //        //string ch = "";
        //        //ch += (char)(i);
        //        table[i] = new byte[i];
        //    }
        //    int old = op[0], n;
        //    string s = table[old];
        //    string c = "";
        //    c += s[0];
        //    int count = 256;
        //    for (int i = 0; i < op.Length - 1; i++)
        //    {
        //        n = op[i + 1];
        //        if (!table.ContainsKey(n))
        //        {
        //            s = table[old];
        //            s = s + c;
        //        }
        //        else
        //        {
        //            s = table[n];
        //        }
        //        c = "";
        //        c += s[0];
        //        table[count] = table[old] + c;
        //        count++;
        //        old = n;
        //    }
        //    return c;
        //}


        public static byte[] LzwDecompress(this byte[] Bufi, int lenght)
        {
            if (Bufi == null) throw new Exception("Input buffer is null.");
            if (Bufi.Length == 0) throw new Exception("Input buffer is empty.");
            var iBufi = Ia(Bufi);
            var iBuf = new List<int>(iBufi);
            CompressedSize = iBuf.Count;
            var dictionary = new Dictionary<int, List<byte>>();
            for (var i = 0; i < 256; i++)
            {
                var e = new List<byte> { (byte)i };
                dictionary.Add(i, e);
            }
            var window = dictionary[iBuf[0]];
            iBuf.RemoveAt(0);
            var oBuf = new List<byte>(window);
            for (int i = 0; i < lenght; i++)
            {
                int k = iBuf[i];
                var entry = new List<byte>();
                if (dictionary.ContainsKey(k))
                    entry.AddRange(dictionary[k]);
                else if (k == dictionary.Count)
                    entry.AddRange(Add(window.ToArray(), new[] { window.ToArray()[0] }));
                if (entry.Count > 0)
                {
                    oBuf.AddRange(entry);
                    DeCompressedSize = oBuf.Count;
                    dictionary.Add(dictionary.Count, new List<byte>(Add(window.ToArray(), new[] { entry.ToArray()[0] })));
                    window = entry;
                }
            }
            return oBuf.ToArray();
        }

        private static byte[] Add(byte[] left, byte[] right)
        {
            var l1 = left.Length;
            var l2 = right.Length;
            var ret = new byte[l1 + l2];
            Buffer.BlockCopy(left, 0, ret, 0, l1);
            Buffer.BlockCopy(right, 0, ret, l1, l2);
            return ret;
        }
        private static int[] Ia(byte[] ba)
        {
            var bal = ba.Length;
            var int32Count = bal / 4 + (bal % 4 == 0 ? 0 : 1);
            var arr = new int[int32Count];
            Buffer.BlockCopy(ba, 0, arr, 0, bal);
            return arr;
        }
    }
}
