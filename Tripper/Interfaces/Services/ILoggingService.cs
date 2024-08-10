using System.Runtime.CompilerServices;

namespace Tripper.Interfaces.Services;

public interface ILoggingService
{
    void Log(string message);
    string GetLog();
    void ClearLog();
}
