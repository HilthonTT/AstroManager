using AstroManagerClient.ViewModels;
using CommunityToolkit.Maui.Views;

namespace AstroManagerClient.Pages.Popups;

public partial class AccountPopupPage : Popup
{
	public AccountPopupPage()
	{
		InitializeComponent();
		BindingContext = new AccountPopupViewModel();
	}
}