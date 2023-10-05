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