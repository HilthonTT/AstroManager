﻿using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class AddCredentialViewModel : BaseViewModel
{ 
    private readonly ICredentialTemplateEndpoint _templateEndpoint;
    private readonly ICredentialEndpoint _credentialEndpoint;
    private readonly ILoggedInUser _loggedInUser;

    public AddCredentialViewModel()
    {
        _templateEndpoint = App.Services.GetService<ICredentialTemplateEndpoint>();
        _loggedInUser = App.Services.GetService<ILoggedInUser>();
        _credentialEndpoint = App.Services.GetService<ICredentialEndpoint>();

        Height = GetHeight();
    }

    [ObservableProperty]
    private TemplateDisplayModel _template;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _type;

    [ObservableProperty]
    private double _height;

    async partial void OnTypeChanged(string value)
    {
        var templates = await _templateEndpoint.GetAllTemplatesAsync();

        Template = new TemplateDisplayModel(
            templates.FirstOrDefault(x =>
                x.Type.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase))
        );
    }

    private static double GetHeight()
    {
        double? height = Application.Current.Windows[0]?.Height;

        // Get 80% of the height 
        if (height is not null)
        {
            return (height / 100 * 80).GetValueOrDefault();
        }

        return 500;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlert("No title provided", "You must provide a title.", "OK");
            return;
        }

        var c = new CredentialDisplayModel()
        {
            Title = Title,
            Fields = Template.Fields,
            Type = Template.Type,
            User = new BasicUserModel((UserModel)_loggedInUser),
            Notes = "",
        };

        var credentialToCreate = ModelConverter.GetCredential(c);

        await _credentialEndpoint.CreateCredentialAsync(credentialToCreate);

        Template = new();
        Title = "";
    }

    [RelayCommand]
    private static void Close()
    {
        var message = new OpenCreateCredentialMessage(false);
        WeakReferenceMessenger.Default.Send(message);
    }
}
