using smartcardLib.Card;
using smartcardLib.PKCS15;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var CardDirectory = new CardControllerEmulatorAsDirectory();
            CardDirectory.SelectAID("00FF888");
            var Pkcs15 = new PKCS15(CardDirectory);

            
        }
    }
}
