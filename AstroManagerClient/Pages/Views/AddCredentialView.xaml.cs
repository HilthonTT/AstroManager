using AstroManagerClient.ViewModels;

namespace AstroManagerClient.Pages.Views;

public partial class AddCredentialView : ContentView
{
	public AddCredentialView()
	{
		InitializeComponent();
		BindingContext = new AddCredentialViewModel();
	}
}