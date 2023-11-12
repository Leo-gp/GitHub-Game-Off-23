using main.entity.Card_Management.Deck_Definition;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using NUnit.Framework;

namespace test.EditMode.integration.main.repository.Card_Management.Deck_Definition
{
    [TestFixture]
    public class ResourceDeckDefinitionRepositoryTest
    {
        // If other languages start being supported, this class should check each language's path
        private const string StarterDeckDefinitionResourcePath = "English(en)/Starter Deck";
        private const string CardPoolDeckDefinitionResourcePath = "English(en)/Card Pool";
        
        private ResourceDeckDefinitionRepository repository;
        
        [TestFixture]
        public class LoadDeckDefinitionTest : ResourceDeckDefinitionRepositoryTest
        {
            [Test]
            public void VerifySingleStarterDeckDefinitionResourceFound()
            {
                var resourceLoader = new ResourceLoader<StarterDeckDefinition>(StarterDeckDefinitionResourcePath);
                
                repository = new ResourceDeckDefinitionRepository(resourceLoader);
                
                var result = repository.LoadDeckDefinition();
            
                Assert.NotNull(result);
                
                Assert.That(result, Is.TypeOf<StarterDeckDefinition>());
            }
        
            [Test]
            public void VerifySingleCardPoolDeckDefinitionResourceFound()
            {
                var resourceLoader = new ResourceLoader<CardPoolDeckDefinition>(CardPoolDeckDefinitionResourcePath);
                
                repository = new ResourceDeckDefinitionRepository(resourceLoader);
                
                var result = repository.LoadDeckDefinition();
            
                Assert.NotNull(result);
                
                Assert.That(result, Is.TypeOf<CardPoolDeckDefinition>());
            }
        }
    }
}