using UnityEngine.Localization;

namespace main.infrastructure
{
    public interface ILocalizationSettings
    {
        LocaleIdentifier SelectedLocaleIdentifier { get; }
    }
}