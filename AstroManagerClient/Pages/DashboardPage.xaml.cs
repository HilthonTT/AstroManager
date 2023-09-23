using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.ViewModels;
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

        float loginsCount = allCredentials.Where(x => x.Type.Name.Contains("Logins")).Count();
        float passwordsCount = allCredentials.Where(x => x.Type.Name.Contains("Password")).Count();
        float secureNoteCount = allCredentials.Where(x => x.Type.Name.Contains("Secure Note")).Count();
        float creditCardCount = allCredentials.Where(x => x.Type.Name.Contains("Credit Card")).Count();
        float identityCount = allCredentials.Where(x => x.Type.Name.Contains("Identity")).Count();

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
                    Color = Color.FromArgb("#EA7C69").ToSKColor(),
                },
                new ChartEntry(CalculateCount(passwordsCount, totalCount))
                {
                    Color = Color.FromArgb("#50D1AA").ToSKColor()
                },
                new ChartEntry(CalculateCount(secureNoteCount, totalCount))
                {
                    Color = Color.FromArgb("#FF7CA3").ToSKColor()
                },
                new ChartEntry(CalculateCount(creditCardCount, totalCount))
                {
                    Color = Colors.WhiteSmoke.ToSKColor()
                },
                new ChartEntry(CalculateCount(identityCount, totalCount))
                {
                    Color = Colors.PaleVioletRed.ToSKColor()
                }
            },
        };
    }
}