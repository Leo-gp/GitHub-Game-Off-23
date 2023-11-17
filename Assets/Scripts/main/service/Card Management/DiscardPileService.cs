using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace main.service.Card_Management
{
    public class DiscardPileService : Service
    {
        /// <summary>
        ///     The card pile entity, which represents the discard pile.
        ///     This is created automatically once the service is instantiated.
        /// </summary>
        private readonly CardPile _discardPile = new();

        /// <summary>
        ///     Creates the singleton
        /// </summary>
        public DiscardPileService()
        {
            Instance ??= this;
            LogInfo("Successfully set the DiscardPileService's singleton instance");
        }

        /// <summary>
        ///     The singleton instance of the service
        /// </summary>
        public static DiscardPileService Instance { get; private set; }

        /// <summary>
        ///     Adds the card to the top of the discard pile
        /// </summary>
        /// <param name="card">The card instance that should be pushed to the discard pile stack</param>
        public void AddToPile([NotNull] Card card)
        {
            LogInfo($"Discarding card '{card}'");
            _discardPile.Pile.Push(card);
        }

        /// <summary>
        ///     Removes all cards from the discard pile and shuffles them randomly back into the deck
        /// </summary>
        public void ShuffleBackIntoDeck()
        {
            // The discard pile should only be shuffled back into the deck if the deck is empty
            Assert.IsTrue(DeckService.Instance.IsEmpty());

            LogInfo("Shuffling the discard pile back into the deck");

            // Gather all cards from the stack and save them in a list
            var asList = new List<Card>();
            while (_discardPile.Pile.Count > 0) asList.Add(_discardPile.Pile.Pop());

            LogInfo("Removed all cards from the discard pile");

            // Randomly add them back into the deck
            while (asList.Count > 0)
            {
                var nextIndexToRemove = Random.Range(0, asList.Count);
                DeckService.Instance.AddCard(asList[nextIndexToRemove]);
                asList.RemoveAt(nextIndexToRemove);
            }
        }

        /// <summary>
        ///     Yields the discard pile as a list of cards
        /// </summary>
        /// <returns>The discard pile stack converted to a list</returns>
        public List<Card> ToList()
        {
            return _discardPile.Pile.ToList();
        }
    }
}