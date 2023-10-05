namespace AstroManagerClient.Pages;

public partial class BreachedCredentialsPage : ContentPage
{
	public BreachedCredentialsPage(BreachedAccountViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        RegisterMessages();
    }

	private void RegisterMessages()
	{
        WeakReferenceMessenger.Default.Register<OpenErrorPopupMessage>(this, async (r, m) =>
        {
            if (m.Value)
            {
                await this.ShowPopupAsync(new ErrorPopupPage());
            }
        });
    }
}