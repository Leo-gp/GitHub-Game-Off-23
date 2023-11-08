using JetBrains.Annotations;
using main.entity.Turn_System;
using main.service.Card_Management;
using UnityEngine.Assertions;

namespace main.service.Turn_System
{
    /// <summary>
    ///     This service provides the business logic for the game entity, including a way to end the current turn.
    /// </summary>
    public class GameService : Service
    {
        /// <summary>
        ///     The non-null game entity created automatically when the service is instantiated.
        /// </summary>
        [NotNull] private readonly Game _game = new();

        /// <summary>
        ///     Creates the singleton of this service if it does not exist and then starts the game
        /// </summary>
        public GameService()
        {
            Instance ??= this;
            LogInfo("Successfully set the GameService's singleton instance");

            StartNewGame();
        }

        /// <summary>
        ///     The non-thread-safe singleton of the service
        /// </summary>
        public static GameService Instance { get; private set; }

        /// <summary>
        ///     Ends the current turn when the end turn button view is clicked.
        /// </summary>
        public void EndTurn()
        {
            LogInfo("Now ending the current turn");
            Assert.IsTrue(_game.currentGameState is GameState.PLAY_CARDS, "Should currently be in the play" +
                                                                          "cards state!");

            // First, the turn is ended
            _game.currentGameState = GameState.TURN_END;
            // TODO: Unity View Stuff (animations, etc.)

            // Now apply all end-of-turn effects
            _game.currentGameState = GameState.END_OF_TURN_EFFECT_EXECUTION;
            EffectAssemblyService.Instance.ExecuteAll();

            // If the game is now over, handle the end of the run
            if (GameIsOver()) HandleGameOver();

            // If the game is not yet over, handle the card swap system
            else HandleCardSwaps();

            // Increment tracking variables
            _game.elapsedTurns++;
            // TODO: Refresh available time

            // At the end, start the new turn
            _game.currentGameState = GameState.TURN_START;
            // TODO: Do view stuff 
        }

        private void StartNewGame()
        {
            LogInfo("Now starting a new game");

            CreateServices();
            LoadDeck();
            DrawStartingHand();
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

            // TODO: Create all services here

            LogInfo("Successfully created all services");
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
            foreach (var card in cardsInVault) CardPoolService.Instance.AddCard(card);

            // Remove the starter deck cards from the card pool
            LogInfo("Removing all cards from the starter deck from the card pool");
            var cardsInStarterDeck = DeckService.Instance.GetDeck().Pile;
            foreach (var card in cardsInStarterDeck) CardPoolService.Instance.RemoveCard(card);
        }

        /// <summary>
        ///     Draws the required amount of cards the player starts with
        /// </summary>
        private void DrawStartingHand()
        {
            LogInfo("Now drawing starting hand");
            // TODO
        }

        private void HandleGameOver()
        {
            Assert.IsTrue(_game.currentGameState is GameState.GAME_OVER_CHECK,
                "Should currently be in the game over check state!");
            _game.currentGameState = GameState.GAME_OVER;

            // TODO: Do the roguelike end of game stuff from the diagram
        }

        private void HandleCardSwaps()
        {
            Assert.IsTrue(_game.currentGameState is GameState.GAME_OVER_CHECK,
                "Should currently be in the game over check state!");
            _game.currentGameState = GameState.CARDS_SWAP;

            // TODO: Discard remaining cards, search for lowest rarity, etc.
        }

        /// <summary>
        ///     Yield true if the game is over and false if it is not, and the game should continue normally.
        ///     A game is considered over if there have been no scaled fish for at least two turns or if the maximum
        ///     turns in a game has been reached (or exceeded).
        /// </summary>
        /// <returns>true - if the game is over; false - if the game is not yet over</returns>
        private bool GameIsOver()
        {
            _game.currentGameState = GameState.GAME_OVER_CHECK;
            return !_game.fishHasBeenScaledThisOrLastTurn || _game.elapsedTurns >= Game.TURNS_IN_A_GAME;
        }
    }
}