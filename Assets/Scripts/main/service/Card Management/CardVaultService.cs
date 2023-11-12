using System.Collections.Generic;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using UnityEngine.Assertions;

namespace main.service.Card_Management
{
    /// <summary>
    ///     This service provides the business logic for the card vault entity, which is mainly used to get all cards
    ///     from the game. This collection will never change.
    /// </summary>
    public class CardVaultService : Service
    {
        /// <summary>
        ///     The card vault entity containing all in-game cards
        /// </summary>
        private readonly CardVault _cardVault = new();

        /// <summary>
        ///     Creates the singleton instance
        /// </summary>
        public CardVaultService()
        {
            Instance ??= this;
            LogInfo("Successfully set the EffectAssemblyService's singleton instance");

            Assert.IsFalse(_cardVault.AllGameCards.Count is 0, "Card vault should not be empty");

            LogInfo("Card vault consists of the following cards:");
            foreach (var card in GetAll()) LogInfo($"- {card}");
        }

        /// <summary>
        ///     The instance of the service singleton
        /// </summary>
        public static CardVaultService Instance { get; private set; }

        /// <summary>
        ///     Yields all cards from the vault
        /// </summary>
        /// <returns>The cards as a hash set</returns>
        public HashSet<Card> GetAll()
        {
            LogInfo("Retrieving all cards from the vault");
            return _cardVault.AllGameCards;
        }
    }
}