using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using smartcardLib.Core;
using smartcardLib.Logging;


namespace smartcardLib.PKCS15
{
    /// <summary>
    /// The class decodes the following ASN.1 syntax
    /// CIAInfo ::= SEQUENCE {
    /// version INTEGER { v1(0),v2(1)} (v1|v2,...),
    /// serialNumber OCTET STRING OPTIONAL,
    /// manufacturerID Label OPTIONAL,
    /// label[0] Label OPTIONAL,
    /// cardflags CardFlags,
    /// seInfo SEQUENCE OF SecurityEnvironmentInfo OPTIONAL,
    /// recordInfo[1] RecordInfo OPTIONAL,
    /// supportedAlgorithms[2] SEQUENCE OF AlgorithmInfo OPTIONAL,
    /// issuerId[3] Label OPTIONAL,
    /// holderId[4] Label OPTIONAL,
    /// lastUpdate[5] LastUpdate OPTIONAL,
    /// preferredLanguage PrintableString OPTIONAL, -- In accordance with IETF RFC 1766
    /// profileIndication[6] SEQUENCE OF ProfileIndication OPTIONAL,
    /// ...
    /// } (CONSTRAINED BY { -- Each AlgorithmInfo.reference value shall be unique --})
    /// </summary>
    public class PKCS15CIAInfo
    {

        #region Card Flags
        /// <summary>
        /// Read only card flag
        /// </summary>
        static public readonly int READ_ONLY = 0x80;
        /// <summary>
        /// Authentication required card flag
        /// </summary>
        static public readonly int AUTHENTICATION_REQUIRED = 0x40;
        /// <summary>
        /// Random number generation capable card flag
        /// </summary>
        static public readonly int PRNG_GENERATION = 0x20;
        #endregion
        /// <summary>
        /// ASN1 object of the CIA Info
        /// </summary>
        public ASN1 tlv { get; private set; }
        /// <summary>
        /// version
        /// </summary>
        public int? version { get; private set; }
        /// <summary>
        /// Serial number
        /// </summary>
        public byte[] serialNumber { get; private set; }
        /// <summary>
        /// Manufacturer ID
        /// </summary>
        public string manufacturerID { get; private set; }
        /// <summary>
        /// label
        /// </summary>
        public string label { get; private set; }
        /// <summary>
        /// Card flags
        /// </summary>
        public uint? cardflags { get; private set; }
        /// <summary>
        /// Secure Element Infromation
        /// </summary>
        public string seInfo { get; private set; }

        /// <summary>
        /// Record Information
        /// </summary>
        public string recordInfo { get; private set; }
        /// <summary>
        /// List of supported algorithms
        /// </summary>
        public List<AlgorithmInfo> supportedAlgorithms { get; private set; }

        /// <summary>
        /// issuer ID
        /// </summary>
        public string issuerId { get; private set; }

