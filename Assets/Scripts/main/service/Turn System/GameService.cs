using System;
using System.Linq;
using main.entity.Card_Management.Card_Data;
using JetBrains.Annotations;
using System.Collections.Generic;
using main.entity.Turn_System;
using main.service.Card_Management;
using main.service.Fish_Management;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace main.service.Turn_System
{
    /// <summary>
    ///     This service provides the business logic for the game entity, including a way to end the current turn.
    /// </summary>
    public class GameService : Service, ITurnDrawPhaseActor, ITurnPlayPhaseActor, ITurnEndPhaseActor
    {
        private readonly Game game;
        private readonly Turn turn;
        private readonly FishService fishService;
        private readonly DeckService deckService;
        private readonly DiscardPileService discardPileService;
        private readonly CardPoolService cardPoolService;
        private readonly PlayerHandService playerHandService;

        public GameService
        (
            Game game,
            Turn turn,
            FishService fishService,
            DeckService deckService,
            DiscardPileService discardPileService,
            CardPoolService cardPoolService,
            PlayerHandService playerHandService
        )
        {
            this.game = game;
            this.turn = turn;
            this.fishService = fishService;
            this.deckService = deckService;
            this.discardPileService = discardPileService;
            this.cardPoolService = cardPoolService;
            this.playerHandService = playerHandService;
            
            StartNewGame();
        }
        
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
        ///     Starts the current game by creating all required services (resetting the old ones if they existed
        ///     already), loading the deck, pool, discard pile, etc. and then drawing the starter hand
        /// </summary>
        private void StartNewGame()
        {
            LogInfo("Now starting a new game");

            RegisterEvents();
            ResetGameVariables();

            LogInfo("Successfully started the game, now waiting for player actions");
        }
        
        // TODO: Remove after player selections are implemented 
        public bool GameIsRunningJustForTest => game.fishHasBeenScaledThisOrLastTurn && turn.CurrentTurnNumber < Game.TURNS_IN_A_GAME;
        
        public void OnDrawStarted()
        {
            game.currentGameState = GameState.TURN_START;
        }
        
        public void OnPlayPhaseStarted()
        {
            LogInfo("Now starting the play cards phase");
            Assert.IsTrue(game.currentGameState is GameState.TURN_START, "Should currently be in the turn start state, but is in " +
                                                                         game.currentGameState);
            game.currentGameState = GameState.PLAY_CARDS;
        }

        public void OnTurnEnded()
        {
            LogInfo("Now ending the current turn");
            Assert.IsTrue(game.currentGameState is GameState.PLAY_CARDS, "Should currently be in the play cards state, but is in " +
                                                                         game.currentGameState);
            // First, the turn is ended
            game.currentGameState = GameState.TURN_END;
            
            // Now apply all end-of-turn effects
            game.currentGameState = GameState.END_OF_TURN_EFFECT_EXECUTION;
            LogInfo("Now executing all end of turn effects");
            
            if (CheckIfGameIsOver())
            {
                HandleGameOver();
                return;
            }
            
            HandleCardSwaps();
        }

        /// <summary>
        ///     After the start of the game, the game should not instantly expect scaling a fish, and it should be in
        ///     the turn start state, and there should be no last offered cards
        /// </summary>
        private void ResetGameVariables()
        {
            game.currentAmountOfScaledFish = 0;
            game.currentGameState = GameState.TURN_START;
            game.lastOfferedCards = null;
        }

        /// <summary>
        ///     Registers all events that the game service is interested in
        /// </summary>
        private void RegisterEvents()
        {
            fishService.OnFishHasBeenScaled.AddListener(() => game.currentAmountOfScaledFish++);
        }

        private void HandleGameOver()
        {
            Assert.IsTrue(game.currentGameState is GameState.GAME_OVER_CHECK,
                "Should currently be in the game over check state!");

            LogInfo("Game over. Now ending the game");
            game.currentGameState = GameState.GAME_OVER;

            LogInfo("Now triggering the OnGameOver event");
            OnGameOver.Invoke();
        }

        private void HandleCardSwaps()
        {
            Assert.IsTrue(game.currentGameState is GameState.GAME_OVER_CHECK,
                "Should currently be in the game over check execution state, but is in " + game.currentGameState);
            game.currentGameState = GameState.CARDS_SWAP;

            // First, discarding all hand cards
            playerHandService.DiscardHand();

            if (CanSwapCards())
            {
                LogInfo("Game is still ongoing, therefore doing the card swap now");

                // If there are more than three cards sharing the least rarity, it should be randomised
                var random = new Random();

                var maximumRarity = turn.CurrentTurnNumber;
                LogInfo($"Maximum rarity available is {maximumRarity}");

                // Randomly select 3 cards of the player's current rarity (i.e. turn number) - 2  or less
                var finalResult = deckService
                    .ToList()
                    .Concat(discardPileService.ToList())
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
                var selectedCards = cardPoolService
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
                if (game.lastOfferedCards?[0] == selectedCards[0] &&
                    game.lastOfferedCards?[1] == selectedCards[1] &&
                    game.lastOfferedCards?[2] == selectedCards[2])
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
                    $" and now is turn {turn.CurrentTurnNumber}");
            return turn.CurrentTurnNumber is >= Game.TURN_TO_START_SWAPPING_CARDS and <= Game.TURN_TO_STOP_SWAPPING_CARDS;
        }

        /// <summary>
        ///     Yields true if the game is over and false if it is not, and the game should continue normally.
        ///     A game is considered over if there have been no scaled fish for at least two turns or if the maximum
        ///     turns in a game has been reached (or exceeded).
        /// </summary>
        /// <returns>true - if the game is over; false - if the game is not yet over</returns>
        private bool CheckIfGameIsOver()
        {
            game.currentGameState = GameState.GAME_OVER_CHECK;
            return turn.CurrentTurnNumber >= Game.TURNS_IN_A_GAME;
        }
    }
}