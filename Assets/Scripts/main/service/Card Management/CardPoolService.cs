using JetBrains.Annotations;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using UnityEngine.Assertions;

namespace main.service.Card_Management
{
    /// <summary>
    ///     This services provides the business logic for the card pool entity, which stores available cards.
    ///     At the start of the game, it should load all cards from the card vault and add them to the pool.
    ///     Then, all cards from the starter deck should be removed from the card pool.
    /// </summary>
    public class CardPoolService : Service
    {
        /// <summary>
        ///     The card pool entity containing discovered / available cards
        /// </summary>
        private readonly CardPool _cardPool = new();

        /// <summary>
        ///     Creates the singleton instance
        /// </summary>
        public CardPoolService()
        {
            Instance ??= this;
        }

        /// <summary>
        ///     The instance of the service singleton
        /// </summary>
        public static CardPoolService Instance { get; private set; }

        /// <summary>
        ///     Adds a non-null card to the pool of discovered cards
        /// </summary>
        /// <param name="cardToAdd">The non-null that should be added to the pool</param>
        public void AddCard([NotNull] Card cardToAdd)
        {
            _cardPool.Pool.Add(cardToAdd);
            LogInfo($"Added card '{cardToAdd}' to the card pool");
        }

        /// <summary>
        ///     Removes a non-null card instance from the pool of discovered cards
        /// </summary>
        /// <param name="cardToRemove">The non-null card reference to remove</param>
        public void RemoveCard([NotNull] Card cardToRemove)
        {
            var refExisted = _cardPool.Pool.Remove(cardToRemove);
            Assert.IsTrue(refExisted, "Trying to remove a card from the card pool, which does not exist there");
            LogInfo($"Removed card '{cardToRemove}' from the card pool");
        }

        /// <summary>
        ///     Yields the amount of cards in the pool
        /// </summary>
        /// <returns>The amount of cards as an integer</returns>
        public int Size()
        {
            return _cardPool.Pool.Count;
        }
    }
}