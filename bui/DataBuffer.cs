using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bui
{
    public class DataBuffer
    {
        private byte[] buffer;

        public int Index { get; private set; }
        public DataBuffer()
        {
                
        }

        public DataBuffer(byte[] buff)
        {
            buffer = buff;
            Index = 0;
        }

        public float GetFloat()
        {
            if (buffer.Length < Index + 4) throw new Exception($"buffer to small: length-{buffer.Length}; Index-{Index}");
            float res = buffer.GetFloat(Index);
            Index += 4;
            return res;
        }

        public UInt16 GetUInt16()
        {
            if (buffer.Length < Index + 2) throw new Exception($"buffer to small: length-{buffer.Length}; Index-{Index}");
            UInt16 res = buffer.GetUInt16(Index);
            Index += 2;
            return res;
        }

        internal byte GetByte()
        {
            if (buffer.Length < Index + 2) throw new Exception($"buffer to small: length-{buffer.Length}; Index-{Index}");
            byte res = buffer[Index++];
            return res;
        }
    }
}
