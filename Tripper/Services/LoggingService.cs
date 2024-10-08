﻿using System.Diagnostics;
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

        Task.Run(InitLog);
    }

    public void Log(string message, [CallerMemberName] string method = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        var callerFileName = filePath.Split('\\').Last().Split('.')[0];

        try
        {
            using (var streamWriter = new StreamWriter(logFileNameAndPath, true))
            {
                streamWriter.WriteLine($"[{DateTime.UtcNow.ToLocalTime():HH:mm:ss}] [{callerFileName}.{method}/{lineNumber}]: {message}");
            }
        }
        catch 
        {
            // Oh NO... anyways https://www.youtube.com/watch?v=lv-mD3qtalw
        }
    }
    
    public async Task<string> GetLog()
    {
        using (var streamReader = new StreamReader(logFileNameAndPath))
        {
            return await streamReader.ReadToEndAsync();
        }
    }

    public async Task ClearLog()
    {
        logNeedsCleanup = true;
        await InitLog();
    }

    #region private

    /// <summary>
    ///     checks:
    ///         if log already exists
    ///         if old log it too old (only holds log for the current day)
    ///         if log needs general cleanup
    ///         
    ///     then it overwrites/creates the log-file or just logs that the log was reinitialized
    /// </summary>
    private async Task InitLog()
    {
        var todayDateString = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        if (!File.Exists(logFileNameAndPath) || await LogNeedsCleanup(todayDateString) || logNeedsCleanup)
        {
            var fs = File.Create(logFileNameAndPath);
            fs.Dispose();

            Log($"---- Log Created: {todayDateString}");

            logNeedsCleanup = false;
            return;
        }

        Log($"---- Reinitialized: {todayDateString}");
    }

    public async Task<bool> LogNeedsCleanup(string today)
    {
        try
        {
            var logCreationDate = await GetLogCreationDate();

            return logCreationDate != today;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> GetLogCreationDate()
    {
        var firstLineOfLog = (await GetLog()).Split("\n")[0];

        // extract date from log and return trimmed
        return firstLineOfLog.Split(']')[1].Split(':')[2].Trim();
    }

    #endregion
}
