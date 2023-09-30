using AstroManagerClient.Messages;
using AstroManagerClient.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.Pages.Popups;

public partial class ErrorPopupPage : Popup
{
	public ErrorPopupPage()
	{
		InitializeComponent();
		BindingContext = new ErrorViewModel();

		WeakReferenceMessenger.Default.Register<OpenErrorPopupMessage>(this, async (r, m) =>
		{
			if (m.Value is false)
			{
				await CloseAsync();
			}
		});
	}
}