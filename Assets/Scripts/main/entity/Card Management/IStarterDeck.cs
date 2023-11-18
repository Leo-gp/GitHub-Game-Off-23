using System.Collections.Generic;
using main.entity.Card_Management.Card_Data;

namespace main.entity.Card_Management
{
    public interface IStarterDeck
    {
        List<Card> Cards { get; }

        bool IsEmpty();
    }
}