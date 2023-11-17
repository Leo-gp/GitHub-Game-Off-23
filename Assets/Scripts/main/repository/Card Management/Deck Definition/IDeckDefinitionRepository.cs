using main.entity.Card_Management.Deck_Definition;

namespace main.repository.Card_Management.Deck_Definition
{
    public interface IDeckDefinitionRepository
    {
        DeckDefinition LoadDeckDefinition();
    }
}