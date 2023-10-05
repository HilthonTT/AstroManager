namespace AstroManagerClient.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
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

        WeakReferenceMessenger.Default.Register<OpenAccountPopupMessage>(this, async (r, m) =>
        {
            if (m.Value)
            {
                await this.ShowPopupAsync(new AccountPopupPage());
            }
        });
    }
}