using System;
using main.entity.Card_Management.Card_Data;
using UnityEngine;

namespace main.entity.Card_Management.Deck_Definition
{
    /// <summary>
    ///     Provides a way to use a serialized pair of a card reference and the amount of times that card should be
    ///     added to the deck
    /// </summary>
    [Serializable]
    public struct CardCopies
    {
        /// <summary>
        ///     Reference to a card that should be in the deck
        /// </summary>
        [Tooltip("Reference to a card that should be in the deck")]
        [SerializeField]
        private Card card;
        
        /// <summary>
        ///     The amount of times the card should be added to the deck
        /// </summary>
        [Tooltip("The amount of times the card should be added to the deck")]
        [Min(1)]
        [SerializeField]
        private int numberOfCopies;
    }
}