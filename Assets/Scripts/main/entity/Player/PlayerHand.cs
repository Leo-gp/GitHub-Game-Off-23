using System.Collections.Generic;

namespace CardManagement
{
    /// <summary>
    /// The PlayerHand entity contains the list of cards that the player holds in their hand.
    /// A new list is instantiated when the PlayerHand object is created, allowing us to simply
    /// create a new PlayerHand instance whenever a new game is started.
    /// </summary>
    /// <author>Gino</author>
    public class PlayerHand
    {
        /// <summary>
        /// The non-null list of hand cards.
        /// Cannot be set manually.
        /// </summary>
        /// <value>The current list of all cards the player holds in their hand</value>
        public List<Card> HandCards { private set; get; }

        /// <summary>
        /// Creates a new empty list of hand cards when instantiated
        /// </summary>
        public PlayerHand() => HandCards = new();

    }

}
