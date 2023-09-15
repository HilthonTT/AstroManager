using AstroManagerClient.ViewModels;

namespace AstroManagerClient.Pages;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}