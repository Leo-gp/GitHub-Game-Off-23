using System.Collections.Generic;

namespace main.entity.Card_Management
{
    /// <summary>
    ///     This entity is used to track discovered cards.
    ///     At the start of the game, this should be the relative component set of the card vault and the starter deck.
    /// </summary>
    public class CardPool
    {
        /// <summary>
        ///     Contains the pool of discovered cards as a list.
        ///     The instance is created automatically.
        /// </summary>
        public List<Card> Pool { get; private set; } = new();
    }
}