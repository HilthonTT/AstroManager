using CommunityToolkit.Mvvm.ComponentModel;

namespace AstroManagerClient.Models;
public partial class LanguageModel : ObservableObject
{
    [ObservableProperty]
    private string _language;

    [ObservableProperty]
    private Color _color = Color.FromArgb("#1F1D2B");

    public string ImageSource => GetImageSource();

    private string GetImageSource()
    {
        return Language.ToLower() switch
        {
            "english-us" or "us-english" => "us_flag.png",
            "french-fr" or "fr-french" => "france_flag.png",
            _ => "food_01.png",
        };
    }
}
