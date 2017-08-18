using smartcardLib.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace smartcardLib.Core
{
    /// <summary>
    /// ISO7816 Padding (EMV) Padding
    /// </summary>
    public enum ISO7618Padding
    {
        /// <summary>
        /// EMV padding 
        /// </summary>
        EMV
    }
    /// <summary>
    /// ASN1 class
    /// </summary>
    public class ASN1
    {
        //public const byte OCTET_STRING = 0x0A;

        //ASN1 Universal Object Tag values
        /// <summary>
        /// Boolean
        /// </summary>
        public const byte BOOLEAN = 0x01;
        /// <summary>
        /// Integer
        /// </summary>
        public const byte INTEGER = 0x02;
        /// <summary>
        /// BIT STRING
        /// </summary>
        public const byte BIT_STRING = 0x03;
        /// <summary>
        /// OCTET STRING
        /// </summary>
        public const byte OCTET_STRING = 0x04;
        /// <summary>
        /// NULL
        /// </summary>
        public const byte NULL = 0x05;
        /// <summary>
        /// Object identifier
        /// </summary>
        public const byte OBJECT_IDENTIFIER = 0x06;
        /// <summary>
        /// Object descriptor
        /// </summary>
        public const byte Object_Descriptor = 0x07;
        /// <summary>
        /// External
        /// </summary>
        public const byte EXTERNAL = 0x08;
        /// <summary>
        /// Real
        /// </summary>
        /// <remarks> Not implemented</remarks>
        public const byte REAL = 0x09;
        /// <summary>
        /// Enumerated
        /// </summary>
        public const byte ENUMERATED = 0x0A;
        /// <summary>
        /// Embedded PDV
        /// </summary>
        /// <remarks>Not Implemented </remarks>
        public const byte EMBEDDED_PDV = 0x0B;
        /// <summary>
        /// UTF8 string
        /// </summary>
        public const byte UTF8String = 0x0C;
        /// <summary>
        /// Relative OID
        /// </summary>
        public const byte RELATIVE_OID = 0x0D;
        /// <summary>
        /// SEQUENCE [and constructive]
        /// </summary>
        public const byte SEQUENCE = 0x30;
        /// <summary>
        /// SET [and constructive]
        /// </summary>
        public const byte SET = 0x31;
        /// <summary>
        /// Numeric String
        /// </summary>
        public const byte NumericString = 0x12;
        /// <summary>
        /// Printable string
        /// </summary>
        public const byte PrintableString = 0x13;
        /// <summary>
        /// T61 string
        /// </summary>
        public const byte T61String = 0x14;
        /// <summary>
        /// VideoTex string
        /// </summary>
        public const byte VideotexString = 0x15;
        /// <summary>
        /// IA5 String
        /// </summary>
        public const byte IA5String = 0x16;
        /// <summary>
        /// UTC time
        /// </summary>
        public const byte UTCTime = 0x17;
        /// <summary>
        /// Generalized Time
        /// </summary>
        public const byte GeneralizedTime = 0x18;
        /// <summary>
        /// Graphic string
        /// </summary>
        public const byte GraphicString = 0x19;
        /// <summary>
        /// Visible string
        /// </summary>
        public const byte VisibleString = 0x1A;
        /// <summary>
        /// General string
        /// </summary>
        public const byte GeneralString = 0x1B;
        /// <summary>
        /// Universal string
        /// </summary>
        public const byte UniversalString = 0x1C;
        /// <summary>
        /// Character string
        /// </summary>
        public const byte CHARACTER_STRING = 0x1D;
        /// <summary>
        /// BMP string
        /// </summary>
        public const byte BMPString = 0x1E;
        /// <summary>
        /// Universal Object names and associated Tag values
        /// </summary>
        public enum NAME
        {

            /// <summary>
            /// Boolean
            /// </summary>
            BOOLEAN = 1,
            /// <summary>
            /// Integer
            /// </summary>
            INTEGER = 2,
            /// <summary>
            /// BIT STRING
            /// </summary>
            BITSTRING = 3,
            /// <summary>
            /// OCTET STRING
            /// </summary>
            OCTETSTRING = 4,
            /// <summary>
            /// NULL
            /// </summary>
            NULL = 5,
            /// <summary>
            /// Object Identifier
            /// </summary>
            OBJECTIDENTIFIER = 6,
            /// <summary>
            /// Object descriptor
            /// </summary>
            ObjectDescriptor = 7,
            /// <summary>
            /// Enumerated
            /// </summary>
            ENUMERATED = 0x0a,
            /// <summary>
            /// REAL
            /// </summary>
            /// <remarks> REAL is not implmented</remarks>
            REAL = 0x09,
            /// <summary>
            /// Embdded PDV
            /// </summary>
            /// <remarks> NOT implmented Type</remarks>
            EMBEDDED_PDV = 0x0B,
            /// <summary>
            /// UTF8 string
            /// </summary>
            UTF8String = 0x0c,
            /// <summary>
            /// SEQUENCE
            /// </summary>
            SEQUENCE = 0x10,
            /// <summary>
            /// SET
            /// </summary>
            SET = 0x11,
            /// <summary>
            /// Numeric string
            /// </summary>
            NumericString = 0x12,
            /// <summary>
            /// Printable string
            /// </summary>
            PrintableString = 0x13,
            /// <summary>
            /// Teletext String
            /// </summary>
            TeletexString = 0x14,
            /// <summary>
            /// Video Text string
            /// </summary>
            VideotexString = 0x15,
            /// <summary>
            /// IA5 string
            /// </summary>
            IA5String = 0x16,
            /// <summary>
            /// UTC time
            /// </summary>
            UTCTime = 0x17,
            /// <summary>
            /// Generalized Time
            /// </summary>
            GeneralizedTime = 0x18,
            /// <summary>
            /// Graphic String
            /// </summary>
            GraphicString = 0x19,
            /// <summary>
            /// Visible String
            /// </summary>
            VisibleString = 0x1A,
            /// <summary>
            /// General string
            /// </summary>
            GeneralString = 0x1B,
            /// <summary>
            /// Universal string
            /// </summary>
            UniversalString = 0x1C,
            /// <summary>
            /// Character string
            /// </summary>
            CHARACTER_STRING = 0x1d,
            /// <summary>
            /// BMP string
            /// </summary>
            BMPString = 0x1E
        }
        /// <summary>
        /// Constructor that uses TAG and data (value) the length will be appended
        /// </summary>
        /// <param name="TAG">Tag</param>
        /// <param name="Data">Value as byte array</param>
        public ASN1(byte TAG, byte[] Data)
        {
            byte[] Val = new byte[] { TAG };
            Val = Val.Append(GetLengthArray(Data.Length));
            Val = Val.Append(Data);
            Parse(Val, 0);
        }
        /// <summary>
        /// Constructor that assume an ASN1 object from the byte array
        /// </summary>
        /// <param name="Data">byte array that includes a full ASN1 object</param>
        public ASN1(byte[] Data) : this(Data, 0) { }
        /// <summary>
        /// Parses an ASN1 object
        /// </summary>
        /// <param name="Data">byte array to be parsed</param>
        /// <param name="offset">offset to which the parse begins</param>
        /// <remarks>
        /// ASN1 with Tag length more than 1 byte not supported
        /// value is not populated unless it is used
        /// </remarks>
        void Parse(byte[] Data, int offset)
        {
            _data = Data;
            Tag = Data[offset];
            Children = new List<ASN1>();
            if ((Tag & 0x1F) == 0x1F) throw new NotSupportedException("Parsing long ASN Tags is not supported ");
            int size;
            length = GetLength(Data, offset + 1, out size);
            TotalLength = length + size + 1;
            _offset = offset + size + 1;
            IsConstructed = ((Tag & 0x20) != 0x0);
            TagClass = (Tag & 0xC0) >> 6;
            IsContext = (TagClass == 2);
            Primtag = Tag & 31;
            tagName = "[";
            switch (this.TagClass)
            {
                case 0:
                    this.tagName += "Universal";
                    break;
                case 1:
                    this.tagName += "Application";
                    break;
                case 2:
                    //this.tagName = null;
                    LoggerFac.GetDefaultLogger().LogInformation("tag class is null !");
                    break;
                case 3:
                    this.tagName += "Private";
                    break;
                case 0x1F:
                    //TODO: support long tag
                    throw new NotSupportedException("Parsing long ASN Tags is not supported ");
                default:
                    LoggerFac.GetDefaultLogger().LogWarning("Couldn't determine the tag class");
                    break;
            }
            TagNumber = Tag & 0x1F;
            if (IsContext) this.tagName += $"{Primtag}]";
            else this.tagName += $" {this.TagNumber}]";
            if (!IsConstructed) this.type += Enum.GetName(typeof(NAME), Primtag);
            //if (Data.Length < _offset + length) Next = new ASN1(Data, TotalLength);
            var Movement = 0;
            var Pointer = _offset + size + 1;
            while (Pointer - 1 - size - _offset < length)
            {
                if (IsConstructed)
                {
                    var child = new ASN1(Data, _offset + Movement);
                    Children.Add(child);
                    Movement += child.TotalLength;
                    Pointer += child.TotalLength;
                }
                else Pointer += length;
            }
        }

        /// <summary>
        /// A constructor that assume that there is an ASN1 instance in a byte array at specific offset
        /// </summary>
        /// <param name="Data">byte array</param>
        /// <param name="offset">offset</param>
        public ASN1(byte[] Data, int offset)
        {
            Parse(Data, offset);

        }
        /// <summary>
        /// Is the current ASN1 a string
        /// </summary>
        /// <returns></returns>
        internal bool IsAsn1String()
        {
            switch (Tag)
            {
                case UTF8String:
                case RELATIVE_OID:
                case NumericString:
                case PrintableString:
                case T61String:
                case VideotexString:
                case GraphicString:
                case VisibleString:
                case GeneralString:
                case UniversalString:
                case CHARACTER_STRING:
                case BMPString:
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Is universal string
        /// </summary>
        /// <returns>true if universal string</returns>
        internal bool IsUniversalString()
        {
            return Tag == UniversalString;
        }

        /// <summary>
        /// Constructor that uses TAG and data and required the data to be padded
        /// </summary>
        /// <param name="TAG">TAG</param>
        /// <param name="Data">value of data</param>
        /// <param name="Padding">Padding technique</param>
        public ASN1(byte TAG, byte[] Data, ISO7618Padding Padding)
        {
            byte[] Val = new byte[] { TAG };
            switch (Padding)
            {
                case ISO7618Padding.EMV:
                    Data = PaddWithEMV(Data);
                    break;
                    //TODO add more padding      
            }
            Val = Val.Append(GetLengthArray(Data.Length));
            Val = Val.Append(Data);
            Parse(Data, 0);
        }
        /// <summary>
        /// Sets the name of the ASN1 object
        /// </summary>
        /// <param name="v">string name</param>
        internal void setName(string v)
        {
            Name = v;
        }

        /// <summary>
        /// Get a children in the ASN tree
        /// </summary>
        /// <param name="v">child number</param>
        /// <returns>ASN1 child object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal ASN1 get(int v)
        {
            return Children[v];
        }
        /// <summary>
        /// Tag value
        /// </summary>
        /// <remarks> Only one byte tag is supported in this ASN1</remarks>
        public byte Tag;
        /// <summary>
        /// Name of the ASN1 object can be set to anything
        /// </summary>
        public string Name;
        /// <summary>
        /// Length of the ASN1 value data
        /// </summary>
        public int length;
        /// <summary>
        /// value data
        /// </summary>
        /// <remarks>Only populated when accessing the value</remarks>
        private byte[] _data;
        /// <summary>
        /// offset in the byte array that holds the object
        /// </summary>
        private int _offset;

        /// <summary>
        /// List of all children ASN1
        /// </summary>
        public List<ASN1> Children;
        /// <summary>
        /// true if the ASN1 object is constructed
        /// </summary>
        public bool IsConstructed;
        /// <summary>
        /// Type
        /// </summary>
        public string type;
        /// <summary>
        /// This function will get the length of value according to TLV standard
        /// </summary>
        /// <param name="Data">Byte array</param>
        /// <param name="Offset">Offset to which the length is stored</param>
        /// <param name="SizeOfLength">out the length of the length field in the ASN1 object</param>
        /// <returns>integer value of the length</returns>
        /// <remarks>ASN1 object with null termination i.e. length is set to 0x80 is not supported yet</remarks>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private int GetLength(byte[] Data, int Offset, out int SizeOfLength)
        {
            byte LengthFirstByte = Data[Offset];
            int Length = 0;
            SizeOfLength = 1;
            if ((LengthFirstByte & 0x80) == 0x80)
            {
                if (LengthFirstByte == 0x80) throw new NotSupportedException("ASN with length is only supported");
                SizeOfLength = LengthFirstByte & 0x7F;
                if (SizeOfLength > 4) throw new ArgumentOutOfRangeException("ASN Length is more than 4 GB");
                for (int i = 0; i < SizeOfLength; i++)
                {
                    Length <<= 8;
                    Length |= Data[Offset + i + 1];
                }
                SizeOfLength++;
            }
            else
            {
                Length = LengthFirstByte;
                if (Length + Offset + 1 > Data.Length)
                    throw new IndexOutOfRangeException("ASN length is larger than buffer");
            }
            return Length;
        }
        /// <summary>
        /// This is a helped function used internally to pad data with EMV standard padding
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>data padded with EMV standard similar to RSA 1.5 padding</returns>
        byte[] PaddWithEMV(byte[] data)
        {
            data = data.Append(new byte[] { 0x80 });

            if (data.Length % 8 != 0)
            {
                var d = new byte[data.Length % 8];
                data = data.Append(d);

            }
            return data;
        }
        #region Documentation
        /// <summary>
        /// Get Length as byte array
        /// </summary>
        /// <param name="length">returns the length of the ASN1 instance as byte array</param>
        /// <returns>byte array of the length</returns>
        #endregion
        private byte[] GetLengthArray(int length)
        {
            if (length < 0x80) return new byte[] { (byte)length };
            else
            {
                byte[] Result = new byte[] { 0x80 };
                int i = length;
                while (i > 0)
                {
                    Result[0]++;
                    Result = Result.Append(new byte[] { (byte)(i & 0xFF) });
                    i >>= 8;
                }
                return Result;
            }
        }

        /// <summary>
        /// Converts the ASN1 object to readable string
        /// </summary>
        /// <returns>Readable string</returns>
        public override string ToString()
        {
            return $"\t{Name} {type}: tag ={tagName} {(IsConstructed ? "constructed" : "primative")}; length = {length} {(IsConstructed ? (string.Join("\n\t", this.Children.Select(x => x.ToString()).ToArray())) : $"\n{this.Value.ToHex()}")}";
        }

        /// <summary>
        /// Convert the ASN1 object to string and tries also
        /// to convert the children of the object if universal to a string representation
        /// </summary>
        /// <returns>String representation of the ASN1 object and its children</returns>
        public string ToStringAndConvert()
        {
            object val;
            Type t;
            bool haveValue = TryGetValue(out val, out t);
            if (typeof(byte[]) == t) return string.Join(" : ", Children.Select(x => x.ToStringAndConvert()).ToArray());
            return $@"{(IsConstructed ? "Constructed Data" : "Primitive Data")} : TAG:{
                Tag}, TagClass:{TagClass}, Value {
                (haveValue ? ((val == null) ? "NULL" :
                (string)t.InvokeMember("ToString", System.Reflection.BindingFlags.InvokeMethod, null, val, null))
                : Value.ToHex()) }, Child Nodes {
                string.Join(" : ", Children.Select(x => x.ToStringAndConvert()).ToArray())}";
        }
        /// <summary>
        /// Tries to convert the Value object to a specific type
        /// </summary>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <param name="value">output of conversion</param>
        /// <returns>if succeeded will return true otherwise will return false</returns>
        /// <remarks>in case of false the value will be populated with the default value for value objects</remarks>
        public bool TryGetValue<T>(out T value)
        {
            try
            {
                value = GetValue<T>();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFac.GetDefaultLogger().LogError(872, "Conversion didn't succeed ", ex);
                value = default(T);
                return false;
            }
        }
        /// <summary>
        /// Tries to convert the Value object according to the Tag value and returns the output as (object) that is default type of the appropriate 
        /// encoded value
        /// </summary>
        /// <param name="val">encoded value decoded to a specific default object type</param>
        /// <param name="type">the type of the decoded val</param>
        /// <returns>true in case of success otherwise error</returns>
        public bool TryGetValue(out object val, out Type type)
        {
            bool ret = false;
            val = null;
            type = null;
            try
            {
                switch (Tag)
                {
                    case 0:
                        {
                            type = typeof(byte[]);
                            val = GetValue<Byte[]>();
                            ret = true;
                        }
                        break;
                    case BOOLEAN:
                        {
                            type = typeof(bool);
                            val = GetValue<bool>();
                            ret = true;
                        }
                        break;
                    case ENUMERATED:
                    case INTEGER:
                        {
                            type = typeof(int);
                            val = GetValue<int>();
                            ret = true;
                        }
                        break;
                    case BIT_STRING:
                    case 4:
                    case OBJECT_IDENTIFIER:
                    case Object_Descriptor:
                    case UTF8String:
                    case RELATIVE_OID:
                    case NumericString:
                    case PrintableString:
                    case T61String:
                    case VideotexString:
                    case GraphicString:
                    case VisibleString:
                    case GeneralString:
                    case UniversalString:
                    case CHARACTER_STRING:
                    case BMPString:
                        {
                            type = typeof(string);
                            val = GetValue<string>();
                            ret = true;
                        }
                        break;
                    case GeneralizedTime:
                    case UTCTime:
                        {
                            type = typeof(DateTime);
                            val = GetValue<DateTime>();
                            ret = true;
                        }
                        break;

                    case NULL:
                        {
                            type = typeof(object);
                            val = null;
                            ret = true;
                        }
                        break;
                    case REAL:
                        {
                            type = typeof(Double);
                            val = GetValue<Double>(); ;
                            ret = true;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerFac.GetDefaultLogger().LogError(575, "Couldn't value of the ASN1 object", ex);
                ret = false;
            }
            return ret;

        }
        /// <summary>
        /// Converts the value to integer
        /// </summary>
        /// <returns>integer value of the </returns>
        int ConvertValueToInt()
        {
            int r0 = 0;
            for (int i = 0; i < length; i++)
            {
                r0 += Value[i];
                if (i != length - 1)
                    r0 <<= 8;
            }
            if ((Value[0] & 0x80) == 0x80)
            {
                byte b0 = 0xff;
                r0 |= b0 << 24;
            }
            return r0;
        }
        /// <summary>
        /// Converts the value to long
        /// </summary>
        /// <returns>long value of the object </returns>
        long ConvertValueToLong()
        {
            long r1 = 0;
            for (int i = 0; i < length; i++)
            {
                r1 += Value[i];
                if (i != length - 1)
                    r1 <<= 8;
            }
            if ((Value[0] & 0x80) == 0x80)
            {
                long b0 = 0xff;
                r1 |= b0 << (24 + 36);
            }
            return r1;
        }
        /// <summary>
        /// converts a bit string to binary string array
        /// </summary>
        /// <returns>converts a bit string to binary string array</returns>
        string ConvertBitStringValueToString()
        {
            if (Value.Length == 1)
            {
                Debug.Assert(Value[0] == 0);
                return "";
            }
            int Left = (int)Value[0];

            var res = Value.Skip(1).ToArray().ToBinary();
            return res.Substring(0, res.Length - Left);
        }

        /// <summary>
        /// Coverts the value to enumeration and returns the enumeration integer value
        /// </summary>
        /// <returns>enumeration integer value</returns>
        int ConvertValueToEnumerated()
        {
            if (length > 4) throw new InvalidCastException("Enumerated value is larger than int MAX");
            int r = 0;
            for (int i = 0; i < length; i++)
            {
                r += Value[i];
                r <<= 8;
            }
            return r;
        }
        /// <summary>
        /// Gets the value according to the required type if possible
        /// </summary>
        /// <typeparam name="T">Type for the output data</typeparam>
        /// <returns>Data converted to the type if possible</returns>
        /// <exception cref="InvalidCastException">will throw invalid cast if couldn't convert to specified type</exception>
        public T GetValue<T>()
        {
            switch (Tag)
            {

                case 0:
                    if (typeof(T) == typeof(byte[])) return (T)(object)Value;
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case BOOLEAN:
                    if (typeof(T) == typeof(bool)) return (T)(object)(Value[0] != 0x00);
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case INTEGER:
                    if (!(typeof(T) != typeof(int) || typeof(T) != typeof(long))) throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                    if (length > 4 && typeof(T) == typeof(int)) throw new InvalidCastException("Integer value is larger than int MAX");
                    if (length > 4 && typeof(T) == typeof(long)) throw new InvalidCastException("Integer value is larger than long MAX");
                    if (typeof(T) == typeof(int)) return (T)(object)ConvertValueToInt();
                    else return (T)(object)ConvertValueToLong();
                case BIT_STRING:
                    if (typeof(T) == typeof(string)) return (T)(object)ConvertBitStringValueToString();
                    if (typeof(T) == typeof(int) || typeof(T) == typeof(uint)) return (T)(object)Value.IntFromBitString();
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case 0x04:
                    if (typeof(T) == typeof(string))
                        return (T)(object)Value.ToHex();
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case NULL:
                    if (typeof(T).IsClass)
                        return (T)(object)null;
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case OBJECT_IDENTIFIER:
                case Object_Descriptor:
                case RELATIVE_OID:
                    if (typeof(T) == typeof(string))
                        return (T)(object)Value.OidString();
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case UTF8String:
                    if (typeof(T) == typeof(string))
                        return (T)(object)ASCIIEncoding.UTF8.GetString(Value);
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case REAL:
                    //TODO: implement real encoding
                    throw new NotImplementedException();
                case ENUMERATED:
                    if (typeof(T) == typeof(int)) return (T)(object)ConvertValueToEnumerated();
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case NumericString:
                case PrintableString:
                case T61String:
                case VideotexString:
                case IA5String:
                case GraphicString:
                case CHARACTER_STRING:
                    if (typeof(T) == typeof(string)) return (T)(object)ASCIIEncoding.ASCII.GetString(Value);
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case UniversalString:
                    if (typeof(T) == typeof(string))
                    {
                        Encoding enc = new UnicodeEncoding(false, true, true);
                        return (T)(object)enc.GetString(Value);
                    }
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case VisibleString:
                    if (typeof(T) == typeof(string))
                        return (T)(object)ASCIIEncoding.Unicode.GetString(Value);
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case BMPString:
                    if (typeof(T) != typeof(string))
                    {
                        if (Value[0] == 0) return (T)(object)Encoding.GetEncoding(1201).GetString(Value);
                        return (T)(object)Encoding.GetEncoding(1201).GetString(Value);
                    }
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case GeneralizedTime:
                    if (typeof(T) == typeof(string))
                        return (T)(object)ASCIIEncoding.ASCII.GetString(Value);
                    if (typeof(T) == typeof(DateTime))
                        return (T)(object)DateTime.ParseExact(ASCIIEncoding.ASCII.GetString(Value), "yyyyMMddhhmmss.ffffZ", CultureInfo.InvariantCulture);
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                case UTCTime:
                    if (typeof(T) == typeof(string))
                        return (T)(object)ASCIIEncoding.ASCII.GetString(Value);
                    if (typeof(T) == typeof(DateTime)) return (T)(object)DateTime.ParseExact(ASCIIEncoding.ASCII.GetString(Value), "yyMMddHHmmssZ", CultureInfo.InvariantCulture);
                    throw new InvalidCastException($"Conversion for type {typeof(T).Name} is not enabled for {Tag}");
                default:
                    throw new NotImplementedException();

            }
            throw new NotImplementedException();
        }
        /// <summary>
        /// returns the value and sets the _value to a new byte array of the value
        /// </summary>
        public byte[] Value
        {

            get
            {
                if (_value == null)
                {
                    _value = new byte[length];
                    Buffer.BlockCopy(_data, _offset, _value, 0, length);
                }
                return _value;
            }
        }
        /// <summary>
        /// Total length of the ASN1 object [Tag, Length and value]
        /// </summary>
        public int TotalLength { get; private set; }
        /// <summary>
        /// this will hold the a pointer to the value when ever value is called
        /// </summary>
        private byte[] _value;
        /// <summary>
        /// Tag class
        /// </summary>
        private int TagClass;
        /// <summary>
        /// children count
        /// </summary>
        internal int elements
        {
            get
            {
                return Children.Count;
            }
        }
        /// <summary>
        /// Is context ASN1 object
        /// </summary>
        public bool IsContext { get; private set; }
        /// <summary>
        /// Primary tag
        /// </summary>
        public int Primtag { get; private set; }
        /// <summary>
        /// Tag Name
        /// </summary>
        public string tagName { get; private set; }
        /// <summary>
        /// Tag Number
        /// </summary>
        public int TagNumber { get; private set; }


        public byte[] FullData
        {
            get
            {
                return _data;
            }
        }
    }
}