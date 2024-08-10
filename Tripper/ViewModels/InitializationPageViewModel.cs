using Prism.Common;
using Tripper.Helpers;
using Tripper.Interfaces.Services;
using Tripper.Services;

namespace Tripper.ViewModels;

public class InitializationPageViewModel : ViewModelBase
{
    public InitializationPageViewModel(ILoggingService loggingService, INavigationService navigationService, IPageAccessor pageAccessor) : base(loggingService, navigationService, pageAccessor) { }
}
