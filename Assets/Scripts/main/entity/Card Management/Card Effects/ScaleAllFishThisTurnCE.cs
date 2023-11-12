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
    [CreateAssetMenu(fileName = "ScaleAllFishThisTurnCE", menuName = "Data/Effects/New ScaleAllFishThisTurnCE")]
    public class ScaleAllFishThisTurnCE : CardEffect
    {
        [FormerlySerializedAs("message")] [SerializeField]
        private string _message;
        [SerializeField] private int _baseAmountOfScales;
        [SerializeField] private float _relativeAmountOfScales;
        //public ItemCard asLongAsItemOnBoard;

        public override void Execute()
        {
            Debug.Log("My message is " + _message);
        }
    }
}