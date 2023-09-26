using AstroManagerClient.Messages;
using AstroManagerClient.Pages.Popups;
using AstroManagerClient.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		WeakReferenceMessenger.Default.Register<OpenAccountPopupMessage>(this, async (r, m) =>
		{
			if (m.Value)
			{
                await this.ShowPopupAsync(new AccountPopupPage());
            }
		});
	}
}