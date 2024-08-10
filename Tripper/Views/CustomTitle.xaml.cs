using Tripper.ViewModels;

namespace Tripper.Views;

public partial class CustomTitle : ContentView
{
	public CustomTitle()
	{
		BindingContext = Application.Current!.Handler.MauiContext!.Services.GetServices<CustomTitleViewModel>().FirstOrDefault();
		InitializeComponent();
	}
}