using Prism.Common;
using Tripper.Helpers;
using Tripper.Interfaces.Services;
using Tripper.Services;

namespace Tripper.ViewModels;

public class CustomTitleViewModel : ViewModelBase
{
    private bool isLogPage = false;

    #region commands

    public ICommand GoToLogPageCommand => new Command(async () =>
    {
        if (isLogPage)
        {
            isLogPage = false;
            await NavigationService.GoBackAsync();
            return;
        }
        isLogPage = true;
        await NavigationService.NavigateAsync("NavigationPage/LogPage");
    });

    #endregion

    public CustomTitleViewModel(ILoggingService loggingService, INavigationService navigationService) 
        : base(loggingService, navigationService) { }
}
