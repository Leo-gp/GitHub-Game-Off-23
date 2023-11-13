using main.entity.Card_Management.Deck_Definition;
using UnityEngine.Assertions;

namespace main.repository.Card_Management.Deck_Definition
{
    public class ResourceDeckDefinitionRepository : IDeckDefinitionRepository
    {
        private readonly IResourceLoader<DeckDefinition> resourceLoader;

        public ResourceDeckDefinitionRepository(IResourceLoader<DeckDefinition> resourceLoader)
        {
            this.resourceLoader = resourceLoader;
        }

        public DeckDefinition LoadDeckDefinition()
        {
            return LoadFromResources();
        }
        
        private DeckDefinition LoadFromResources()
        {
            var definition = resourceLoader.GetAll();

            Assert.AreEqual(definition.Length, 1,
                $"There should be exactly one deck definition in Resources path: \"{resourceLoader.ResourcePath}\"");

            return definition[0];
        }
    }
}