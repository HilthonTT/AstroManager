namespace AstroManagerClient.Models;
public partial class ThemeModel : ObservableObject
{
    public string Theme { get; set; }
    public string ImageSource => GetImageSource();

    [ObservableProperty]
    private Color _color = Color.FromArgb("#1F1D2B");

    private string GetImageSource()
    {
        return Theme.ToLower() switch
        {
            "dark mode" or "darkmode" => "night_mode.png",
            "light mode" or "lightmode" => "light_mode.png",
            _ => "food_01.png",
        };
    }
}
