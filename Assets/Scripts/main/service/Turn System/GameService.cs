using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using main.entity.Turn_System;
using main.service.Card_Management;
using main.service.Fish_Management;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace main.service.Turn_System
{
    /// <summary>
    ///     This service provides the business logic for the game entity, including a way to end the current turn.
    ///     TODO: I feel like this class is responsible for too much, let's review it!
    /// </summary>
    public class GameService : Service
    {
        /// <summary>
        ///     The non-null game entity created automatically when the service is instantiated.
        /// </summary>
        [NotNull] private readonly Game _game = new();

        /// <summary>
        ///     Triggered once the card swap has selected three cards from the players deck.
        ///     The player must choose one of these cards and remove it from the deck.
        ///     The first argument list is for the cards that are taken from the deck, where one must be removed.
        ///     The second argument list is for the cards that are offered to the player, where one must be added.
        /// </summary>
        public readonly UnityEvent<List<Card>, List<Card>> OnCardSwap = new();

        /// <summary>
        ///     Triggered once the game is over because there have been two turns without scaling a fish,
        ///     or the round limit has been reached
        /// </summary>
        public readonly UnityEvent OnGameOver = new();

        /// <summary>
        ///     Triggered once the turn number is increased when a new turn is started.
        ///     It uses the current turn number as its argument.
        /// </summary>
        public readonly UnityEvent<int> OnTurnNumberIncreased = new();

        /// <summary>
        ///     Creates the singleton of this service if it does not exist and then starts the game
        /// </summary>
        public GameService()
        {
            Instance = this;
            LogInfo("Successfully set the GameService's singleton instance");

            StartNewGame();
        }

        /// <summary>
        ///     The non-thread-safe singleton of the service
        /// </summary>
        public static GameService Instance { get; private set; }

        // TODO: Remove after player selections are implemented 
        public bool GameIsRunningJustForTest =>
            _game is { fishHasBeenScaledThisOrLastTurn: true, elapsedTurns: < Game.TURNS_IN_A_GAME };

        /// <summary>
        ///     Starts the current game by creating all required services (resetting the old ones if they existed
        ///     already), loading the deck, pool, discard pile, etc. and then drawing the starter hand
        /// </summary>
        private void StartNewGame()
        {
            LogInfo("Now starting a new game");

            CreateServices();
            RegisterEvents();
            LoadDeck();
            ResetGameVariables();

            LogInfo("Successfully started the game, now waiting for player actions");
        }

        /// <summary>
        ///     Ends the current turn when the end turn button view is clicked.
        /// </summary>
        public void EndTurn()
        {
            LogInfo("Now ending the current turn");
            Assert.IsTrue(_game.currentGameState is GameState.PLAY_CARDS, "Should currently be in the play " +
                                                                          "cards state, but is in " +
                                                                          _game.currentGameState);

            // First, the turn is ended
            _game.currentGameState = GameState.TURN_END;

            // Now apply all end-of-turn effects
            _game.currentGameState = GameState.END_OF_TURN_EFFECT_EXECUTION;
            LogInfo("Now executing all end of turn effects");
            EffectAssemblyService.Instance.ExecuteAll();

            HandleCardSwaps();

            // Increment tracking variables
            _game.elapsedTurns++;
            LogInfo($"Incrementing turn number. It now is: {_game.elapsedTurns}");

            LogInfo("Triggering the OnTurnNumberIncreased event");
            OnTurnNumberIncreased.Invoke(_game.elapsedTurns);

            // At the end, start the new turn
            _game.currentGameState = GameState.TURN_START;
        }

        /// <summary>
        ///     Resets the time available this turn and draws five cards
        /// </summary>
        public void StartTurn()
        {
            // If the game is now over, handle the end of the run
            if (CheckIfGameIsOver())
            {
                HandleGameOver();
            }
            else
            {
                _game.currentGameState = GameState.TURN_START;

                // TODO Reset time variable 

                // The player draws five cards
                PlayerHandService.Instance.Draw(5);

                _game.currentGameState = GameState.PLAY_CARDS;
            }
        }

        /// <summary>
        ///     After the start of the game, the game should not instantly expect scaling a fish, and it should be in
        ///     the play card state, and there should be no last offered cards
        /// </summary>
        private void ResetGameVariables()
        {
            _game.fishHasBeenScaledThisOrLastTurn = true;
            _game.currentGameState = GameState.GAME_OVER_CHECK;
            _game.lastOfferedCards = null;
        }

        /// <summary>
        ///     Creates the singletons of all service classes
        /// </summary>
        private void CreateServices()
        {
            LogInfo("Now creating all service singleton instances");

            new EffectAssemblyService();
            LogInfo("EffectAssemblyService has been instantiated");

            new CardVaultService();
            LogInfo("CardVaultService has been instantiated");

            new DeckService();
            LogInfo("DeckService has been instantiated");

            new CardPoolService();
            LogInfo("CardPoolService has been instantiated");

            new PlayerHandService();
            LogInfo("PlayerHandService has been instantiated");

            new DiscardPileService();
            LogInfo("DiscardPileService has been instantiated");

            new FishService();
            LogInfo("FishService has been instantiated");

            LogInfo("Successfully created all services");
        }

        /// <summary>
        ///     Registers all events that the game service is interested in
        /// </summary>
        private void RegisterEvents()
        {
            FishService.Instance.OnFishHasBeenScaled.AddListener(() => _game.fishHasBeenScaledThisOrLastTurn = true);
        }

        /// <summary>
        ///     Loads the deck piles and sets up the card pool
        /// </summary>
        private void LoadDeck()
        {
            LogInfo("Now loading the deck collections");

            // Fill the card pool with all cards from the vault
            LogInfo("Filling the card pool with all cards from the vault");
            var cardsInVault = CardVaultService.Instance.GetAll();
            foreach (var card in cardsInVault)
                for (var i = 0; i < card.NumberOfCopiesInPool; i++)
                    CardPoolService.Instance.AddCard(card);
            LogInfo($"In total, there are {CardPoolService.Instance.Size()} cards in the pool");

            // Remove the starter deck cards from the card pool
            LogInfo("Removing all cards from the starter deck from the card pool");
            var cardsInStarterDeck = DeckService.Instance.ToList();
            foreach (var card in cardsInStarterDeck) CardPoolService.Instance.RemoveCard(card);
            LogInfo($"After removal, in total, there are {CardPoolService.Instance.Size()} cards in the pool");
        }

        private void HandleGameOver()
        {
            Assert.IsTrue(_game.currentGameState is GameState.GAME_OVER_CHECK,
                "Should currently be in the game over check state!");

            LogInfo("Game over. Now ending the game");
            _game.currentGameState = GameState.GAME_OVER;

            LogInfo("Now triggering the OnGameOver event");
            OnGameOver.Invoke();
        }

        private void HandleCardSwaps()
        {
            Assert.IsTrue(_game.currentGameState is GameState.END_OF_TURN_EFFECT_EXECUTION,
                "Should currently be in the end of turn execution  state, but is in " + _game.currentGameState);
            _game.currentGameState = GameState.CARDS_SWAP;

            // First, discarding all hand cards
            PlayerHandService.Instance.DiscardHand();

            if (CanSwapCards())
            {
                LogInfo("Game is still ongoing, therefore doing the card swap now");

                // If there are more than three cards sharing the least rarity, it should be randomised
                var random = new Random();

                var maximumRarity = _game.elapsedTurns;
                LogInfo($"Maximum rarity available is {maximumRarity}");

                // Randomly select 3 cards of the player's current rarity (i.e. turn number) - 2  or less
                var finalResult = DeckService
                    .Instance
                    .ToList()
                    .Concat(DiscardPileService.Instance.ToList())
                    .Distinct(new CardComparer())
                    .Where(card => card.Rarity <= maximumRarity - 2)
                    .OrderBy(_ => random.Next())
                    .Take(3)
                    .ToList();

                // In any case, there should definitely be three elements now
                Assert.IsNotNull(finalResult[0]);
                Assert.IsNotNull(finalResult[1]);
                Assert.IsNotNull(finalResult[2]);

                LogInfo("The three random cards in both piles up to current rarity - 2 are: " +
                        string.Join(",", finalResult));

                LogInfo("Now randomly selecting three cards from the card pool up to the maximum rarity");
                var selectedCards = CardPoolService
                    .Instance
                    .ToList()
                    .Distinct(new CardComparer())
                    .Where(card => card.Rarity <= maximumRarity)
                    .OrderBy(_ => random.Next())
                    .Take(3)
                    .ToList();

                // Verifies the game integrity, there should be exactly three cards at this point
                Assert.IsNotNull(selectedCards[0]);
                Assert.IsNotNull(selectedCards[1]);
                Assert.IsNotNull(selectedCards[2]);

                // If the player would be offered the same 3 card pool cards two turns in a row, randomly swap one of
                // the offered card pool cards with another type (either of equal rarity or the next highest rarity).
                if (_game.lastOfferedCards?[0] == selectedCards[0] &&
                    _game.lastOfferedCards?[1] == selectedCards[1] &&
                    _game.lastOfferedCards?[2] == selectedCards[2])
                {
                    LogInfo("The game tries to offer the exact same cards from the card pool" +
                            " as last time, therefore randomly swapping one card with another card of equal" +
                            " or next highest rarity.");

                    Assert.IsNotNull(selectedCards[3], "Trying to swap cards with the next rare card, but " +
                                                       "there is none in the card pool.");

                    // Do the swap with the next highest rarity card 
                    var randomIndexToSwap = UnityEngine.Random.Range(0, 3);
                    selectedCards[randomIndexToSwap] = selectedCards[3];
                }

                LogInfo("The three cards chosen from the pool to swap are: " +
                        $"\n- {selectedCards[0]}" +
                        $"\n- {selectedCards[1]}" +
                        $"\n- {selectedCards[2]}");

                LogInfo("Now waiting for the player to select one card to exchange");
                LogInfo("Triggering the OnCardSwap event");
                OnCardSwap.Invoke(finalResult, selectedCards);

                // TODO remove player give choice from card pool
            }
            else
            {
                LogInfo("Because of the current turn, there will be no card swap");
            }

            LogInfo("Handled the card swap");
        }

        public void RegisterCardSwapSelections([NotNull] Card cardToRemoveFromDeck, [NotNull] Card cardToAddToDeck)
        {
            LogInfo($"Registered the selection made by the player. Removing card '{cardToRemoveFromDeck}'" +
                    $" and adding card '{cardToAddToDeck}'");
        }

        /// <summary>
        ///     Yields true if the system should swap cards or not.
        ///     During the first three turns, the system will NOT swap cards because there are too few low-rarity cards.
        /// </summary>
        /// <returns>true - if the game can swap cards; false - if it cannot</returns>
        private bool CanSwapCards()
        {
            LogInfo("Checking if the swap system can be used or not because of the current turn");
            LogInfo($"Cards should only be swapped, starting in turn {Game.TURN_TO_START_SWAPPING_CARDS}" +
                    $" until turn {Game.TURN_TO_STOP_SWAPPING_CARDS}" +
                    $" and now is turn {_game.elapsedTurns}");
            return _game.elapsedTurns is >= Game.TURN_TO_START_SWAPPING_CARDS and <= Game.TURN_TO_STOP_SWAPPING_CARDS;
        }

        /// <summary>
        ///     Yields true if the game is over and false if it is not, and the game should continue normally.
        ///     A game is considered over if there have been no scaled fish for at least two turns or if the maximum
        ///     turns in a game has been reached (or exceeded).
        /// </summary>
        /// <returns>true - if the game is over; false - if the game is not yet over</returns>
        private bool CheckIfGameIsOver()
        {
            _game.currentGameState = GameState.GAME_OVER_CHECK;
            LogInfo("Checking if the game is over");
            return !_game.fishHasBeenScaledThisOrLastTurn || _game.elapsedTurns >= Game.TURNS_IN_A_GAME;
        }
    }
}