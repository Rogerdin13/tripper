using Prism.Common;
using Tripper.Helpers;
using Tripper.Interfaces.Services;
using Tripper.Services;

namespace Tripper.ViewModels;

public class CustomTitleViewModel : ViewModelBase
{
    #region commands

    public ICommand SwitchToLogPageCommand => new Command(async () =>
    {
        var currentPage = PageAccessor?.Page.Navigation.NavigationStack[0].Title;
        var pageToGoTo = currentPage == "Home" ? "NavigationPage/LogPage" : "NavigationPage/Home";
        await NavigationService.NavigateAsync(pageToGoTo);
    });

    #endregion

    public CustomTitleViewModel(ILoggingService loggingService, INavigationService navigationService, IPageAccessor pageAccessor) : base(loggingService, navigationService, pageAccessor) { }
}
