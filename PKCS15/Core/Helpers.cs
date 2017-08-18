using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace smartcardLib.Core
{
    /// <summary>
    /// Helpers static class which include all helpers 
    /// </summary>
    /// <remarks>make different helpers for different objects</remarks>
    public static class Helpers
    {
        /// <summary>
        /// Removes trail zeros from a byte array
        /// </summary>
        /// <param name="bt">the byte array</param>
        /// <returns>a new byte array with zero trails removed</returns>
        /// <remarks>Array doesn't change the output of this array is to be used</remarks>
        public static byte[] RemoveTrailZeros(this byte[] bt)
        {
            var i = bt.Length;
            while (i >= 1 && bt[i - 1] == 0)
            {
                --i;
            }
            if (i == 0)
                return null;
            var temp = new byte[i];
            Buffer.BlockCopy(bt, 0, temp, 0, i);
            return temp;
        }
        /// <summary>
        /// Coverts an OID string to equivalent byte array (trims spaces)
        /// </summary>
        /// <param name="oid">string as OID</param>
        /// <returns>OID as byte array</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        /// <remarks>It might be a good idea to add the RegEx </remarks>
        public static byte[] Oid(this string oid)
        {
            if (!Regex.Match(oid.Trim(), "[[0-9]+\\.]*").Success)
                throw new ArgumentException("String is not an OID");
            string[] split = oid.Trim(' ', '.').Split('.');
            List<int> retVal = new List<int>();

            for (int a = 0, b = 0, i = 0; i < split.Length; i++)
            {
                if (i == 0)
                    a = int.Parse(split[0]);
                else if (i == 1)
                    retVal.Add(40 * a + int.Parse(split[1]));
                else
                {
                    b = int.Parse(split[i]);

                    if (b < 128)
                        retVal.Add(b);
                    else
                    {
                        retVal.Add(128 + (b / 128));
                        retVal.Add(b % 128);
                    }
                }
            }

            byte[] temp = new byte[retVal.Count];

            for (int i = 0; i < retVal.Count; i++)
                temp[i] = (byte)retVal[i];

            return temp;

        }
        /// <summary>
        /// Convert an ASN INTEGER to unsigned int
        /// </summary>
        /// <param name="iBytes">VALUE of ASN INTEGER</param>
        /// <returns>unsigned value</returns>
        /// <exception cref="ArgumentException"></exception>
        public static uint ToUnsigned(this byte[] iBytes)
        {
            uint value = 0;
            var count = 0;
            var StillLeftHandSide = true;
            for (var i = 0; i < iBytes.Length; i++)
            {
                if (StillLeftHandSide == true && iBytes[i] == 0)
                    continue;
                StillLeftHandSide = false;
                var q = iBytes[i];
                value = value * 256 + q;
                count++;
                if (count > 4)
                    throw new ArgumentException("toUnsigned: Can't convert ByteString is too long");
            }

            return value;
        }
        /// <summary>
        /// Convert an ASN INTEGER to signed int
        /// </summary>
        /// <param name="iBytes">VALUE of ASN INTEGER as byte array</param>
        /// <returns>signed value</returns>
        /// <exception cref="ArgumentException"></exception>
        public static int ToSigned(this byte[] iBytes)
        {
            var FirstByte = iBytes[0];
            var value = 0;
            var count = 0;
            var StillLeftHandSide = true;
            if ((FirstByte & 128) == 128)
            {
                for (var i = 0; i < iBytes.Length; i++)
                {
                    if (StillLeftHandSide == true && iBytes[i] == 255)
                        continue;
                    StillLeftHandSide = false;
                    int q = iBytes[i];
                    q = ~q;
                    value = value * 256 + q;
                    count++;
                    if (count > 4)
                        throw new ArgumentException("toSigned: Can't convert ByteString is too long");
                }
                value = value + 1;
            }
            else
            {
                for (var i = 0; i < iBytes.Length; i++)
                {
                    if (iBytes[i] == 0 && StillLeftHandSide == true)
                        continue;
                    StillLeftHandSide = false;
                    var q = iBytes[i];
                    value = value * 256 + q;
                    count++;
                    if (count > 4)
                        throw new Exception("toSigned: Can't convert ByteString is too long");
                }


            }
            return value;
        }
        /// <summary>
        /// Converts a byte array to string of bits
        /// </summary>
        /// <param name="arr">byte array</param>
        /// <returns>string of bits</returns>
        public static string ToBits(this byte[] arr)
        {
            return string.Join("", arr.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));

        }
        /// <summary>
        /// Find the choice on a bit string enumerated value
        /// </summary>
        /// <param name="bitString">bit string</param>
        /// <returns>choice as integer</returns>
        public static int GetChoiceFromBitString(this byte[] bitString)
        {
            if (bitString.Length == 1)
            {
                if (bitString[0] != 0)
                    throw new ArgumentException("Wrong BIT STRING");
                return 0;
            }
            var str = bitString.Skip(1).ToArray().ToBits();
            if (bitString[0] != 0)
                str = str.Remove(str.Length - bitString[0]);
            int i = 0;
            foreach (var c in str)
            {
                if (c == '1')
                    return i;
                i++;
            }
            throw new ArgumentException("Wrong BIT STRING");
        }
        /// <summary>
        /// Converts BIT STRING to int
        /// </summary>
        /// <param name="bitString">BIT STRING as byte array</param>
        /// <returns>integer value of INT STRING</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int IntFromBitString(this byte[] bitString)
        {
            System.Diagnostics.Debug.Assert(bitString.Length < 5);
            var bits = (int)bitString.Skip(1).ToArray().ToSigned();
            return bits >> bitString[0];
        }
        /// <summary>
        /// converts a byte array to Oid String
        /// </summary>
        /// <param name="oid">byte array of OID</param>
        /// <returns>OID string</returns>
        public static string OidString(this byte[] oid)
        {
            StringBuilder retVal = new StringBuilder();

            for (int i = 0; i < oid.Length; i++)
            {
                if (i == 0)
                {
                    int b = oid[0] % 40;
                    int a = (oid[0] - b) / 40;
                    retVal.AppendFormat("{0}.{1}", a, b);
                }
                else
                {
                    if (oid[i] < 128)
                        retVal.AppendFormat(".{0}", oid[i]);
                    else
                    {
                        retVal.AppendFormat(".{0}",
                           ((oid[i] - 128) * 128) + oid[i + 1]);
                        i++;
                    }
                }
            }

            return retVal.ToString();
        }
        /// <summary>
        /// Appends byte arrays the array
        /// </summary>
        /// <param name="Dest">byte Array</param>
        /// <param name="Source">byte array</param>
        /// <returns></returns>
        /// <remarks>
        /// (WARNING) the array itself is not changes the returned value is the appended array
        /// </remarks>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static byte[] Append(this byte[] Dest, byte[] Source)
        {
            byte[] rv = new byte[Dest.Length + Source.Length];
            System.Buffer.BlockCopy(Dest, 0, rv, 0, Dest.Length);
            System.Buffer.BlockCopy(Source, 0, rv, Dest.Length, Source.Length);
            return rv;
        }
        /// <summary>
        /// Get the hex length of a string it is useful if the string is a hex array and you want to calculated the 
        /// array length 
        /// </summary>
        /// <param name="str">Hex string</param>
        /// <param name="RequestedLength">the size of the output as string i.e. if you have a 4 characters array and you state that the output should be 4 then you will get 0002</param>
        /// <returns>returns the hex array length as string</returns>
        public static string HexLength(this string str, int RequestedLength)
        {
            return (str.Length / 2).ToString($"X{RequestedLength}");
        }
        /// <summary>
        /// Converts a byte array to binary of base 2 
        /// </summary>
        /// <param name="arr">byte array</param>
        /// <returns>base 2 binary string</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string ToBinary(this byte[] arr)
        {
            return string.Concat(arr.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray());
        }
        /// <summary>
        /// Converts a byte array to hex string
        /// </summary>
        /// <param name="arr">byte array</param>
        /// <returns>hex string</returns>
        public static string ToHex(this byte[] arr)
        {
            return ToHex(arr, arr.Length);
        }
        /// <summary>
        /// Converts a specific amount from a byte array to hex if the Count is bigger than the length of the array the whole array will be converted and count is ignored
        /// </summary>
        /// <param name="ba">byte array</param>
        /// <param name="count">how many bytes from the array to convert</param>
        /// <returns>Hex string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string ToHex(this byte[] ba, int count)
        {
            count = (count > ba.Length) ? ba.Length : count;
            System.Text.StringBuilder hex = new System.Text.StringBuilder(count * 2);
            for (int j = 0; j < count; j++)
                hex.AppendFormat("{0:x2}", ba[j]);
            return hex.ToString();

        }
        /// <summary>
        /// Get the hex value of a character as integer
        /// </summary>
        /// <param name="hex">hex character</param>
        /// <returns>integer value</returns>
        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
        /// <summary>
        /// Converts the hex string to byte array
        /// </summary>
        /// <param name="hex">hex string</param>
        /// <returns>byte array of represented string</returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] ToBytes(this string hex)
        {
            if (hex.Length % 2 == 1)
                throw new ArgumentException("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static bool HasData(this string str)
        {
            int HexLength = str.Length / 2;
            if (HexLength < 4 || str.Length % 2 != 0)
            {
                throw new Exception("Not an APDU");
            }
            if (HexLength <= 5) return false;

            return true;

        }

        public static StringBuilder Pad(this StringBuilder sb, char ch, int count)
        {
            if (sb.Length / 2 < count)
                return sb.Append(ch, (count - sb.Length / 2) * 2);

            return sb;
        }
        public static StringBuilder Pad(this StringBuilder sb, int count)
        {
            if (sb.Length / 2 < count)
                return sb.Append('0', (count - sb.Length / 2) * 2);
            return sb;
        }

        public static byte ByteAt(this StringBuilder sb, int HexLocation)
        {
            return byte.Parse(sb.ToString(HexLocation * 2, 2), System.Globalization.NumberStyles.HexNumber);
        }


        /// <summary>
        /// Appends a hex string equivalent of byte array to string builder
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="Buffer"></param>
        /// <returns>Appended stringbuilder with hex values of bytes in buffer</returns>
        public static StringBuilder AppendBytes(this StringBuilder sb, byte[] Buffer)
        {
            return Append(sb, Buffer, (uint)Buffer.Length);
        }
        /// <summary>
        /// Converts string builder to byte array
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Byte array of string</returns>
        public static Byte[] SBToBytes(this StringBuilder s)
        {
            if (s.Length == 0)
                return null;
            int length = s.Length;
            Byte[] ByteArray = new Byte[(length / 2)];
            try
            {
                for (int i = 0; i < length; i += 2)
                {
                    //each byte consists of two Hex numbers.                     
                    ByteArray[i / 2] = Convert.ToByte(s.ToString(i, 2), 16);
                }
            }
            catch (FormatException e) { return null; }
            return ByteArray;
        }
        /// <summary>
        /// Appends specified length of bytes of hex string equivalent of byte array to string builder
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="Buffer">Buffer to be appended</param>
        /// <param name="BufferLength">bytes to be appended from the buffer if this size larger than the buffer size only the buffer size will be appended</param>
        /// <returns>Appended stringbuilder with hex values of bytes specified by bufferlength from buffer</returns>
        public static StringBuilder Append(this StringBuilder sb, byte[] Buffer, uint BufferLength)
        {
            int length = (int)(BufferLength <= (uint)Buffer.Length ? BufferLength : (uint)Buffer.Length);
            for (int i = 0; i < length; i++)
                sb.Append( /*String.Format("{0:X2}", Buffer[i])*/Buffer[i].ToString("X2"));

            return sb;
        }
        /// <summary>
        /// Minus two byte arrays
        /// </summary>
        /// <param name="input"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static byte[] Minus(this byte[] input, byte[] src)
        {

            src = src.Reverse().ToArray();
            var dst = input.Reverse().ToArray().Copy();
            for (int i = 0; i < dst.Length; ++i)
            {
                byte odst = dst[i];
                byte osrc = i < src.Length ? src[i] : (byte)0;

                if (osrc <= odst)
                    dst[i] -= osrc;
                else
                {
                    bool Found = false;
                    for (int j = i + 1; j < dst.Length; j++)
                    {
                        if (dst[j] > 0)
                        {
                            Found = true;
                            int val = dst[i] + 0xff;
                            val -= osrc;
                            dst[i] = (byte)val;
                            dst[j]--;
                            break;
                        }
                        dst[j] = 0xFF;
                    }
                    //tODO: add if not found to change to negative incase of i
                }

            }
            dst = dst.Reverse().ToArray();
            return dst;
        }
        /// <summary>
        /// Adds byte array as if they are integers
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <returns>Added array </returns>
        /// <remaks> the array is not changed only the result is changed </remaks>
        public static byte[] Add(this byte[] dst, byte[] src)
        {
            int carry = 0;
            src = src.Reverse().ToArray();
            dst = dst.Reverse().ToArray();
            for (int i = 0; i < dst.Length; ++i)
            {
                byte odst = dst[i];
                byte osrc = i < src.Length ? src[i] : (byte)0;

                byte ndst = (byte)(odst + osrc + carry);

                dst[i] = ndst;
                carry = ndst < odst ? 1 : 0;
            }
            dst = dst.Reverse().ToArray();
            return dst;
        }

        public static byte[] GetBytesFrom(this StringBuilder sb, int HexLocation)
        {
            return sb.ToString(HexLocation * 2, sb.Length - HexLocation * 2).ToBytes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="offset"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static byte[] Xor(this byte[] b, int offset, byte[] Data)
        {
            if (b.Length < offset + Data.Length)
                throw new IndexOutOfRangeException();
            for (int i = 0; i < Data.Length; i++)
                b[i + offset] = (byte)(b[i + offset] ^ Data[i]);
            return b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static byte[] Xor(this byte[] b, byte[] Data)
        {
            return Xor(b, 0, Data);

        }
        /// <summary>
        /// Copy 
        /// </summary>
        /// <param name="b">array</param>
        /// <returns>copy of array</returns>
        public static byte[] Copy(this byte[] b)
        {
            byte[] rv = new byte[b.Length];
            System.Buffer.BlockCopy(b, 0, rv, 0, b.Length);
            return rv;
        }
        public static bool HasLe(this string str)
        {
            int HexLength = str.Length / 2;
            if (HexLength < 4 || str.Length % 2 != 0)
            {
                throw new Exception("Not an APDU");
            }
            if (HexLength == 5) return true;
            if (HexLength == 4) return false;
            int DataLength = int.Parse(str.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            if (DataLength + 6 == HexLength) return true;
            if (DataLength + 5 == HexLength) return false;
            throw new Exception("Not an APDU");
        }
        /// <summary>
        /// Returns the length as hext string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="RequestedLength"></param>
        /// <returns></returns>
        public static string LengthAsHexString(this string str, int RequestedLength)
        {
            return (str.Length / 2).ToString($"X{RequestedLength}");
        }
    

    }
}