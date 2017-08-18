using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using smartcardLib.Core;
using smartcardLib.Card;
using smartcardLib.Logging;
namespace smartcardLib.PKCS15
{
    /// <summary>
    /// Shall encode PKCS 15 DDO ASN1 object:
    /// 
    /// DDO ::= SEQUENCE {
    /// oid OBJECT IDENTIFIER,
    /// odfPath Path OPTIONAL,
    /// tokenInfoPath[0] Path OPTIONAL,
    /// unusedPath[1] Path OPTIONAL,
    /// ... -- For future extensions
    /// }
    /// </summary>
    public class PKCS15
    {
        private ICardFile ef_dir;
        private ILogger logger;
        private string df;
        /// <summary>
        /// Constructor that should use a card controller
        /// </summary>
        /// <param name="card">Card Controller (Applet Controller)</param>
        public PKCS15(ICardController card)
        {
            this.cardController = card;
            this.logger = LoggerFac.GetDefaultLogger();
        }

        /// <summary>
        /// Read Application directory [DIR]
        /// if DIR application contains more than on AID information then this call will return Directory of AID and Application Templates
        /// </summary>
        /// <returns>Directory of AID and Application Templates</returns>
        public Dictionary<string, PKCS15ApplicationTemplate> readApplicationDirectory()
        {
            this.ef_dir = new MockedCardFile(this.cardController, ":3F00:2F00");
            this.aidlist = new Dictionary<string,PKCS15ApplicationTemplate>();
            if (this.ef_dir.IsTransparent())
            {
                var data = this.ef_dir.ReadBinary();
                logger.LogTrace(data.ToHex());
                if (data == null)
                {
                    data = this.ef_dir.ReadBinary((uint)this.ef_dir.Length);
                }
                
                if (data != null && (data.Length > 0) && (data[0] == 0x61))
                {
                    logger.LogTrace($"Directory data is {data.ToHex()}");
                    int offset = 0;
                    while (data.Length > offset)
                    {

                    
                        var tlv = new ASN1(data, offset);
                        offset += tlv.TotalLength;

                        var at = new PKCS15ApplicationTemplate(tlv);
                        this.aidlist[at.aid] = at;
                        while (offset < data.Length && data[offset] != 0x61)
                        {
                            offset++;
                        }
                    }
                }

            }
            else
            {
                var rec = 1;
                while (rec < 256)
                {
                    byte[] data;
                    try
                    {
                        data = this.ef_dir.ReadRecord(rec);
                    }
                    catch(SmartCardException e)
                    {
                        logger.LogError(930, "Resading Record Exception", e);
                        throw;
                    }
                    catch 
                    {
                        break;
                    }

                    var tlv = new ASN1(data);
                    var at = new PKCS15ApplicationTemplate(tlv);
                    this.aidlist[at.aid] = at;
                    rec++;
                }
            }
            return this.aidlist;
        }
        /// <summary>
        /// Read a card object at a specific location
        /// </summary>
        /// <param name="df">Directory File</param>
        /// <param name="path">path under the directory file if the path is absolute then df can be null</param>
        /// <returns>Data object</returns>
        public byte[] readCardObject(string df, PKCS15Path path)
        {
            var p = path.getAbsolutePath(df);
            logger.LogError($"Reading from: {p}");
            var ef = new MockedCardFile(this.cardController, p);
            byte[] data;
            if (path.index != null)
            {
                data = ef.ReadBinary((uint?)path.index,(uint?)  path.length);
            }
            else
            {
                var len = ef.Length;
          
                data = ef.ReadBinary(0, len);
            }
            return data;
        }

