using AstroManagerClient.Messages;
using AstroManagerClient.Pages.Views;
using AstroManagerClient.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        WeakReferenceMessenger.Default.Register<ViewCredentialMessage>(this, (r, m) =>
        {
            if (m.Value is not null)
            {
                NavSubContent(false);
            }
        });

        WeakReferenceMessenger.Default.Register<AddCredentialMessage>(this, (r, m) =>
        {
            NavSubContent(m.Value);
        });
    }

    public void MenuFlyoutItem_ParentChanged(object sender, EventArgs e)
    {
        if (sender is BindableObject bo)
        {
            bo.BindingContext = this.BindingContext;
        }
    }

    public void NavSubContent(bool show)
    {
        var displayWidth = DeviceDisplay.Current.MainDisplayInfo.Width;

        if (show)
        {
            var addForm = new AddCredentialView();
            PageGrid.Add(addForm, 1);
            Grid.SetRowSpan(addForm, 3);
            // translate off screen right
            addForm.TranslationX = displayWidth - addForm.X;
            addForm.TranslateTo(0, 0, 800, easing: Easing.CubicOut);
        }
        else
        {
            // remove the product window

            var view = (AddCredentialView)PageGrid.Children.Where(v => v.GetType() == typeof(AddCredentialView)).SingleOrDefault();

            var x = DeviceDisplay.Current.MainDisplayInfo.Width;  

            if (view is not null)
            {
                view.TranslateTo(displayWidth - view.X, 0, 800, easing: Easing.CubicIn);
                PageGrid.Children.Remove(view);
            }
        }
    }
}