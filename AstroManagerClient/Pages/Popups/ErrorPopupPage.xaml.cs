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