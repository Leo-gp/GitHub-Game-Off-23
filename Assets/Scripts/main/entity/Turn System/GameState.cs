namespace main.entity.Turn_System
{
    /// <summary>
    /// The GameState determine the state that the game is in at any particular point.
    /// It is updated by the GameService and is used to assert program correctness.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// The game is currently starting a new turn.
        /// This includes drawing up to the maximum number of cards, etc.
        /// </summary>
        TURN_START,
        
        /// <summary>
        /// The game is currently letting the player play any number of cards.
        /// End-of-turn card effects can be enqueued to the <see cref="EffectAssembly"/> at this point.
        /// </summary>
        PLAY_CARDS,
        
        /// <summary>
        /// The game is currently ending the current turn, triggered by the player clicking the button in the game's UI.
        /// </summary>
        TURN_END,
        
        /// <summary>
        /// The game is currently executing all end-of-turn effects stored in the <see cref="EffectAssembly"/>and then
        /// clears that list.
        /// </summary>
        END_OF_TURN_EFFECT_EXECUTION,
        
        /// <summary>
        /// The game is currently checking if the game is over either because the turn limit has been exceeded or
        /// no fish has been scaled for at least two turns.
        /// </summary>
        GAME_OVER_CHECK,
        
        /// <summary>
        /// The game is currently in the game-over state, where the player can choose to replace cards from their
        /// current deck with discovered cards.
        /// </summary>
        GAME_OVER,
        
        /// <summary>
        /// The game is currently in the state, in which the player swaps out cards from their deck. 
        /// </summary>
        CARDS_SWAP
    }
}