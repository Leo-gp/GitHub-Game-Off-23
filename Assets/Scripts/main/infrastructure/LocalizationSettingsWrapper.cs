using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace main.infrastructure
{
    public class LocalizationSettingsWrapper : ILocalizationSettings
    {
        public Locale SelectedLocale
        {
            get => LocalizationSettings.SelectedLocale;
            set => LocalizationSettings.SelectedLocale = value;
        }
    }
}