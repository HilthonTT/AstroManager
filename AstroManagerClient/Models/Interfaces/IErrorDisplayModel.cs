namespace AstroManagerClient.Models.Interfaces;
public interface IErrorDisplayModel
{
    public string ErrorMessage { get; set; }

    void SetErrorMessage(string message);
}
