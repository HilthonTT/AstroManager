namespace AstroManagerClient.Pages.Views;

public partial class SelectedCredentialView : ContentView
{
    public SelectedCredentialView()
	{
		InitializeComponent();
		BindingContext = new SelectedCredentialViewModel();
	}
}