        /// <summary>
        /// Holder ID
        /// </summary>
        public string holderId { get; private set; }
        /// <summary>
        /// Last update
        /// </summary>
        public string lastUpdate { get; private set; }
        /// <summary>
        /// Preferred Language
        /// </summary>
        public string preferredLanguage { get; private set; }
        /// <summary>
        /// Profile Indication
        /// </summary>
        public string profileIndication;
        /// <summary>
        /// Logger
        /// </summary>
        public ILogger Logger { get; private set; }
        /// <summary>
        /// Constructor from ASN1
        /// </summary>
        /// <param name="tlv">ASN1 object containing the PKCS15 CIA</param>
        public PKCS15CIAInfo(ASN1 tlv)
        {
            this.Logger = LoggerFac.GetDefaultLogger();
            Debug.Assert(tlv.Tag== ASN1.SEQUENCE);
            Debug.Assert(tlv.elements > 2);
            this.tlv = tlv;
            var i = 0;
            ASN1 t;
            tlv.setName("CIAInfo");
            // version
            t = tlv.get(i);
            Debug.Assert(t.Tag== ASN1.INTEGER);
            t.setName("version");
            this.version = t.Value.ToSigned();
            i++;
            // serialNumber	
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.OCTET_STRING))
            {
                t.setName("serialNumber");
                this.serialNumber = t.Value;
                i++;
            }
            // manufacturerID
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== ASN1.UTF8String))
            {
                t.setName("manufacturerID");
                this.manufacturerID = t.GetValue<string>();
                i++;
            }
            // label
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== 0x80))
            {
                t.setName("label");
                this.label = ASCIIEncoding.UTF8.GetString( t.Value);
                i++;
            }
            if (i < tlv.elements)
            {
                t = tlv.get(i);
                Debug.Assert(t.Tag== ASN1.BIT_STRING);
                if (t.length > 1)
                {
                    this.cardflags = t.Value.Skip(1).Take(1).ToArray().ToUnsigned();
                    t.setName("cardflags {" + this.getCardflagsAsString() + " }");
                }
                else
                {
                    this.cardflags = 0;
                }
                i++;
            }
            // seInfo SEQUENCE OF SecurityEnvironmentInfo OPTIONAL,
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== ASN1.SEQUENCE))
            {
                t.setName("seInfo");
                Logger.LogDebug("CIAInfo.seInfo not further decoded : " + t);
                this.seInfo = "### not implemented ###";
                i++;
            }
            // recordInfo [1] RecordInfo OPTIONAL,
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== 0xA1))
            {
                t.setName("recordInfo");
                Logger.LogDebug("CIAInfo.recordInfo not further decoded : " + t);
                this.recordInfo = "### not implemented ###";
                i++;
            }
            // supportedAlgorithms [2] SEQUENCE OF AlgorithmInfo OPTIONAL,
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== 0xA2))
            {
                t.setName("supportedAlgorithms");
                this.supportedAlgorithms = new List<AlgorithmInfo>();
                for (var j = 0; j < t.elements; j++)
                {
                    var alg = new AlgorithmInfo(t.get(j));
                    this.supportedAlgorithms.Add(alg);
                }
                i++;
            }
            // issuerId [3] Label OPTIONAL,
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== 0xA3))
            {
                t.setName("issuerId");
                Logger.LogDebug("CIAInfo.issuerId not further decoded : " + t);
                this.issuerId = "### not implemented ###";
                i++;
            }
            // holderId [4] Label OPTIONAL,
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== 0xA4))
            {
                t.setName("holderId");
                Logger.LogDebug("CIAInfo.holderId not further decoded : " + t);
                this.holderId = "### not implemented ###";
                i++;
            }
            // lastUpdate [5] LastUpdate OPTIONAL,
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== 0xA5))
            {
                t.setName("lastUpdate");
                Logger.LogDebug("CIAInfo.lastUpdate not further decoded : " + t);
                this.lastUpdate = "### not implemented ###";
                i++;
            }
            // preferredLanguage PrintableString OPTIONAL, -- In accordance with IETF RFC 1766
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== ASN1.PrintableString))
            {
                t.setName("preferredLanguage");
                this.preferredLanguage = t.GetValue<string>();
                i++;
            }
            // profileIndication [6] SEQUENCE OF ProfileIndication OPTIONAL,
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag== 0xA6))
            {
                t.setName("profileIndicator");
                Logger.LogDebug("CIAInfo.profileIndicator not further decoded : " + t);
                this.profileIndication = "### not implemented ###";
                i++;
            }
        }
        /// <summary>
        /// Get card flag as readable string
        /// </summary>
        /// <returns>card flag as string</returns>
        private string getCardflagsAsString()
        {
            return $@"{(((this.cardflags & 0x80)!=0 )? " readonly" : "") }
           {(((this.cardflags & 0x40)!=0) ? " authRequired" : "") }
           {(((this.cardflags & 0x20)!=0) ? " prnGeneration" : "")}";
        }
        /// <summary>
        /// Converts the CIA to string
        /// </summary>
        /// <returns>a readable string of the PKCS15 CIA</returns>
        public override string ToString()
        {
            return $@"CIAInfo {{
            version={this.version }\n
            {((this.serialNumber != null)?$"serialNumber={this.serialNumber.ToHex() }\n":"")}
            {((this.manufacturerID != null)?$"manufacturerID={ this.manufacturerID},\n":"")}         
            {((this.label != null)?$"label={ this.label },\n":"")}            
            cardflags={this.getCardflagsAsString() },\n
            {((this.supportedAlgorithms != null)? $"supportedAlgorithms={{\n{string.Join("\n", this.supportedAlgorithms)} }}\n":"")}                
            {((this.preferredLanguage != null)?$"preferredLanguage={this.preferredLanguage},\n":"")}
            }}";
            
        }
    }
}