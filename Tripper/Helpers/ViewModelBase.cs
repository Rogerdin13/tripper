using Prism.Common;
using Tripper.Interfaces.Services;
using Tripper.Services;

namespace Tripper.Helpers;


public class ViewModelBase : ObservableRecipient
{
    protected readonly ILoggingService LoggingService;
    protected readonly INavigationService NavigationService;

    public ViewModelBase(ILoggingService loggingService, INavigationService navigationService)
    {
        LoggingService = loggingService;
        NavigationService = navigationService; 
    }
}
