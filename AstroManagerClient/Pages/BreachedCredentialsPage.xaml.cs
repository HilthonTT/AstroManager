using AstroManagerClient.ViewModels;

namespace AstroManagerClient.Pages;

public partial class BreachedCredentialsPage : ContentPage
{
	public BreachedCredentialsPage(BreachedAccountViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

    }
}