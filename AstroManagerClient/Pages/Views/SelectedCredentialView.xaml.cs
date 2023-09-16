using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;

namespace AstroManagerClient.Pages.Views;

public partial class SelectedCredentialView : ContentView
{
    public SelectedCredentialView()
	{
		InitializeComponent();
		BindingContext = this;

		WeakReferenceMessenger.Default.Register<CredentialDisplayModel>(this, (r, m) =>
		{
			Credential = m;
		});
	}

    public static readonly BindableProperty CredentialProperty = BindableProperty.Create(
    nameof(CredentialProperty), typeof(CredentialDisplayModel), typeof(ContentView), new CredentialDisplayModel());

    public CredentialDisplayModel Credential
	{ 
		get => (CredentialDisplayModel)GetValue(CredentialProperty);
		set
		{
            SetValue(CredentialProperty, value);
			OnPropertyChanged(nameof(Credential));
        }
	}
}