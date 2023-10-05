namespace AstroManagerClient.Models;
public partial class ErrorDisplayModel : ObservableObject, IErrorDisplayModel
{
    [ObservableProperty]
    private string _errorMessage;

    public void SetErrorMessage(string message)
    {
        ErrorMessage = $"Something went wrong: {message}";
    }
}
