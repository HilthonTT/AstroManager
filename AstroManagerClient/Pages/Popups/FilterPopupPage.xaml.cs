namespace AstroManagerClient.Pages.Popups;

public partial class FilterPopupPage : Popup
{
	public FilterPopupPage()
	{
		InitializeComponent();
        BindingContext = new FilterPopupViewModel();

        WeakReferenceMessenger.Default.Register<CloseFilterPopupMessage>(this, async (r, m) =>
        {
            await CloseAsync();
        });
    }
}