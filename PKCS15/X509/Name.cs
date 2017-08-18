using smartcardLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcardLib.X509
{
    /// <summary>
    /// Name ::= CHOICE { -- only one possibility for now --rdnSequence  RDNSequence
    ///}
    ///RDNSequence::= SEQUENCE OF RelativeDistinguishedName

    ///RelativeDistinguishedName ::=
    ///  SET SIZE (1..MAX) OF AttributeTypeAndDistinguishedValue

    ///  AttributeTypeAndDistinguishedValue::= SEQUENCE {
    ///  type ATTRIBUTE.&id({ SupportedAttributes}),
    ///  value ATTRIBUTE.&Type({ SupportedAttributes}{@type}),
    ///  primaryDistinguished BOOLEAN DEFAULT TRUE,
    /// valuesWithContext
    ///    SET SIZE(1..MAX) OF
    ///     SEQUENCE
    ///{
    ///    distingAttrValue
    ///                 [0]
    ///    ATTRIBUTE.&Type({ SupportedAttributes}{ @type})
    ///                    OPTIONAL,
    ///                contextList SET SIZE (1..MAX) OF Context
    ///}
    ///OPTIONAL
    ///}
    /// </summary>
    class Name
    {
        List<X509Name> names;
        public Name(ASN1 tlv)
        {

            names = new List<X509Name>();
            if (tlv.Tag != ASN1.SEQUENCE)
            {
                return;
            }
            //We will only implment X509Name

            foreach (var t in tlv.Children)
            {
                names.Add(new X509Name(t));
            }

        }

        public override string ToString()
        {
            return $"{string.Join(",", names.Select(x => x.ToString()))}";
        }
    }
}
