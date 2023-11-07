using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace main.entity.Card_Management
{
    /// <summary>
    ///     The Card ScriptableObject is used to
    /// </summary>
    [CreateAssetMenu(fileName = "Card", menuName = "Data/New Card")]
    public class Card : ScriptableObject
    {
        /// <summary>
        ///     The name of the card as it will be rendered on the view component.
        ///     Assigned in the Editor on the ScriptableObject.
        /// </summary>
        [InfoBox("The name of the card as it will be rendered on the view component")] [SerializeField]
        private string _cardName;

        /// <summary>
        ///     The description of the card as it will be rendered on the view.
        ///     Assigned in the Editor on the ScriptableObject.
        ///     Note that this is used instead of a description for each card effect.
        ///     Instead, each card has this field briefly describing the card.
        ///     For example, "Draw 1 card and deal 1 damage".
        /// </summary>
        [Space(20)] [InfoBox("The description of the card as it will be rendered on the view.")] [SerializeField]
        private string _cardDescription;

        /// <summary>
        ///     The sprite of the card as it will be rendered on the view.
        ///     Assigned in the Editor on the ScriptableObject.
        /// </summary>
        [Space(20)] [InfoBox("The sprite of the card as it will be rendered on the view")] [SerializeField]
        private Sprite _cardSprite;

        /// <summary>
        ///     The time required in order for the player to be able to play this card.
        ///     Right now, it is limited to 10, but feel free to change depending on the GDD.
        ///     Assigned in the Editor on the ScriptableObject.
        /// </summary>
        [Space(20)]
        [FormerlySerializedAs("_timeCost")]
        [FormerlySerializedAs("_requiredTime")]
        [FormerlySerializedAs("_requiredMana")]
        [InfoBox("The time required in order for the player to be able to play this card")]
        [Range(0, 10)]
        [SerializeField]
        private int _unitTimeCost;

        /// <summary>
        ///     All card effects IN THE SAME ORDER as they should be executed in-game.
        ///     Index zero will always be executed before index one, and so on.
        ///     Must be created as a ScriptableObject first and then set as a reference on this card.
        /// </summary>
        [Space(20)]
        [InfoBox("All card effects IN THE SAME ORDER as they should be executed in-game")]
        [Expandable]
        [SerializeField]
        private CardEffect[] _cardEffects;
    }
}