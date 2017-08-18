
namespace smartcardLib.Card
{
    /// <summary>
    /// Card controller interface
    /// </summary>
    public interface ICardController
    {
        /// <summary>
        /// Select Application Identifier
        /// </summary>
        /// <param name="aid">Application Identifier</param>
        /// <returns>value</returns>
        bool SelectAID(string aid);
    }
}