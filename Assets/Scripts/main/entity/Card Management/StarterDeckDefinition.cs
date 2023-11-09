using System.Collections.Generic;
using main.entity.Card_Management.Card_Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace main.entity.Card_Management
{
    /// <summary>
    ///     This entity represents the deck that the player starts with once the game is loaded.
    ///     It is a scriptable singleton, there can only be one instance of this entity in the editor.
    /// </summary>
    [CreateAssetMenu(fileName = "StarterDeckDefinition", menuName = "Data/New StarterDeckDefinition", order = 0)]
    public class StarterDeckDefinition : ScriptableSingleton<StarterDeckDefinition>
    {
        /// <summary>
        ///     The starter deck as it will be assigned in the editor.
        ///     Note that there can only be a single instance of this field.
        /// </summary>
        [SerializeField] private List<StarterDeckCardAmountPair> _starterDeck;

        /// <summary>
        ///     Retrieves the starter deck as it is defined in the editor
        /// </summary>
        /// <returns>The starter deck as a copied list of cards</returns>
        public List<Card> Get()
        {
            Assert.IsFalse(_starterDeck is null, "The start deck should be set up in the Unity editor");
            List<Card> cards = new();
            foreach (var pair in _starterDeck)
                for (var i = 0; i < pair.NumberOfCopies; i++)
                    cards.Add(pair.Card);
            return cards;
        }
    }
}