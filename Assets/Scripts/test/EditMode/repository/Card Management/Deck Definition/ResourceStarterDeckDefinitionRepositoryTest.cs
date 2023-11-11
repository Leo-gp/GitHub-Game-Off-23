using main.repository.Card_Management.Deck_Definition;
using NUnit.Framework;

namespace test.EditMode.repository.Card_Management.Deck_Definition
{
    public class ResourceStarterDeckDefinitionRepositoryTest
    {
        [Test]
        public void WhenFileFoundInPath_returnDeckDefinition()
        {
            var repository = new ResourceStarterDeckDefinitionRepository();
            var deckDefinition = repository.Load();
            
            Assert.IsNotNull(deckDefinition);
        }
    }
}