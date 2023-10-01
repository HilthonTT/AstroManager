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
            "english" => "us_flag.png",
            "français" => "france_flag.png",
            "deutsch" => "germany_flag.png",
            "bahasa indonesia" => "indonesia_flag.png",
            _ => "food_01.png",
        };
    }
}
