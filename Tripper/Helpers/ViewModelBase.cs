using Prism.Common;
using Tripper.Interfaces.Services;
using Tripper.Services;

namespace Tripper.Helpers;


public class ViewModelBase : ObservableRecipient
{
    protected readonly ILoggingService LoggingService;
    protected readonly INavigationService NavigationService;
    protected readonly IPageAccessor PageAccessor;

    public ViewModelBase(ILoggingService loggingService, INavigationService navigationService, IPageAccessor pageAccessor)
    {
        LoggingService = loggingService;
        NavigationService = navigationService; 
        PageAccessor = pageAccessor;
    }
}
