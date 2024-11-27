using UnityEngine;

namespace main.service
{
    /// <summary>
    ///     The base class of all service classes equips them with the ability to log simple info messages.
    /// </summary>
    public abstract class Service
    {
        /// <summary>
        ///     If the debug mode is set to true, logs the message to the Unity console
        /// </summary>
        /// <param name="message">the message that should be logged</param>
        protected void LogInfo(string message)
        {
#if UNITY_EDITOR
            Debug.Log($"({this}): {message}");
#endif
        }
    }
}