﻿using AstroManagerClient.Resources.Languages;
using System.ComponentModel;
using System.Globalization;

namespace AstroManagerClient;
public class LocalizationResourceManager : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private LocalizationResourceManager()
    {
        AppResources.Culture = CultureInfo.CurrentCulture;
    }

    public static LocalizationResourceManager Instance { get; } = new();
    public object this[string resourceKey] => 
        AppResources.ResourceManager.GetObject(resourceKey, AppResources.Culture) ?? Array.Empty<byte>();

    public void SetCulture(CultureInfo culture) 
    {
        AppResources.Culture = culture;
        PropertyChanged?.Invoke(this, new(null));
    }
}