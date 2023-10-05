namespace AstroManagerClient.ViewModels;
public partial class RecoveryKeyViewModel : BaseViewModel
{
    private readonly IErrorDisplayModel _error;
    private readonly IRecoveryKeyEndpoint _recoveryKeyEndpoint;
    private readonly ILoggedInUser _loggedInUser;

    public RecoveryKeyViewModel()
    {
        _error = App.Services.GetService<IErrorDisplayModel>();
        _recoveryKeyEndpoint = App.Services.GetService<IRecoveryKeyEndpoint>();
        _loggedInUser = App.Services.GetService<ILoggedInUser>();

        WeakReferenceMessenger.Default.Register<ViewRecoveryMessage>(this, (r, m) =>
        {
            LoadRecoveryKeys(m.Value);
        });
    }

    [ObservableProperty]
    private RecoveryKeyDisplayModel _recoveryKeys;

    [ObservableProperty]
    private List<KeyValueModel> _keyValues;

    [ObservableProperty]
    private bool _isPassword = true;

    [ObservableProperty]
    private string _isHidden;
    async partial void OnIsHiddenChanged(string value)
    {
        try
        {
            IsPassword = value.ToLower() switch
            {
                "show" => false,
                "hidden" => true,
                _ => true,
            };

            var recovery = await _recoveryKeyEndpoint.GetUsersRecoveryKeyAsync(_loggedInUser.Id);
            var mappedRecovery = new RecoveryKeyDisplayModel(recovery);

            LoadRecoveryKeys(mappedRecovery);
        }
        catch (Exception ex)
        {
            _error.SetErrorMessage(ex.Message);
            OpenErrorPopup();
        }
    }

    private void LoadRecoveryKeys(RecoveryKeyDisplayModel recovery)
    {
        RecoveryKeys = recovery;

        var keyValues = new List<KeyValueModel>();

        for (int i = 0; i < RecoveryKeys.RecoveryKeys.Count; i++)
        {
            string first22Chars;

            if (IsPassword)
            {
                first22Chars = string.Concat(Enumerable.Repeat("*", 22));
            }
            else
            {
                string value = RecoveryKeys.RecoveryKeys.ToList()[i];
                first22Chars = value[..Math.Min(value.Length, 22)] + "...";   
            }

            keyValues.Add(new()
            {
                KeyIndex = i,
                Value = first22Chars,
            });
        }

        KeyValues = new(keyValues);
    }

    [RelayCommand]
    private async Task CopyAsync()
    {
        var stringBuilder = new StringBuilder();

        int index = 1;

        foreach (var recoveryKey in RecoveryKeys.RecoveryKeys)
        {
            stringBuilder.AppendLine($"{index}: {recoveryKey}");

            index++;
        }

        string resultString = stringBuilder.ToString();

        await Clipboard.SetTextAsync(resultString);
        await Shell.Current.DisplayAlert(
            "Keys Copied!", "Your keys have been copied to the clipboard.", "OK");
    }
}
