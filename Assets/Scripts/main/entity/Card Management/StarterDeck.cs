using System.Collections.Generic;
using main.entity.Card_Management.Card_Data;

namespace main.entity.Card_Management
{
    public class StarterDeck
    {
        public List<Card> Cards { get; }

        public StarterDeck(List<Card> cards)
        {
            Cards = cards;
        }
    }
}