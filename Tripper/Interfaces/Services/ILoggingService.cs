using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tripper.Interfaces.Services;

public interface ILoggingService
{
    /// <summary>
    ///     <para>Logs to local file in AppSpecific files, adds time, and minimal stack trace to message already</para>
    ///     <para></para>
    ///     <para>Format:  [{DateTime.UtcNow.ToLocalTime():HH:mm:ss}] [{className}.{method}/{lineNumber}]: {message}</para>
    /// </summary>
    /// <param name="message"></param>
    void Log(string message, [CallerMemberName] string method = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0);
    /// <summary>
    ///     <para>Returns current contents of the local log file</para>
    /// </summary>
    /// <returns></returns>
    string GetLog();
    void ClearLog();
}
