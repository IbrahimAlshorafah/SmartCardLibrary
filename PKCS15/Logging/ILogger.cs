using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smartcardLib.Logging
{
    /// <summary>
    /// Error levels available
    /// </summary>
    public enum ErrorLevel
    {
        //Critical, Error, Warning, Information , Debug,  Trace
        /// <summary>
        /// Trace logging
        /// </summary>
        Trace,
        /// <summary>
        /// Debug logging
        /// </summary>
        Debug,
        /// <summary>
        /// Information logging
        /// </summary>
        Information,
        /// <summary>
        /// Warning logging
        /// </summary>
        Warning,
        /// <summary>
        /// Error logging
        /// </summary>
        Error,
        /// <summary>
        /// Critical Logging
        /// </summary>
        Critical
    };
    /// <summary>
    /// Logger Interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log debug 
        /// </summary>
        /// <param name="ErrorCode">a number that identify a debug information</param>
        /// <param name="Error">debug information</param>
        /// <param name="ex">if it is associated with exception you can call this overload</param>
        void LogDebug(int ErrorCode, string Error, Exception ex);
        /// <summary>
        /// Log debug 
        /// </summary>
        /// <param name="ErrorCode">a number that identify a debug information</param>
        /// <param name="Error">debug information</param>        
        void LogDebug(int ErrorCode, string Error);
        /// <summary>
        /// Log debug 
        /// </summary>        
        /// <param name="Error">debug information</param>        
        void LogDebug(string Error);
        /// <summary>
        /// Log warning 
        /// </summary>
        /// <param name="ErrorCode">a number that identify the warning</param>
        /// <param name="Error">warning information</param>
        /// <param name="ex">if warning is associated with exception you can call this overload</param>
        void LogWarning(int ErrorCode, string Error, Exception ex);
        /// <summary>
        /// Log warning 
        /// </summary>
        /// <param name="ErrorCode">a number that identify the warning</param>
        /// <param name="Error">warning details</param>
        void LogWarning(int ErrorCode, string Error);

        /// <summary>
        /// Log warning
        /// </summary>
        /// <param name="Error">warning information</param>
        void LogWarning(string Error);
        /// <summary>
        /// Log error 
        /// </summary>
        /// <param name="ErrorCode">a number that identify a error information</param>
        /// <param name="Error">error information</param>
        /// <param name="ex">if it is associated with exception you can call this overload</param>
        
        void LogError(int ErrorCode, string Error, Exception ex);
        /// <summary>
        /// Log error 
        /// </summary>        
        /// <param name="ErrorCode">a number that identify a error information</param>
        /// <param name="Error">error information</param>        
        void LogError(int ErrorCode, string Error);
        /// <summary>
        /// Log error 
        /// </summary>
        /// <param name="Error">error information</param>
        void LogError(string Error);
        /// <summary>
        /// Log critical error
        /// </summary>
        /// <param name="ErrorCode">a number that identify the critical error</param>
        /// <param name="Error">critical error information</param>
        /// <param name="ex">if error is associated with exception you can call this overload</param>
        void LogCritical(int ErrorCode, string Error, Exception ex);
        /// <summary>
        /// Log critical error
        /// </summary>
        /// <param name="ErrorCode">a number that identify the critical error</param>
        /// <param name="Error">critical error information</param>
        void LogCritical(int ErrorCode, string Error);
        /// <summary>
        /// Log critical error
        /// </summary>
        /// <param name="Error">critical error information</param>

        void LogCritical(string Error);
        /// <summary>
        /// Log for trace
        /// </summary>
        /// <param name="ErrorCode">a number that identify the trace information</param>
        /// <param name="Error">trace information</param>
        /// <param name="ex">if the trace is associated with exception you can call this overload</param>
        void LogTrace(int ErrorCode, string Error, Exception ex);
        /// <summary>
        /// Log for trace
        /// </summary>
        /// <param name="ErrorCode">a number that identify the trace information</param>
        /// <param name="Error">trace information</param>
        void LogTrace(int ErrorCode, string Error);
        /// <summary>
        /// Log for trace
        /// </summary>
        /// <param name="Error">trace information</param>
        void LogTrace(string Error);
        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="ErrorCode">a number that identify the information details</param>
        /// <param name="Error">Information details</param>
        /// <param name="ex">if the information is associated with exception you can call this overload</param>
        void LogInformation(int ErrorCode, string Error, Exception ex);
        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="ErrorCode">a number that identify the information details</param>
        /// <param name="Error">Information details</param>
        void LogInformation(int ErrorCode, string Error);
        /// <summary>
        /// Log information
        /// </summary>       
        /// <param name="Error">Information details</param>
        void LogInformation(string Error);
        /// <summary>
        /// returns the current log level configured
        /// </summary>
        ErrorLevel CurrentErrorLevel { get; set; }
    }
}