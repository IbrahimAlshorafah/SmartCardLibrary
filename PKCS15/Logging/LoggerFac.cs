using System;
using System.Collections.Generic;
namespace smartcardLib.Logging
{
    /// <summary>
    /// Logger Factory
    /// </summary>
    public class LoggerFac
    {
        /// <summary>
        /// A dictionary with all the available loggers
        /// </summary>
        static Dictionary<string, ILogger> Loggers = new Dictionary<string, ILogger>();
        /// <summary>
        /// static function to add loggers to the dictionary
        /// </summary>
        /// <param name="t">Type of the logger</param>
        /// <param name="Value">instance of the logger</param>
        /// <returns>true if added successfully </returns>
        /// <remarks> There can't be two instance of loggers with the same type</remarks>
        static public bool AddLogger(Type t, ILogger Value)
        {
            if (Loggers == null)
                Loggers = new Dictionary<string, ILogger>();
            if (Loggers.ContainsKey(t.FullName))
                return false;
            Loggers.Add(t.FullName, Value);
            return true;
        }
        /// <summary>
        /// Get a specific logger depending on the type
        /// </summary>
        /// <typeparam name="T">The type of the logger to retrieve</typeparam>
        /// <returns>Logger as a type it must implements the <see cref="ILogger"/> of no logger of that type is added to the factory the output will be null   </returns>
        static public T GetLogger<T>() where T : class
        {
            if(Loggers.ContainsKey(typeof(T).FullName))
            {
                return (T)Loggers[typeof(T).FullName];
            }
            throw new ArgumentException("Type not found");
        }
        /// <summary>
        /// Gets the default logger currently the default logger is  <see cref="ConsoleLogger"/>
        /// </summary>
        /// <returns>Default logger <see cref="ConsoleLogger"/></returns>
        static public ILogger GetDefaultLogger()
        {
            try
            {
                return (ILogger)GetLogger<ConsoleLogger>();
            }
            catch
            {

                AddLogger(typeof(ConsoleLogger), new ConsoleLogger());
                return (ILogger)GetLogger<ConsoleLogger>();
            }
        }
    }
}