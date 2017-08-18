using Microsoft.VisualStudio.TestTools.UnitTesting;
using smartcardLib.PKCS15;
using smartcardLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcardLib.PKCS15.Tests
{
    [TestClass()]
    public class PKCS15PrivateKeyTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            PKCS15PrivateKey AcsRule = new PKCS15PrivateKey(new Core.ASN1("3040301E0C0950724B2E43482E4453020103300E300C03020520A20604010704010A300C040101030306004002020082A00430023000A10A30083002040002020400302F30140C0B50724B2E4943432E41555403020780040107300B0401020302052002020081A10A30083002040002020400".ToBytes()));
            Assert.AreNotEqual(AcsRule.ToString(),"");
        }
    }
}