using Prism.Common;
using Tripper.Helpers;
using Tripper.Interfaces.Services;
using Tripper.Services;

namespace Tripper.ViewModels;

public class CustomTitleViewModel : ViewModelBase
{
    public bool isLogPage = false;

    #region commands

    public ICommand GoToLogPageCommand => new Command(async () =>
    {
        if (isLogPage) return;
        try
        {
            var result = await NavigationService.CreateBuilder().UseRelativeNavigation().AddSegment("Home/LogPage").NavigateAsync();
            isLogPage = result.Success;
        }
        catch (Exception ex)
        { 
            isLogPage = false;
        }        
    });

    #endregion

    public CustomTitleViewModel(ILoggingService loggingService, INavigationService navigationService) 
        : base(loggingService, navigationService) { }
}
