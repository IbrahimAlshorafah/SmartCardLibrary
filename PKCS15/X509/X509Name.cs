using smartcardLib;
using smartcardLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcardLib.X509
{
    /// <summary>
    ///  RDNSequence ::= SEQUENCE OF RelativeDistinguishedName
    ///     RelativeDistinguishedName ::= SET SIZE(1..MAX) OF AttributeTypeAndValue
    ///  AttributeTypeAndValue::= SEQUENCE {
    ///type OBJECT IDENTIFIER,
    ///value ANY }
    /// </summary>
     class X509Name
    {
        /// <summary>
        /// country code - StringType(SIZE(2))
        /// </summary>
        public static readonly string C = "2.5.4.6";
        /// <summary>
        /// organization - StringType(SIZE(1..64))
        /// </summary>
        public static readonly string O = "2.5.4.10";
        /// <summary>
        /// organizational unit name - StringType(SIZE(1..64))
        /// </summary>
        public static readonly string OU = "2.5.4.11";
        /// <summary>
        /// Title
        /// </summary>
        public static readonly string T = "2.5.4.12";
        /// <summary>
        /// common name - StringType(SIZE(1..64))
        /// </summary>
        public static readonly string CN = "2.5.4.3";
        /// <summary>
        /// street - StringType(SIZE(1..64))
        /// </summary>
        public static readonly string Street = "2.5.4.9";
        /// <summary>
        /// device serial number name - StringType(SIZE(1..64))
        /// </summary>
        public static readonly string SerialNumber = "2.5.4.5";
        /// <summary>
        /// locality name - StringType(SIZE(1..64))
        /// </summary>
        public static readonly string L = "2.5.4.7";
        /// <summary>
        /// state, or province name - StringType(SIZE(1..64))
        /// </summary>
        public static readonly string ST = "2.5.4.8";
        /// <summary>
        /// Naming attributes of type X520name
        /// </summary>
        public static readonly string Surname = "2.5.4.4";
        public static readonly string GivenName = "2.5.4.42";
        public static readonly string Initials = "2.5.4.43";
        public static readonly string Generation = "2.5.4.44";
        public static readonly string UniqueIdentifier = "2.5.4.45";
        /// <summary>
        /// businessCategory - DirectoryString(SIZE(1..128)
        /// </summary>
        public static readonly string BusinessCategory = "2.5.4.15";
        /// <summary>
         /// postalCode - DirectoryString(SIZE(1..40)
         /// </summary>
        public static readonly string PostalCode = "2.5.4.17";

        /// <summary>
        /// dnQualifier - DirectoryString(SIZE(1..64)
        /// </summary>
        public static readonly string DnQualifier = "2.5.4.46";

        /// <summary>
        /// RFC 3039 Pseudonym - DirectoryString(SIZE(1..64)
        /// </summary>
        public static readonly string Pseudonym = "2.5.4.65";

        /// <summary>
        /// RFC 3039 DateOfBirth - GeneralizedTime - YYYYMMDD000000Z
        /// </summary>
        public static readonly string DateOfBirth = "1.3.6.1.5.5.7.9.1";

        /// <summary>
        /// RFC 3039 PlaceOfBirth - DirectoryString(SIZE(1..128)
        /// </summary>
        public static readonly string PlaceOfBirth = "1.3.6.1.5.5.7.9.2";

        /// <summary>
        /// RFC 3039 DateOfBirth - PrintableString (SIZE(1)) -- "M", "F", "m" or "f"
        /// </summary>
        public static readonly string Gender = "1.3.6.1.5.5.7.9.3";

        /// <summary>
        /// RFC 3039 CountryOfCitizenship - PrintableString (SIZE (2)) -- ISO 3166
        /// codes only
        /// </summary>
        public static readonly string CountryOfCitizenship = "1.3.6.1.5.5.7.9.4";

        /// <summary>
        /// RFC 3039 CountryOfCitizenship - PrintableString (SIZE (2)) -- ISO 3166
        /// codes only
        /// </summary>
        public static readonly string CountryOfResidence = "1.3.6.1.5.5.7.9.5";

        /// <summary>
        /// ISIS-MTT NameAtBirth - DirectoryString(SIZE(1..64)
        /// </summary>
        public static readonly string NameAtBirth = "1.3.36.8.3.14";

        /// <summary>
        /// RFC 3039 PostalAddress - SEQUENCE SIZE (1..6) OF
        /// DirectoryString(SIZE(1..30))
        /// </summary>
        public static readonly string PostalAddress = "2.5.4.16";

        /// <summary>
        /// RFC 2256 dmdName
        /// </summary>
        public static readonly string DmdName = "2.5.4.54";

        /// <summary>
        /// id-at-telephoneNumber
        /// </summary>
        public static readonly string TelephoneNumber = "2.5.4.20";

        /// <summary>
        /// id-at-name
        /// </summary>
        public static readonly string Name = "2.5.4.41";

        /// <summary>
        /// Email address (RSA PKCS#9 extension) - IA5String.
        /// <p>Note: if you're trying to be ultra orthodox, don't use this! It shouldn't be in here.</p>
        /// </summary>
        public static readonly string EmailAddress = "1.2.840.113549.1.9.1";

        /// <summary>
        /// more from PKCS#9
        /// </summary>
        public static readonly string UnstructuredName = "1.2.840.113549.1.9.2";
        public static readonly string UnstructuredAddress = "1.2.840.113549.1.9.8";

        /// <summary>
        /// email address in Verisign certificates
        /// </summary>
        public static readonly string E = EmailAddress;

        ///<summary>
        /// others...
        /// </summary>
        public static readonly string DC = "0.9.2342.19200300.100.1.25";

        /// <summary>
        /// LDAP User id.
        /// </summary>
        public static readonly string UID = "0.9.2342.19200300.100.1.1";
        private ASN1 seq;
        private List<string> ordering;
        private List<string> values;
        static private Dictionary<string, string> DefaultSymbols;
        static private Dictionary<string, string> RFC2253Symbols;
        static private Dictionary<string, string> RFC1779Symbols;
        static private Dictionary<string, string> DefaultLookup;
        private List<bool> added;
        private X509Name()
        {
            added = new List<bool>();
            ordering = new List<string>();
            values = new List<string>();
            if (DefaultSymbols == null)
            {
                DefaultSymbols = new Dictionary<string, string>();
                DefaultSymbols.Add(C, "C");
                DefaultSymbols.Add(O, "O");
                DefaultSymbols.Add(T, "T");
                DefaultSymbols.Add(OU, "OU");
                DefaultSymbols.Add(CN, "CN");
                DefaultSymbols.Add(L, "L");
                DefaultSymbols.Add(ST, "ST");
                DefaultSymbols.Add(SerialNumber, "SERIALNUMBER");
                DefaultSymbols.Add(EmailAddress, "E");
                DefaultSymbols.Add(DC, "DC");
                DefaultSymbols.Add(UID, "UID");
                DefaultSymbols.Add(Street, "STREET");
                DefaultSymbols.Add(Surname, "SURNAME");
                DefaultSymbols.Add(GivenName, "GIVENNAME");
                DefaultSymbols.Add(Initials, "INITIALS");
                DefaultSymbols.Add(Generation, "GENERATION");
                DefaultSymbols.Add(UnstructuredAddress, "unstructuredAddress");
                DefaultSymbols.Add(UnstructuredName, "unstructuredName");
                DefaultSymbols.Add(UniqueIdentifier, "UniqueIdentifier");
                DefaultSymbols.Add(DnQualifier, "DN");
                DefaultSymbols.Add(Pseudonym, "Pseudonym");
                DefaultSymbols.Add(PostalAddress, "PostalAddress");
                DefaultSymbols.Add(NameAtBirth, "NameAtBirth");
                DefaultSymbols.Add(CountryOfCitizenship, "CountryOfCitizenship");
                DefaultSymbols.Add(CountryOfResidence, "CountryOfResidence");
                DefaultSymbols.Add(Gender, "Gender");
                DefaultSymbols.Add(PlaceOfBirth, "PlaceOfBirth");
                DefaultSymbols.Add(DateOfBirth, "DateOfBirth");
                DefaultSymbols.Add(PostalCode, "PostalCode");
                DefaultSymbols.Add(BusinessCategory, "BusinessCategory");
                DefaultSymbols.Add(TelephoneNumber, "TelephoneNumber");
            }
            if (RFC2253Symbols == null)
            {
                RFC2253Symbols = new Dictionary<string, string>();
                RFC2253Symbols.Add(C, "C");
                RFC2253Symbols.Add(O, "O");
                RFC2253Symbols.Add(OU, "OU");
                RFC2253Symbols.Add(CN, "CN");
                RFC2253Symbols.Add(L, "L");
                RFC2253Symbols.Add(ST, "ST");
                RFC2253Symbols.Add(Street, "STREET");
                RFC2253Symbols.Add(DC, "DC");
                RFC2253Symbols.Add(UID, "UID");
            }
            if (RFC1779Symbols == null)
            {
                RFC1779Symbols = new Dictionary<string, string>();
                RFC1779Symbols.Add(C, "C");
                RFC1779Symbols.Add(O, "O");
                RFC1779Symbols.Add(OU, "OU");
                RFC1779Symbols.Add(CN, "CN");
                RFC1779Symbols.Add(L, "L");
                RFC1779Symbols.Add(ST, "ST");
                RFC1779Symbols.Add(Street, "STREET");
            }
            if (DefaultLookup == null)
            {
                DefaultLookup = new Dictionary<string, string>();
                DefaultLookup.Add("c", C);
                DefaultLookup.Add("o", O);
                DefaultLookup.Add("t", T);
                DefaultLookup.Add("ou", OU);
                DefaultLookup.Add("cn", CN);
                DefaultLookup.Add("l", L);
                DefaultLookup.Add("st", ST);
                DefaultLookup.Add("serialnumber", SerialNumber);
                DefaultLookup.Add("street", Street);
                DefaultLookup.Add("emailaddress", E);
                DefaultLookup.Add("dc", DC);
                DefaultLookup.Add("e", E);
                DefaultLookup.Add("uid", UID);
                DefaultLookup.Add("surname", Surname);
                DefaultLookup.Add("givenname", GivenName);
                DefaultLookup.Add("initials", Initials);
                DefaultLookup.Add("generation", Generation);
                DefaultLookup.Add("unstructuredaddress", UnstructuredAddress);
                DefaultLookup.Add("unstructuredname", UnstructuredName);
                DefaultLookup.Add("uniqueidentifier", UniqueIdentifier);
                DefaultLookup.Add("dn", DnQualifier);
                DefaultLookup.Add("pseudonym", Pseudonym);
                DefaultLookup.Add("postaladdress", PostalAddress);
                DefaultLookup.Add("nameofbirth", NameAtBirth);
                DefaultLookup.Add("countryofcitizenship", CountryOfCitizenship);
                DefaultLookup.Add("countryofresidence", CountryOfResidence);
                DefaultLookup.Add("gender", Gender);
                DefaultLookup.Add("placeofbirth", PlaceOfBirth);
                DefaultLookup.Add("dateofbirth", DateOfBirth);
                DefaultLookup.Add("postalcode", PostalCode);
                DefaultLookup.Add("businesscategory", BusinessCategory);
                DefaultLookup.Add("telephonenumber", TelephoneNumber);
            }
        }
        /// <summary>
        /// Constructor from Asn1Sequence
        /// the principal will be a list of constructed sets, each containing an (OID, string) pair.
        /// </summary>
        public X509Name( ASN1 tlv):this()
        {
            this.seq = tlv;
            foreach (ASN1 asn1Obj in tlv.Children)
            {
                for (int i = 0; i < asn1Obj.elements; i++)
                {
                    ASN1 s = asn1Obj.get(i);

                    if (s.elements != 2)
                        throw new ArgumentException("badly sized pair");

                    ordering.Add(s.get(0).GetValue<string>());

                    var derValue = s.get(1);
                    if (derValue.IsAsn1String() && !(derValue.IsUniversalString()))
                    {
                        string v =derValue.GetValue<string>();
                        if (v.StartsWith("#"))
                        {
                            v = "\\" + v;
                        }

                        values.Add(v);
                    }
                    else
                    {
                        values.Add("#" + derValue.Value.ToHex()) ;
                    }
                    added.Add(i != 0);
                }
            }
        }
        /// <summary>
        /// Append Name and values to a string buffer
        /// </summary>
        /// <param name="strBuffer"></param>
        /// <param name="oidSymbols"></param>
        /// <param name="oid"></param>
        /// <param name="val"></param>
        private void AppendValue(
            StringBuilder strBuffer,
            Dictionary<string,string> oidSymbols,
            string oid,
            string val)
        {
            string sym = (string)oidSymbols[oid];

            if (sym != null)
            {
                strBuffer.Append(sym);
            }
            else
            {
                strBuffer.Append(oid);
            }
            strBuffer.Append('=');
            int index = strBuffer.Length;
            strBuffer.Append(val);
            int end = strBuffer.Length;
            if (val.StartsWith("\\#"))
            {
                index += 2;
            }
            while (index != end)
            {
                if ((strBuffer[index] == ',')
                || (strBuffer[index] == '"')
                || (strBuffer[index] == '\\')
                || (strBuffer[index] == '+')
                || (strBuffer[index] == '=')
                || (strBuffer[index] == '<')
                || (strBuffer[index] == '>')
                || (strBuffer[index] == ';'))
                {
                    strBuffer.Insert(index++, "\\");
                    end++;
                }

                index++;
            }
        }
        /// <summary>
        /// Write the X509 Name in a readable format
        /// </summary>
        /// <returns>readable format</returns>
        public override string ToString()
        {
            List<object> Names = new List<object>();
            StringBuilder available = null;

            for (int i = 0; i < ordering.Count; i++)
            {
                if (added[i])
                {
                    available.Append('+');
                    AppendValue(available, DefaultSymbols,
                        ordering[i],
                        values[i]);
                }
                else
                {
                    available = new StringBuilder();
                    AppendValue(available, DefaultSymbols,
                        ordering[i],
                        values[i]);
                    Names.Add(available);
                }
            }

            StringBuilder buf = new StringBuilder();

            if (Names.Count > 0)
            {
                buf.Append(Names[0].ToString());

                for (int i = 1; i < Names.Count; ++i)
                {
                    buf.Append(',');
                    buf.Append(Names[i].ToString());
                }
            }

            return buf.ToString();

        }
    }
}
