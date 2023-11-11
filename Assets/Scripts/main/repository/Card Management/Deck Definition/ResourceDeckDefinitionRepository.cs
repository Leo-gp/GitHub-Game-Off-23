using main.entity.Card_Management.Deck_Definition;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization.Settings;

namespace main.repository.Card_Management.Deck_Definition
{
    public abstract class ResourceDeckDefinitionRepository : IDeckDefinitionRepository
    {
        protected abstract string ResourcePath { get; }
        
        public DeckDefinition Load()
        {
            return LoadFromResources();
        }
        
        private DeckDefinition LoadFromResources()
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.ProjectLocale; // TODO: Remove
            
            Assert.IsNotNull(LocalizationSettings.SelectedLocale, "There is no locale selected");

            var path = LocalizationSettings.SelectedLocale.Identifier + ResourcePath;

            var definition = Resources.LoadAll<DeckDefinition>(path);

            Assert.AreEqual(definition.Length, 1,
                $"There should be exactly one deck definition in Resources path: \"{path}\"");

            return definition[0];
        }
    }
}