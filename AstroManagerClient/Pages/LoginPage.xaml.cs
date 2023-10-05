namespace AstroManagerClient.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
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