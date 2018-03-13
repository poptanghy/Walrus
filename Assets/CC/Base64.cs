using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QL.Core
{
    internal static class Base64
    {
        private static string encode_tbl_ = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        private static byte[] decode_tbl_ =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 0,	0, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,62, 0, 0, 0,63,
            52,53,54,55,56,57,58,59,60,61, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,
            15,16,17,18,19,20,21,22,23,24,25, 0, 0, 0, 0, 0,
            0,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,
            41,42,43,44,45,46,47,48,49,50,51
        };

        public static string Encode(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            byte[] cTmp = {0, 0, 0 };
            int len = data.Length;
            int iDiv3Num = len / 3;
            int iMod3Num = len % 3;

            int line_len = 0;
            int kkk = 0;
            for (int i = 0; i < iDiv3Num; i++)
            {
                cTmp[0] = data[kkk++];
                cTmp[1] = data[kkk++];
                cTmp[2] = data[kkk++];

                sb.Append(encode_tbl_[cTmp[0] >> 2]);
                sb.Append(encode_tbl_[((cTmp[0] << 4) | (cTmp[1] >> 4)) & 0x3F]);
                sb.Append(encode_tbl_[((cTmp[1] << 2) | (cTmp[2] >> 6)) & 0x3F]);
                sb.Append(encode_tbl_[cTmp[2] & 0x3F]);

                line_len += 4;
                if (line_len == 76)
                {
                    sb.Append("\r\n");
                    line_len = 0;
                }
            }

            //对剩余数据进行编码
            if (iMod3Num == 1)
            {
                cTmp[0] = data[kkk++];
                sb.Append(encode_tbl_[(cTmp[0] & 0xFC) >> 2]);
                sb.Append(encode_tbl_[((cTmp[0] & 0x03) << 4)]);
                sb.Append("==");
            }
            else if (iMod3Num == 2)
            {
                cTmp[0] = data[kkk++];
                cTmp[1] = data[kkk++];
                sb.Append(encode_tbl_[(cTmp[0] & 0xFC) >> 2]);
                sb.Append(encode_tbl_[((cTmp[0] & 0x03) << 4) | ((cTmp[1] & 0xF0) >> 4)]);
                sb.Append(encode_tbl_[((cTmp[1] & 0x0F) << 2)]);
                sb.Append("=");
            }

            return sb.ToString();
        }

        public static byte[] Decode(string d_in)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(d_in);
            int len = data.Length;

            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            int nValue;
            int i = 0;

            int kkk = 0;
            while (i < len)
            {
                if (data[kkk] == '\r' || data[kkk] == '\n')
                {
                    kkk++;
                    i++;
                    continue;
                }

                nValue = (int)(decode_tbl_[data[kkk++]] << 18);
                nValue += (int)(decode_tbl_[data[kkk++]] << 12);

                int c3 = (nValue & 0x00FF0000) >> 16;
                //strDecoded+=(nValue & 0x00FF0000) >> 16;
                bw.Write((byte)c3);

                if (data[kkk] != '=')
                {
                    nValue += (int)(decode_tbl_[data[kkk++]] << 6);

                    int c4 = (nValue & 0x0000FF00) >> 8;
                    bw.Write((byte)c4);

                    if (data[kkk] != '=')
                    {
                        nValue += (int)(decode_tbl_[data[kkk++]]);

                        int c5 = nValue & 0x000000FF;
                        bw.Write((byte)c5);
                    }
                }
                i += 4;
            }

            return ms.ToArray();
        }
    }
}
