using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Tripper.Interfaces.Services;

namespace Tripper.Services;

/// <summary>
///     Service to deal with the local log-file
///     Local log file only shows the current and last day
///     May be extended for external logging in the future
/// </summary>
public class LoggingService : ILoggingService
{
    private readonly string logFileNameAndPath;
    private bool logNeedsCleanup;

    public LoggingService()
    {
        var appFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        logFileNameAndPath = Path.Combine(appFolder, Constants.logFileName);

        InitLog();
    }

    /// <summary>
    ///     Logs to local file in AppSpecific files
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Log(string message)
    {
        StackFrame frame = new StackFrame(1, true);
        var method = frame.GetMethod();
        var callerFileName = frame.GetFileName()?.Split('\\').Last().Split('.')[0];
        var lineNumber = frame.GetFileLineNumber();

        try
        {
            using (var streamWriter = new StreamWriter(logFileNameAndPath, true))
            {
                streamWriter.WriteLine($"[{DateTime.UtcNow.ToLocalTime():HH:mm:ss}] [{callerFileName}.{method}/{lineNumber}]: {message}");
            }
        }
        catch 
        { 
            // Oh NO... anyways
        }
    }
    
    /// <summary>
    ///     Returns local log-file contents
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string GetLog()
    {
        using (var streamReader = new StreamReader(logFileNameAndPath))
        {
            return streamReader.ReadToEnd();
        }
    }

    /// <summary>
    ///     Deletes and creates clean new log-file
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void ClearLog()
    {
        logNeedsCleanup = true;
        InitLog();
    }

    #region private

    private void InitLog()
    {
        var todayDateString = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        if (!File.Exists(logFileNameAndPath) || LogNeedsCleanup(todayDateString) || logNeedsCleanup)
        {
            var fs = File.Create(logFileNameAndPath);
            fs.Dispose();

            Log($"---- Log Created: {todayDateString}");

            logNeedsCleanup = false;
            return;
        }

        Log($"---- Reinitialized: {todayDateString}");
    }

    public bool LogNeedsCleanup(string today)
    {
        try
        {
            var logCreationDate = GetLogCreationDate();

            return logCreationDate != today;
        }
        catch
        {
            return false;
        }
    }

    public string GetLogCreationDate()
    {
        var firstLineOfLog = GetLog().Split("\n")[0];

        // extract date from log and return trimmed
        return firstLineOfLog.Split(']')[1].Split(':')[2].Trim();
    }

    #endregion
}