        /// <summary>
        /// Read multiple file objects (TLV objects after each other) 
        /// </summary>
        /// <param name="df">Directory File</param>
        /// <param name="path">path under the directory file if the path is absolute then df can be null</param>
        /// <returns>List of the data objects in the file</returns>
        public List<ASN1> readCardObjects(string df, PKCS15Path path)
        {
            var p = path.getAbsolutePath(df);
            logger.LogTrace($"Reading from: { p}");
            var ef = new MockedCardFile(this.cardController, p);
            var list = new List<ASN1>();
            switch (p)
            {
                case "":
                    {
                    }
                    break;
            }

            if (ef.IsTransparent())
            {
                byte[] data;
                //if (path.index != null)
                //{
                    
                        data = ef.ReadBinary((uint?)path.index,(uint?) path.length);
                    
                //}
                //else
                //{
                //    var len0 = ef.Length;
                //    data = ef.ReadBinary(0, len0);
                //}


                var len = data.Length;

                try
                {
                    while (len > 0)
                    {
                        if ((data[0] == 0x00) || (data[0] == 0xFF))
                        {
                            len--;
                            data = data.Skip(1).ToArray();
                        }
                        else
                        {
                           // logger.LogTrace($"Reading data {data.ToHex()}");
                            var tlv = new ASN1(data);
                            var tlvsize = tlv.TotalLength;
                            len -= tlvsize;
                            data = data.Skip(tlvsize).ToArray();
                            list.Add(tlv);
                           // logger.LogTrace($"readCardObjects: {tlv}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(234,"Error reading cryptographic information object: " ,ex);
                    logger.LogError(data.ToHex());
                }
            }
            else
            {
                var rec = 1;
                while (rec < 256)
                {
                    byte[] data;
                    try
                    {
                        data = ef.ReadRecord(rec);
                    }
                    catch ( SmartCardException ex)
                    {
                        logger.LogError(930, "Reading Record Exception", ex);
                        throw;
                    }
                    catch
                    {
  
                        break;
                    
   
                    }

                    data = data.Skip(2).ToArray(); // Strip of first two bytes
                    if (data.Length > 2)
                    {   // Record might be empty
                        try
                        {
                            var tlv = new ASN1(data);
                            list.Add(tlv);
              
                        }
                        catch (Exception e)
                        {
                            logger.LogError(234, "Error reading cryptographic information object: ", e);
                            logger.LogError(data.ToHex());
                        }
                    }
                    rec++;
                }
            }
            return list;
        }
        /// <summary>
        /// Parses List of objects (ASN1)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>List of all ASN1 objects found</returns>
        public List<ASN1> parseObjectList(byte[] data)
        {
            var list = new List<ASN1>();
            var len = data.Length;

            while (len > 0)
            {
                if ((data[0] == 0x00) || (data[0] == 0xFF))
                {
                    len--;
                    data = data.Skip(1).ToArray();
                }
                else
                {
                    
                    var tlv = new ASN1(data);
                    var tlvsize = tlv.TotalLength;
                    len -= tlvsize;
                    data = data.Skip(tlvsize).ToArray();
                    list.Add(tlv);
                   
                }
            }
            return list;
        }

        /// <summary>
        /// Read Object object in an Application template 
        /// Make sure that the DDO is filled since it is Optional and not all card implmentation fills that field
        /// you should fill it manually if this is the case
        /// </summary>
        /// <param name="at">Application template to be read</param>
        /// <remarks>
        /// The output of this call is on the logger [Default Logger]
        /// </remarks>
        public void readObjectListForApplication(PKCS15ApplicationTemplate at)
        {
            if (at.ddo == null)
            {
                throw new PKC15Exception("Application has no PKCS#15 information");
            }

            if (at.ddo.odfPath == null)
            {
                throw new PKC15Exception("Application has no odfPath");
            }

            at.objlist = new List<PKCS15CommonObjectAttributes>();
            if (!string.IsNullOrEmpty(at.aid))
            {
                bool res = this.cardController.SelectAID(at.aid);
            }
            var dos = this.readCardObjects(":3F00", at.ddo.odfPath);

            // Determine current DF
            var df = at.ddo.odfPath.getAbsolutePath(":3F00");
            df = df.Substring(0, df.Length-5);
            this.df = df;

            for (var i = 0; i < dos.Count; i++)
            {
                var tlv = dos[i];
                Debug.Assert(tlv.IsConstructed);
                var ciotype = tlv.Tag;
                var path = new PKCS15Path(tlv.get(0));
                //Unusual errors
                string upperPath = at.ddo.odfPath.efidOrPath;
                upperPath = upperPath.Substring(0, upperPath.Length - 5);
                if (!path.efidOrPath.StartsWith(upperPath))
                {
                    path.efidOrPath = $"{upperPath}{path.efidOrPath.Substring(upperPath.Length)}";
                }
                try
                {
                    
                    var cios = this.readCardObjects(df, path);
                    var dontPush = false;
                    for (var j = 0; j < cios.Count; j++)
                    {
                        tlv = cios[j];

                        PKCS15CommonObjectAttributes cio = null;
                        switch (ciotype)
                        {
                            case 0xA0:
                                cio = new PKCS15PrivateKey(tlv);
                                //					cio.type = "PrivateKey";
                                logger.LogInformation( $" Private Key :{ cio.ToString()}");
                                break;
                            case 0xA1:
                                cio = new PKCS15PublicKey(tlv);
                                //cio = new PKCS15CommonObjectAttributes(tlv);
                                cio.type = "PublicKey";
                                logger.LogInformation($" Public Key :{ cio.ToString()}");
                                break;
                            case 0xA2:
                                //					cio = new PKCS15PublicKey(tlv);
                                cio = new PKCS15CommonObjectAttributes(tlv);
                                cio.type = "TrustedPublicKey";
                                logger.LogInformation($" Trusted Public Key :{ cio.ToString()}");
                                break;
                            case 0xA3:
                                //					cio = new PKCS15_SecretKey(tlv);
                                cio = new PKCS15CommonObjectAttributes(tlv);
                                cio.type = "SecretKey";
                                logger.LogInformation($" Secret  Key :{ cio.ToString()}");
                                break;
                            case 0xA4:
                                cio = new PKCS15Certificate(tlv);
                                cio.type = "Card Certificate";
                                logger.LogInformation($" Card Certificate :{ cio.ToString()}");
                                break;
                            case 0xA5:
                                cio = new PKCS15Certificate(tlv);
                                cio.type = "Trusted" + cio.type;
                                logger.LogInformation($" Trusted Certificate :{ cio.ToString()}");
                                break;
                            case 0xA6:
                                cio = new PKCS15Certificate(tlv);
                                cio.type = "Useful" + cio.type;
                                logger.LogInformation($" Useful certificate :{ cio.ToString()}");
                                break;
                            case 0xA7:
                                cio = new PKCS15DataContainerObject(tlv);
                                cio.type = "DataContainerObject";
                                logger.LogInformation($"Data Container Object :{ cio.ToString()}");
                                break;
                            case 0xA8:
                                try
                                {
                                    cio = new PKCS15AuthenticationObject(tlv);
                                    cio.type = "AuthObject";
                                    logger.LogInformation($" Authentication object :{ cio.ToString()}");
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(4746, "Reading Authentication Object throw exception", ex);
                                    dontPush = true;
                                }
                                break;
                            default:
                                Debug.Assert(false);
                                break;
                        }
                        if (!dontPush || cio != null)
                        {
                            at.objlist.Add(cio);
                        }
                        dontPush = false;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(989, "Read Object List Error ", ex);
                }
            }

        }
        /// <summary>
        /// Card Controller current used
        /// </summary>
        public ICardController cardController { get; private set; }
        /// <summary>
        /// AID list read from the DIR EF
        /// </summary>
        public Dictionary<string, PKCS15ApplicationTemplate> aidlist { get; private set; }
    }
}