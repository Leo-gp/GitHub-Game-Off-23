using System.Collections.Generic;
using UnityEngine;

namespace main.entity.Card_Management
{
    /// <summary>
    ///     This entity represents the deck that the player starts with once the game is loaded.
    ///     It is a scriptable singleton, there can only be one instance of this entity in the editor.
    /// </summary>
    [CreateAssetMenu(fileName = "StarterDeckDefinition", menuName = "Data/New StarterDeckDefinition", order = 0)]
    public class StarterDeckDefinition : ScriptableObject
    {
        /// <summary>
        ///     The starter deck as it will be assigned in the editor.
        ///     Note that there can only be a single instance of this field.
        /// </summary>
        [SerializeField] private List<StarterDeckCardAmountPair> _starterDeck;

        /// <summary>
        ///     Yields the starter deck as a list of amount, reference pairs
        /// </summary>
        public List<StarterDeckCardAmountPair> StarterDeck => _starterDeck;
    }
}