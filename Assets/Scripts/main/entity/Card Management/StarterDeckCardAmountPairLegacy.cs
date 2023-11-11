using System;
using main.entity.Card_Management.Card_Data;
using NaughtyAttributes;
using UnityEngine;

namespace main.entity.Card_Management
{
    /// <summary>
    ///     Provides a way to use a serialized pair of a card reference and the amount of times that card should be
    ///     added to the starter deck
    /// </summary>
    [Serializable]
    public class StarterDeckCardAmountPairLegacy
    {
        /// <summary>
        ///     The card reference to the card that should be in the starter deck
        /// </summary>
        [InfoBox("The card reference to the card that should be in the starter deck")] [SerializeField]
        private Card _card;

        /// <summary>
        ///     The amount of times the card should be added to the starter deck
        /// </summary>
        [Space(20)] [InfoBox("The amount of times the card should be added to the starter deck")] [SerializeField]
        private int _numberOfCopies;

        /// <summary>
        ///     Yields the card reference as a <see cref="Card" /> entity
        /// </summary>
        public Card Card => _card;

        /// <summary>
        ///     Yields the number of copies for the card as an integer
        /// </summary>
        public int NumberOfCopies => _numberOfCopies;
    }
}