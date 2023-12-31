namespace AstroManagerClient.Pages.Popups;

public partial class AddCredentialPopupPage : Popup
{
	public AddCredentialPopupPage()
	{
		InitializeComponent();
		BindingContext = new AddCredentialViewModel();

        WeakReferenceMessenger.Default.Register<OpenCreateCredentialMessage>(this, async (r, m) =>
		{
			if (m.Value is false)
			{
                await CloseAsync();
            }
		});
	}
}