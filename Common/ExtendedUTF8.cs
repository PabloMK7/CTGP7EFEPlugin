using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGP7.Common
{
    class ExtendedUTF8
    {
        static public int DecodeCodepoint(out uint output, byte[] input, int offset)
        {
            byte code1, code2, code3, code4;

            code1 = input[offset];
            
            if (code1 < 0x80)
            {
                /* 1-byte sequence */
                output = code1;
                return 1;
            }
            else if (code1 < 0xC2)
            {
                throw new ArgumentException("Invalid UTF8 sequence.");
            }
            else if (code1 < 0xE0)
            {
                /* 2-byte sequence */
                code2 = input[offset + 1];

                if ((code2 & 0xC0) != 0x80)
                {
                    throw new ArgumentException("Invalid UTF8 sequence.");
                }

                output = (uint)((code1 << 6) + code2 - 0x3080);
                return 2;
            }
            else if (code1 < 0xF0)
            {
                /* 3-byte sequence */
                code2 = input[offset + 1];

                if ((code2 & 0xC0) != 0x80)
                {
                    throw new ArgumentException("Invalid UTF8 sequence.");
                }
                if (code1 == 0xE0 && code2 < 0xA0)
                {
                    throw new ArgumentException("Invalid UTF8 sequence.");
                }

                code3 = input[offset + 2];

                if ((code3 & 0xC0) != 0x80)
                {
                    throw new ArgumentException("Invalid UTF8 sequence.");
                }

                output = (uint)((code1 << 12) + (code2 << 6) + code3 - 0xE2080);
                return 3;
            }
            else if (code1 < 0xF5)
            {
                /* 4-byte sequence */
                code2 = input[offset + 1];

                if ((code2 & 0xC0) != 0x80)
                {
                    throw new ArgumentException("Invalid UTF8 sequence.");
                }
                if (code1 == 0xF0 && code2 < 0x90)
                {
                    throw new ArgumentException("Invalid UTF8 sequence.");
                }
                if (code1 == 0xF4 && code2 >= 0x90)
                {
                    throw new ArgumentException("Invalid UTF8 sequence.");
                }

                code3 = input[offset + 2];
                if ((code3 & 0xC0) != 0x80)
                {
                    throw new ArgumentException("Invalid UTF8 sequence.");
                }

                code4 = input[offset + 3];
                if ((code4 & 0xC0) != 0x80)
                {
                    throw new ArgumentException("Invalid UTF8 sequence.");
                }

                output = (uint)((code1 << 18) + (code2 << 12) + (code3 << 6) + code4 - 0x3C82080);
                return 4;
            }
            throw new ArgumentException("Invalid UTF8 sequence.");
        }
    }
}
