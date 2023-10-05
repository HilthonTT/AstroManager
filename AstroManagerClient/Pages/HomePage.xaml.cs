namespace AstroManagerClient.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        RegisterMessages();
    }


    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<OpenCreateCredentialMessage>(this, async (r, m) =>
        {
            if (m.Value)
            {
                await this.ShowPopupAsync(new AddCredentialPopupPage());
            }
        });

        WeakReferenceMessenger.Default.Register<OpenErrorPopupMessage>(this, async (r, m) =>
        {
            if (m.Value)
            {
                await this.ShowPopupAsync(new ErrorPopupPage());
            }
        });
    }

    public void MenuFlyoutItem_ParentChanged(object sender, EventArgs e)
    {
        if (sender is BindableObject bo)
        {
            bo.BindingContext = BindingContext;
        }
    }

    public void NavSubContent<T>(bool show) where T : ContentView, new()
    {
        var displayWidth = DeviceDisplay.Current.MainDisplayInfo.Width;

        if (show)
        {
            var contentView = new T();
            PageGrid.Add(contentView, 1);
            Grid.SetRowSpan(contentView, 3);
            // translate off screen right
            contentView.TranslationX = displayWidth - contentView.X;
            contentView.TranslateTo(0, 0, 800, easing: Easing.CubicOut);
        }
        else
        {
            // remove the product window

            var view = PageGrid.Children.OfType<T>().SingleOrDefault();

            var x = DeviceDisplay.Current.MainDisplayInfo.Width;  

            if (view is not null)
            {
                view.TranslateTo(displayWidth - view.X, 0, 800, easing: Easing.CubicIn);
                PageGrid.Children.Remove(view);
            }
        }
    }
}