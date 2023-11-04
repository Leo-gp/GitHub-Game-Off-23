using System.Collections.Generic;

namespace CardManagement
{
    /// <summary>
    /// The CardPool entity is used to provide access to all in-game cards.
    /// The hash set is filled by its own repository.
    /// </summary>
    /// <author>Gino</author>
    public class CardPool
    {
        /// <summary>
        /// Contains the pool of all cards that are available in the game, whether the player
        /// already has them or not.
        /// The set is created automatically and filled by the repository.
        /// </summary>
        /// <returns>A non-null hash set of all game cards</returns>
        public HashSet<Card> AllGameCards { private set; get; } =
            new(new CardPoolRepository().GetAll());

    }

}
