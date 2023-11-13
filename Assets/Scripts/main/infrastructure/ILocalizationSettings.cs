using UnityEngine.Localization;

namespace main.infrastructure
{
    public interface ILocalizationSettings
    {
        Locale SelectedLocale { get; }
    }
}