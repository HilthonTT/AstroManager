using AstroManagerClient.Messages;
using AstroManagerClient.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.Pages.Popups;

public partial class AccountPopupPage : Popup
{
	public AccountPopupPage()
	{
		InitializeComponent();
		BindingContext = new AccountPopupViewModel();

		WeakReferenceMessenger.Default.Register<OpenAccountPopupMessage>(this, async (r, m) =>
		{
			if (m.Value is false)
			{
				await CloseAsync();
			}
		});
	}
}