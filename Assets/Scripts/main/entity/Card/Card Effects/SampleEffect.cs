
using UnityEngine;

namespace CardManagement
{
    /// <summary>
    /// A simple example of how classes can define card effects.
    /// This simply defines a field "message" that will be printed upon execution.
    /// NOTE: Can be removed once the actual card effects exist. 
    /// </summary>
    /// <author>Gino</author>
    [CreateAssetMenu(fileName = "Sample Card Effect", menuName = "Data/Effects/New Sample")]
    public class SampleEffect : CardEffect
    {
        [SerializeField] private string message;

        public override void Execute()
        {
            Debug.Log("My message is " + message);
        }
    }

}
