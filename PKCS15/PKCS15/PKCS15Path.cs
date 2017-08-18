using smartcardLib.Core;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// The class decodes the following ASN.1 syntax:
    /// Path::= SEQUENCE {
    ///efidOrPath OCTET STRING,
    ///index INTEGER(0..cia-ub-index) OPTIONAL,
    ///length[0] INTEGER(0..cia-ub-index) OPTIONAL
    ///  }( WITH COMPONENTS {..., index PRESENT, length PRESENT }|
    ///   WITH COMPONENTS {..., index ABSENT, length ABSENT })
    /// </summary>
    public class PKCS15Path
    {
        /// <summary>
        /// AID of path
        /// </summary>
        public string aid { get; private set; }
        /// <summary>
        /// EF or Path
        /// </summary>
        public string efidOrPath;
        /// <summary>
        /// index ie offset
        /// </summary>
        /// <remarks>Optional will be null if not available</remarks>
        public int? index;
        /// <summary>
        /// length of the data object
        /// </summary>
        /// <remarks>Optional will be null if not available</remarks>
        public int? length;
        /// <summary>
        /// Constructs a PKCS15 Path
        /// </summary>
        /// <param name="tlv">Tag Length Value structure of PKCS15 Path object</param>
        public PKCS15Path(ASN1 tlv)
        {
            index = null;
            length = null;
            Debug.Assert(tlv.Children.Count > 0);
            var t = tlv.Children[0];
            //  Debug.Assert(t.Tag == ASN1.OCTET_STRING);
            t.Name = "efidOrPath";
            this.efidOrPath = "";
            for (var i = 0; i < t.length; i += 2)
            {
                this.efidOrPath += ":" + t.Value.Skip(i).Take(2).ToArray().ToHex();
            }
            if (tlv.Children.Count == 3)
            {
                t = tlv.Children[1];
                Debug.Assert(t.Tag == ASN1.INTEGER);
                Debug.Assert(t.length > 0);
                t.Name = "index";
                this.index = t.GetValue<int>();
                t = tlv.Children[2];
                Debug.Assert(t.Tag == 0x80);
                Debug.Assert(t.length > 0);
                t.Name = "length";
                this.length = t.GetValue<int>();
            }
        }
        /// <summary>
        /// overrides the to string object to provide a readable information of that instance
        /// </summary>
        /// <returns>Readable information of that instance</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("{");
            if (!string.IsNullOrEmpty(this.efidOrPath))str.Append($"efidOrPath={ this.efidOrPath}");            
            if (this.index != -1)str.Append($",index={ this.index}");            
            if (this.length != -1)str.Append($",length={ this.length}");            
            str.Append("}");
            return str.ToString();
        }
        /// <summary>
        /// Returns the absolute path without the MF file
        /// if there is an aid for the application it shall return aid# and then the path
        /// the path DF are separated by semicolons 
        /// </summary>
        /// <param name="df">If you have a df you want to pad to the path you should put it it here</param>
        /// <returns>If the path is absolute it will return the full path otherwise it will return the padded path with the df input</returns>
        public string getAbsolutePath(string df)
        {
            if (string.IsNullOrEmpty(this.aid))
            {
                return $"{ this.aid }#{ this.efidOrPath}";
            }
            if (df == null)
                df = "";

            var p = this.efidOrPath;
            if (p.Substring(0, 5) != ":3F00" && p.Substring(0, 5) != ":3f00")
            {
                p = $" {df}{ p}";
            }

            return p;
        }
    }
}