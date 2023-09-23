using AstroManagerClient.ViewModels;
using Microcharts;
using SkiaSharp.Views.Maui;

namespace AstroManagerClient.Pages;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DrawChart();
    }

    private void DrawChart()
    {
        TypesChart.Chart = new RadialGaugeChart
        {
            LabelTextSize = 24,
            BackgroundColor = Colors.Transparent.ToSKColor(),
            IsAnimated = true,
            Entries = new List<ChartEntry>
            {
                new ChartEntry(10)
                {
                    Color = Color.FromArgb("#EA7C69").ToSKColor()
                },
                new ChartEntry(51)
                {
                    Color = Color.FromArgb("#50D1AA").ToSKColor()
                },
                new ChartEntry(14)
                {
                    Color = Color.FromArgb("#FF7CA3").ToSKColor()
                },new ChartEntry(2)
                {
                    Color = Colors.WhiteSmoke.ToSKColor()
                }
            },
        };
    }
}