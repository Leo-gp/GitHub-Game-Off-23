
namespace YPPGUtilities.Core
{
    using System.Reflection;
    using global::Core;
    using UnityEngine;

    /// <summary>
    /// The View class is used to represent UI GameObjects adapting the Model-View-Controller
    /// pattern. If started from the development environment, all views will be checked for 
    /// missing fields that can be easily forgotten sometimes.
    /// </summary>
    public abstract class View : MonoBehaviour
    {

        protected virtual void Start()
        {
            RegisterEvents();
        }

        public virtual void RegisterEvents() { }

        protected LoggingOptions _loggingOptions = new()
        {
            EnableLogging = false
        };

        /// <summary>
        /// ! Uses reflection, which can have drastic impact on performance, and should only be 
        /// ! used during development. NEVER in game builds.
        /// 
        /// Asserts if all [SerializeField] fields of the view are not null
        /// </summary>
        public void AssertFieldsAreNotNull()
        {
            Logger.Instance.LogInfo($"Asserting fields of '{this}'", _loggingOptions);

            var viewType = GetType();
            FieldInfo[] fields = viewType.GetFields
            (
                BindingFlags.Instance |
                BindingFlags.NonPublic
            );

            bool reachedDerivedClass = false;

            foreach (FieldInfo field in fields)
            {
                Logger.Instance.LogInfo($"Found [SerializeField] on view called '{field}'",
                    _loggingOptions);

                var length = field.GetCustomAttributes(typeof(SerializeField), true).Length;
                if (reachedDerivedClass && length > 0)
                {
                    object value = field.GetValue(this) ??
                        throw new MissingComponentException($"Field '{field.Name}' is null. " +
                            "Did you forget to assign it in the inspector?");
                }

                if (field.DeclaringType == viewType)
                {
                    // We reached the fields of the derived class.
                    reachedDerivedClass = true;
                }
            }
        }

    }

}
