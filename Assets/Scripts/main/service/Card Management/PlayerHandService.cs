using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace main.service.Card_Management
{
    /// <summary>
    ///     This services provides the business logic for the player hand entity, which allows drawing cards,
    ///     playing them, etc.
    /// </summary>
    public class PlayerHandService : Service
    {
        /// <summary>
        ///     The player hand entity which contains the cards the player holds in their hand
        /// </summary>
        private readonly PlayerHand _playerHand = new();

        /// <summary>
        ///     Triggered when a card from the player's hand has been discarded.
        ///     If multiple cards are discarded, this event is triggered once per each card.
        ///     The index of the discarded card is its parameter.
        /// </summary>
        public readonly UnityEvent<int> OnCardDiscarded = new();

        /// <summary>
        ///     Triggered when a new card has been drawn.
        ///     If multiple cards are drawn, this event is triggered once per each card.
        /// </summary>
        public readonly UnityEvent<Card> OnCardDrawn = new();

        /// <summary>
        ///     Triggered when the entire hand has been discarded.
        /// </summary>
        public readonly UnityEvent OnHandDiscarded = new();

        /// <summary>
        ///     Creates the singleton instance
        /// </summary>
        public PlayerHandService()
        {
            Instance = this;
            LogInfo("Successfully set the PlayerHandService's singleton instance");
        }

        /// <summary>
        ///     The singleton instance of the service
        /// </summary>
        public static PlayerHandService Instance { get; private set; }

        /// <summary>
        ///     Plays the card at the specified index and then discards it
        /// </summary>
        /// <param name="index">the positive index of the cards within bounds</param>
        public void PlayCardAt(int index)
        {
            Assert.IsTrue(_playerHand.HandCards.Count > index && index >= 0,
                "The view should check if the card to play is within bounds!");

            var card = _playerHand.HandCards[index];
            LogInfo($"Playing card '{card}'");

            // TODO card effect implementation

            _playerHand.HandCards.RemoveAt(index);
            DiscardPileService.Instance.AddToPile(card);
            OnCardDiscarded.Invoke(index);

            LogInfo("Successfully played the card");
        }

        /// <summary>
        ///     Draws the specified amount of cards. If the amount is larger than the amount of cards left in the deck,
        ///     all cards from the discard pile will be shuffled back into the deck and the remaining cards will be
        ///     drawn from the newly shuffled deck.
        /// </summary>
        /// <param name="amountOfCardsToDraw">The amount of cards to draw as an integer</param>
        public void Draw(int amountOfCardsToDraw)
        {
            LogInfo($"Drawing {amountOfCardsToDraw} card(s)");
            var amountOfCardsInDeck = DeckService.Instance.Size();

            // TODO: new shuffle
            // TODO: if the deck is empty and discard pile are empty, just return out
            // Does the deck need to be refilled and reshuffled?
            if (amountOfCardsToDraw > amountOfCardsInDeck)
            {
                var remainingCardsToDrawAfterDrawingLastCardsFromDeck = amountOfCardsToDraw - amountOfCardsInDeck;

                // Draw all remaining cards from the deck
                DrawCardsFromDeck(amountOfCardsInDeck);

                // Now refill the deck and shuffle it
                DiscardPileService.Instance.ShuffleBackIntoDeck();

                // Now draw the remaining amount of cards
                DrawCardsFromDeck(remainingCardsToDrawAfterDrawingLastCardsFromDeck);
            }
            // If the deck has enough cards, just draw them
            else
            {
                DrawCardsFromDeck(amountOfCardsToDraw);
            }
        }

        /// <summary>
        ///     Helper method that will "actually" draw the cards from the deck and then add them to the hand.
        ///     All assertions, state checks and so on should be done by the caller!
        /// </summary>
        /// <param name="amount">The amount of cards to draw</param>
        private void DrawCardsFromDeck(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var drawnCard = DeckService.Instance.DrawFromTop();
                _playerHand.HandCards.Add(drawnCard);

                OnCardDrawn.Invoke(drawnCard);
                LogInfo("Triggered the OnCardDrawn event");
            }
        }

        /// <summary>
        ///     Removes all cards from the player's hand and adds them to the discard pile
        /// </summary>
        public void DiscardHand()
        {
            LogInfo("Discarding the entire player hand");

            foreach (var playerHandHandCard in _playerHand.HandCards)
                DiscardPileService.Instance.AddToPile(playerHandHandCard);

            _playerHand.HandCards.Clear();

            OnHandDiscarded.Invoke();
        }
    }
}