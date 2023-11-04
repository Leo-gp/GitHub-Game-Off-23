using System.Collections.Generic;

namespace CardManagement
{
    /// <summary>
    /// The card pile entity is used for the player's deck, the discard pile, and other similar
    /// card piles. It consists of a non-null stack of cards. 
    /// </summary>
    /// <author>Gino</author>
    public class CardPile
    {
        /// <summary>
        /// The non-null stack containing the cards of the pile.
        /// The pile can only be accessed, but not rewritten.
        /// A new empty stack is created automatically.
        /// </summary>
        /// <returns>The non-null stack containing all cards in the pile</returns>
        public Stack<Card> Pile { private set; get; } = new();

    }

}
