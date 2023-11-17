using main.entity.Card_Management.Card_Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace main.entity.Card_Management.Card_Effects
{
    /// <summary>
    ///     A simple example of how classes can define card effects.
    ///     This simply defines a field "message" that will be printed upon execution.
    ///     NOTE: Can be removed once the actual card effects exist.
    /// </summary>
    [CreateAssetMenu(fileName = "TimeReductionCE", menuName = "Data/Effects/New TimeReductionCE")]
    public class TimeReductionCE : CardEffect
    {
        [FormerlySerializedAs("message")] [SerializeField]
        private string _message;

        [SerializeField] private int _reductionAmount;
        [SerializeField] private int _halfAmountInstead;
        [SerializeField] private CardClass requiredCardClass;
        [SerializeField] private bool _affectsActionCards;
        [SerializeField] private bool _affectsItemCards;
        [SerializeField] private bool _affectsAllCards;

        public override void Execute(int multiplier)
        {
            Debug.Log("My message is " + _message);
        }
    }
}