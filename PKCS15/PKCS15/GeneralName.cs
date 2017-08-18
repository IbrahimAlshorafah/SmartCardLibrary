using smartcardLib.Core;
using smartcardLib.X509;
using System;

namespace smartcardLib.PKCS15
{
    /// <summary>
    ///  
    /// The GeneralName object.
    /// <pre>
    /// GeneralName ::= CHOICE {
    /// otherName[0]     OtherName,
    /// rfc822Name[1]     IA5String,
    /// dNSName[2]     IA5String,
    /// x400Address[3]     ORAddress,
    /// directoryName[4]     Name,
    /// ediPartyName[5]     EDIPartyName,
    /// uniformResourceIdentifier[6]     IA5String,
    /// iPAddress[7]     OCTET STRING,
    /// registeredID[8]     OBJECT IDENTIFIER}
    /// 
    /// OtherName ::= Sequence {
    /// type - id    OBJECT IDENTIFIER,
    /// value[0] EXPLICIT ANY DEFINED BY type - id }
    /// 
    /// EDIPartyName ::= Sequence {
    /// nameAssigner[0]     DirectoryString OPTIONAL,
    /// partyName[1]     DirectoryString }
    /// </pre>    
    /// </summary>
    public class GeneralName
    {
        /// <summary>
        /// OtherName CHOICE
        /// </summary>
        public const int OtherName = 0;
        /// <summary>
        /// Rfc822Name CHOICE
        /// </summary>
        public const int Rfc822Name = 1;
        /// <summary>
        /// DNS name CHOICE
        /// </summary>
        public const int DnsName = 2;
        /// <summary>
        /// X400Address CHOICE
        /// </summary>
        /// <remarks> NOT supported</remarks>
        public const int X400Address = 3;
        /// <summary>
        /// Directory Name CHOICE
        /// </summary>
        public const int DirectoryName = 4;
        /// <summary>
        /// EDI Party Name CHOICE
        /// </summary>
        public const int EdiPartyName = 5;
        /// <summary>
        /// Uniform Resource Identifier CHOICE
        /// </summary>
        public const int UniformResourceIdentifier = 6;
        /// <summary>
        /// IP address CHOICE
        /// </summary>
        public const int IPAddress = 7;
        /// <summary>
        /// Registered Id CHOICE
        /// </summary>
        public const int RegisteredID = 8;
        /// <summary>
        /// Name as string
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlv">ASN1 object that holds General Name</param>
        public GeneralName(ASN1 tlv )
        {
            switch(tlv.TagNumber)
            {
                case OtherName:
                case EdiPartyName:
                    Name = new GeneralName(tlv.get(0)).ToString();                    
                    break;
                case IPAddress:
                case Rfc822Name:
                case DnsName:
                case UniformResourceIdentifier:
                case RegisteredID:
                    Name = tlv.get(0).GetValue<string>();
                    break;
                
                case X400Address:
                    throw new NotSupportedException();
                case DirectoryName:
                    //TODO: make sure it is explicit
                    Name = new X509Name(tlv).ToString();
                    break;       
            }

        }
        /// <summary>
        /// Converts to readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
