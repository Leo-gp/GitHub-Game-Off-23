using JetBrains.Annotations;
using main.entity.Turn_System;
using UnityEngine.Assertions;

namespace main.service.Turn_System
{
    /// <summary>
    ///     This service provides the business logic for the game entity, including a way to end the current turn.
    /// </summary>
    public class GameService
    {
        /// <summary>
        ///     The non-null game entity created automatically when the service is instantiated.
        /// </summary>
        [NotNull] private readonly Game _game = new();

        /// <summary>
        ///     Creates the singleton of this service if it does not exist
        /// </summary>
        public GameService()
        {
            Assert.IsNull(Instance, "There already is a service singleton instance! Should not try to " +
                                    "create a new instance of a singleton!");

            Instance = this;
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