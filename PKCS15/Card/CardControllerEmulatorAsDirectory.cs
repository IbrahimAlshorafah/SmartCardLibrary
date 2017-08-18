using smartcardLib.Card;
using smartcardLib.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcardLib.Card
{
    /// <summary>
    /// Card emulator controller as folde under c:\\VirtualCards\\ {AID}
    /// </summary>
    public class CardControllerEmulatorAsDirectory : ICardController
    {
        private string AllVirtualAIDDirectory = @"C:\VirtualCards\";
        /// <summary>
        /// Selects AID as directory under C:\VirtualCards
        /// </summary>
        /// <param name="aid">Directory Name</param>
        /// <returns>true if succeeded otherwise false</returns>
        public bool SelectAID(string aid)
        {
            try
            {
                Directory.SetCurrentDirectory($"{AllVirtualAIDDirectory}{aid}");
                return true;
            }
            catch(Exception ex)
            {
                LoggerFac.GetDefaultLogger().LogError(829, $"Couldn't set working directory to {AllVirtualAIDDirectory}{aid}", ex);
                return false;   
            }
        }
    }
}
