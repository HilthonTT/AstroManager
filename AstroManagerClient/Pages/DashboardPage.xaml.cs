using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Pages.Popups;
using AstroManagerClient.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microcharts;
using SkiaSharp.Views.Maui;

namespace AstroManagerClient.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly ICredentialEndpoint _credentialEndpoint;
    private readonly ILoggedInUser _loggedInUser;

    public DashboardPage(
        DashboardViewModel viewModel,
        ICredentialEndpoint credentialEndpoint,
        ILoggedInUser loggedInUser)
	{
		InitializeComponent();

		BindingContext = viewModel;
        _credentialEndpoint = credentialEndpoint;
        _loggedInUser = loggedInUser;

        WeakReferenceMessenger.Default.Register<OpenFilterPopupMessage>(this, async (r, m) =>
        {
            await this.ShowPopupAsync(new FilterPopupPage());
        });
    }

    protected override async void OnAppearing()
    {
        await DrawChartAsync();
    }

    private static float CalculateCount(float dataCount, int totalCount)
    {
        return dataCount * 100 / totalCount;
    }

    private async Task DrawChartAsync()
    {
        var allCredentials = await _credentialEndpoint.GetUsersCredentialsAsync(_loggedInUser.Id);

        var credentialCounts = allCredentials
            .GroupBy(x => x.Type.Name)
            .Select(group => new
            {
                Type = group.Key,
                Count = group.Count()
            })
            .ToList();

        float loginsCount = credentialCounts.FirstOrDefault(x => x.Type.Contains("Logins"))?.Count ?? 0;
        float passwordsCount = credentialCounts.FirstOrDefault(x => x.Type.Contains("Password"))?.Count ?? 0;
        float secureNoteCount = credentialCounts.FirstOrDefault(x => x.Type.Contains("Secure Note"))?.Count ?? 0;
        float creditCardCount = credentialCounts.FirstOrDefault(x => x.Type.Contains("Credit Card"))?.Count ?? 0;
        float identityCount = credentialCounts.FirstOrDefault(x => x.Type.Contains("Identity"))?.Count ?? 0;


        int totalCount = allCredentials.Count;

        TypesChart.Chart = new RadialGaugeChart
        {
            LabelTextSize = 24,
            BackgroundColor = Colors.Transparent.ToSKColor(),
            IsAnimated = true,
            Entries = new List<ChartEntry>
            {
                new ChartEntry(CalculateCount(loginsCount, totalCount))
                {
                    Color = Color.FromArgb("#0074FF").ToSKColor(),
                },
                new ChartEntry(CalculateCount(passwordsCount, totalCount))
                {
                    Color = Color.FromArgb("#3498DB").ToSKColor(), 
                },
                new ChartEntry(CalculateCount(secureNoteCount, totalCount))
                {
                    Color = Color.FromArgb("#66B2FF").ToSKColor(), 
                },
                new ChartEntry(CalculateCount(creditCardCount, totalCount))
                {
                    Color = Color.FromArgb("#3399FF").ToSKColor(),
                },
                new ChartEntry(CalculateCount(identityCount, totalCount))
                {
                    Color = Color.FromArgb("#0033CC").ToSKColor(), 
                }
            },
        };
    }
}