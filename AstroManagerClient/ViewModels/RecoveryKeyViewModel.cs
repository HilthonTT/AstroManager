using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class RecoveryKeyViewModel : BaseViewModel
{
    public RecoveryKeyViewModel()
    {
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
    private bool _isHidden;

    private void LoadRecoveryKeys(RecoveryKeyDisplayModel recovery)
    {
        RecoveryKeys = recovery;

        var keyValues = new List<KeyValueModel>();

        for (int i = 0; i < RecoveryKeys.RecoveryKeys.Count; i++)
        {
            keyValues.Add(new()
            {
                KeyIndex = i,
                Value = RecoveryKeys.RecoveryKeys.ToList()[i]
            });
        }

        KeyValues = new(keyValues);
    }

    [RelayCommand]
    private void ToggleHidden()
    {
        IsHidden = !IsHidden;
    }
}
