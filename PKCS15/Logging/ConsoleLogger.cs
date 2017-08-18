
using System;
using System.Threading;
namespace smartcardLib.Logging
{
    /// <summary>
    /// Logs to console
    /// </summary>
    public class ConsoleLogger : ILogger
    { 
        string Text;
        /// <summary>
        /// Constructor
        /// </summary>
        public ConsoleLogger()
        {
            Text = "";
        }
        /// <summary>
        /// Clears the text
        /// </summary>
        public void Clear()
        {
            Text = "";
        }
        /// <summary>
        /// Logs string
        /// </summary>
        /// <param name="str">String to log</param>
        /// <param name="level">logging level</param>
        /// <param name="Code">Logging code</param>
        /// <param name="ex">Exception if any</param>
        private void Log(string str,  ErrorLevel level, int Code = -1, Exception ex = null )
        {
            if (level < CurrentErrorLevel) return;
            try
            {
                Text = $"{DateTime.Now.ToString()} {level.ToString()} : {(Code == -1 ? "" : $"({Code}) ")}{str}{(ex == null ? "" : $" (Exception Message({ex.Message}), Trace({ex.StackTrace}) ) ")} ";
                ThreadPool.QueueUserWorkItem(o=>
                {
                    try
                    {
                        try
                        {
                            if (Text == null)
                                Text = "";
                            Text += $"{Text}\n";

                        }catch { }
                        Console.WriteLine(Text);
                    }catch { }
                }
                );
            }catch { }
        }
        /// <summary>
        /// Current logging level
        /// </summary>
        public ErrorLevel CurrentErrorLevel
        {
            get;
            set;
            
        }
        /// <summary>
        /// Log critical error
        /// </summary>
        /// <param name="Error">critical error information</param>
        public void LogCritical(string Error)
        {
            Log(Error, ErrorLevel.Critical);
        }
        /// <summary>
        /// Log critical error
        /// </summary>
        /// <param name="ErrorCode">a number that identify the critical error</param>
        /// <param name="Error">critical error information</param>
        public void LogCritical(int ErrorCode, string Error)
        {
            Log(Error, ErrorLevel.Critical, ErrorCode);
        }
        /// <summary>
        /// Log for trace
        /// </summary>
        /// <param name="ErrorCode">a number that identify the trace information</param>
        /// <param name="Error">trace information</param>
        /// <param name="ex">if the trace is associated with exception you can call this overload</param>
        public void LogCritical(int ErrorCode, string Error, Exception ex)
        {
            Log(Error, ErrorLevel.Critical, ErrorCode, ex);
        }
        /// <summary>
        /// Log debug 
        /// </summary>        
        /// <param name="Error">debug information</param>     
        public void LogDebug(string Error)
        {
            Log(Error, ErrorLevel.Debug);
        }
        /// <summary>
        /// Log debug 
        /// </summary>
        /// <param name="ErrorCode">a number that identify a debug information</param>
        /// <param name="Error">debug information</param>    
        public void LogDebug(int ErrorCode, string Error)
        {
            Log(Error, ErrorLevel.Debug, ErrorCode);
        }
        /// <summary>
        /// Log debug 
        /// </summary>
        /// <param name="ErrorCode">a number that identify a debug information</param>
        /// <param name="Error">debug information</param>
        /// <param name="ex">if it is associated with exception you can call this overload</param>
        public void LogDebug(int ErrorCode, string Error, Exception ex)
        {
            Log(Error, ErrorLevel.Debug, ErrorCode, ex);
        }
        /// <summary>
        /// Log error 
        /// </summary>
        /// <param name="Error">error information</param>
        public void LogError(string Error)
        {
            Log(Error, ErrorLevel.Error);
        }
        /// <summary>
        /// Log error 
        /// </summary>        
        /// <param name="ErrorCode">a number that identify a error information</param>
        /// <param name="Error">error information</param>        
        public void LogError(int ErrorCode, string Error)
        {
            Log(Error, ErrorLevel.Error, ErrorCode);
        }
        /// <summary>
        /// Log error 
        /// </summary>
        /// <param name="ErrorCode">a number that identify a error information</param>
        /// <param name="Error">error information</param>
        /// <param name="ex">if it is associated with exception you can call this overload</param>
        public void LogError(int ErrorCode, string Error, Exception ex)
        {
            Log(Error, ErrorLevel.Error, ErrorCode, ex);
        }
        /// <summary>
        /// Log information
        /// </summary>       
        /// <param name="Error">Information details</param>
        public void LogInformation(string Error)
        {
            Log(Error, ErrorLevel.Information);
        }
        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="ErrorCode">a number that identify the information details</param>
        /// <param name="Error">Information details</param>
        public void LogInformation(int ErrorCode, string Error)
        {
            Log(Error, ErrorLevel.Information, ErrorCode);
        }
        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="ErrorCode">a number that identify the information details</param>
        /// <param name="Error">Information details</param>
        /// <param name="ex">if the information is associated with exception you can call this overload</param>
        public void LogInformation(int ErrorCode, string Error, Exception ex)
        {
            Log(Error, ErrorLevel.Information, ErrorCode, ex);
        }
        /// <summary>
        /// Log for trace
        /// </summary>
        /// <param name="Error">trace information</param>
        public void LogTrace(string Error)
        {
            Log(Error, ErrorLevel.Trace);
        }
        /// <summary>
        /// Log for trace
        /// </summary>
        /// <param name="ErrorCode">a number that identify the trace information</param>
        /// <param name="Error">trace information</param>
        public void LogTrace(int ErrorCode, string Error)
        {
            Log(Error, ErrorLevel.Trace, ErrorCode);
        }
        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="ErrorCode">a number that identify the information details</param>
        /// <param name="Error">Information details</param>
        /// <param name="ex">if the information is associated with exception you can call this overload</param>
        public void LogTrace(int ErrorCode, string Error, Exception ex)
        {
            Log(Error, ErrorLevel.Trace, ErrorCode, ex);
        }
        /// <summary>
        /// Log warning
        /// </summary>
        /// <param name="Error">warning information</param>
        public void LogWarning(string Error)
        {
            Log(Error, ErrorLevel.Warning);
        }
        /// <summary>
        /// Log warning 
        /// </summary>
        /// <param name="ErrorCode">a number that identify the warning</param>
        /// <param name="Error">warning details</param>
        public void LogWarning(int ErrorCode, string Error)
        {
            Log(Error, ErrorLevel.Warning, ErrorCode);
        }
        /// <summary>
        /// Log warning 
        /// </summary>
        /// <param name="ErrorCode">a number that identify the warning</param>
        /// <param name="Error">warning information</param>
        /// <param name="ex">if warning is associated with exception you can call this overload</param>
        public void LogWarning(int ErrorCode, string Error, Exception ex)
        {
            Log(Error, ErrorLevel.Warning, ErrorCode, ex);
        }
    }
}