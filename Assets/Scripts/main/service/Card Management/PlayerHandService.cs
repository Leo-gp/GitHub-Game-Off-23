using main.entity.Card_Management;

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
        ///     Creates the singleton instance
        /// </summary>
        public PlayerHandService()
        {
            Instance ??= this;
        }

        /// <summary>
        ///     The singleton instance of the service
        /// </summary>
        public static PlayerHandService Instance { get; private set; }

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
            }
        }
    }
}