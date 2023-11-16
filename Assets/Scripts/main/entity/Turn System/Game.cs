using main.entity.Card_Management.Card_Data;

namespace main.entity.Turn_System
{
    /// <summary>
    ///     This entity is responsible for storing information about the current game.
    ///     This includes the state the game is in at the moment, the amount of turns that have elapsed since game
    ///     start, and a tracker for fish scaling.
    /// </summary>
    public class Game
    {
        /// <summary>
        ///     Defines how many turns there are in a game
        /// </summary>
        public const int TURNS_IN_A_GAME = 20;

        /// <summary>
        ///     Defines the turn in which the card swap system will start.
        ///     This prevents only having low-rarity card swaps in the early-game.
        /// </summary>
        public const int TURN_TO_START_SWAPPING_CARDS = 3;

        /// <summary>
        ///     Determines the non-null state that the game is in right now.
        ///     The GameService should be responsible for setting the correct game state and classes should assert as
        ///     often as possible if the current game state is the correct state to be in.
        /// </summary>
        public GameState currentGameState = GameState.TURN_START;

        /// <summary>
        ///     The turns since the game has started.
        ///     Note that this is initially set to one because the service increments this variable just before a new
        ///     turn, instead of at the start of a turn.
        /// </summary>
        public int elapsedTurns = 1;

        /// <summary>
        ///     Tracks if at least one fish has been scaled either this turn or in the last turn.
        ///     This is used to track the losing condition (in this case the run is considered lost).
        /// </summary>
        public bool fishHasBeenScaledThisOrLastTurn;

        /// <summary>
        ///     Tracks the last three offered cards by the system during the card swap phase.
        ///     This is required to be able to prevent the game from offering the same three cards two consecutive
        ///     times.
        /// </summary>
        public Card[] lastOfferedCards;
    }
}