
namespace YPPGUtilities.Core
{
    using UnityEngine;

    /// <summary>
    /// The DevelopmentEnvironment class is used to set up development code that should be
    /// ignored in builds
    /// </summary>
    public class DevelopmentEnvironment : MonoBehaviour
    {

#if UNITY_EDITOR
        private void Awake()
        {
            new Logger();

            Logger.Instance.ShouldPrintErrorLogs = true;
            Logger.Instance.ShouldPrintInfoLogs = true;
            Logger.Instance.ShouldPrintWarningLogs = true;
            Logger.Instance.ShouldPrintSuccessLogs = true;

            Logger.Instance.LogWarning("You are currently testing in the development environment");

            Logger.Instance.LogInfo("Asserting all fields of all View components in the scene");
            foreach (var view in FindObjectsOfType<View>())
            {
                view.AssertFieldsAreNotNull();
            }
            Logger.Instance.LogSuccess("Successfully asserted all fields");
        }
#endif

    }

}
