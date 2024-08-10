using Prism.Common;
using Tripper.Helpers;
using Tripper.Interfaces.Services;
using Tripper.Services;

namespace Tripper.ViewModels;

public class LogPageViewModel : ViewModelBase
{
	private string logContents = "";
	public string LogContents
    {
        get => logContents;
        set => SetProperty(ref logContents, value);
    }

    public LogPageViewModel(ILoggingService loggingService, INavigationService navigationService) 
        : base(loggingService, navigationService) 
    {
        LogContents = LoggingService.GetLog();
    }
}
