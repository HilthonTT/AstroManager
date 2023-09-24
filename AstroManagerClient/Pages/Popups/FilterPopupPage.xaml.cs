using AstroManagerClient.Messages;
using AstroManagerClient.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

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