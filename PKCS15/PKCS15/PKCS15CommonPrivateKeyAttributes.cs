using smartcardLib.Core;
using smartcardLib.Logging;
using smartcardLib.X509;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// CommonPrivateKeyAttributes ::= SEQUENCE {
    /// subjectName Name OPTIONAL,
    /// keyIdentifiers[0] SEQUENCE OF CredentialIdentifier {{ KeyIdentifiers} }
    /// OPTIONAL,
    /// ... -- For future extensions
    /// }
    /// </summary>
    public class PKCS15CommonPrivateKeyAttributes : PKCS15CommonKeyAttributes
    {
        private ILogger logger;
        Name SubjectName;
        List<PKCS15CredentialIdentifier> Ident;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tlv">ASN1 contains a common private key attributes</param>
        public PKCS15CommonPrivateKeyAttributes(ASN1 tlv) : base(tlv)
        {
            this.logger = LoggerFac.GetDefaultLogger();
            Ident = new List<PKCS15CredentialIdentifier>();
            var t = tlv.get(2);
            if (t.Tag == 0xA0)
            {
                t.setName("subClassAttributes");

                Debug.Assert(t.elements == 1);
                t = t.get(0);
                t.setName("commonPrivateKeyAttributes");

                var i = 0;
                if (t.elements > i)
                {
                    switch (t.Tag)
                    {
                        case 0x30: //SubjectName
                            {
                                t.setName("subjectName");
                                SubjectName = new Name(t);

                            }
                            break;
                        case 0xA0:
                            {

                                foreach (var ch in t.Children)
                                {
                                    Ident.Add(new PKCS15CredentialIdentifier(ch));
                                }

                            }
                            break;

                        default:

                            logger.LogInformation($"Unknown tag value in commonPrivateKeyAttributes{t.Tag}");
                            break;
                    }
                }
            }
        }



        /// <summary>
        /// Overriding to string
        /// </summary>
        /// <returns></returns>

        public override string ToString()
        {
            if (SubjectName == null && Ident.Count == 0)
                return base.ToString();
            else if (Ident.Count == 0)
            {
                return $@"{base.ToString()},
                        subClassAttributes{{
                        subjectName rdnSequence : {{ {SubjectName.ToString()}
                        }}
                     }},";
            }
            else if (SubjectName == null)
            {
                return $@"{base.ToString()},
                        keyIdentifiers  {{ {string.Join(",\n", Ident.Select(x => x.ToString()))}
                        }}
                     }},";
            }
            else
            {
                return $@"{base.ToString()},
                        subjectName rdnSequence : {{ {SubjectName.ToString()}
                        keyIdentifiers  {{ {string.Join(",\n", Ident.Select(x => x.ToString()))}
                        }}
                     }},";
            }
        }



    }

}