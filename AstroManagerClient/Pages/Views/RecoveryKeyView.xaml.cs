using AstroManagerClient.ViewModels;

namespace AstroManagerClient.Pages.Views;

public partial class RecoveryKeyView : ContentView
{
	public RecoveryKeyView()
	{
		InitializeComponent();
		BindingContext = new RecoveryKeyViewModel();
	}
}