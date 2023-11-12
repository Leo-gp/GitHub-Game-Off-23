using System.Linq;
using main.entity.Card_Management;
using UnityEngine;

namespace main.service.Card_Management
{
    /// <summary>
    ///     This services provides the business logic for the deck entity, represented as a card pile.
    ///     The deck is created and shuffled automatically by using the starter deck definition entity.
    /// </summary>
    public class DeckService : Service
    {
        /// <summary>
        ///     Contains the deck of the player at all points in the game.
        ///     This is created automatically once the service is instantiated, and loads the starter deck as it is
        ///     defined in the editor in a random order (shuffled).
        /// </summary>
        private readonly CardPile _deck;

        /// <summary>
        ///     Creates the singleton of this service if it does not exist and then starts the game
        /// </summary>
        public DeckService()
        {
            Instance ??= this;
            LogInfo("Successfully set the DeckService's singleton instance");

            LogInfo("Now retrieving starter deck definition");
            _deck = new CardPile(StarterDeckDefinition.instance.Get(), true);
            LogInfo("Deck has been set and shuffled");

            LogInfo("Deck consists of these cards:");
            if (debugMode) _deck.Pile.ToList().ForEach(card => Debug.Log($"\t- {card}"));
        }

        /// <summary>
        ///     The non-thread-safe singleton of the service
        /// </summary>
        public static DeckService Instance { get; private set; }

        /// <summary>
        ///     Yields the deck
        /// </summary>
        /// <returns>The deck as a CardPile</returns>
        public CardPile GetDeck()
        {
            LogInfo("Yielding the deck pile");
            return _deck;
        }
    }
}