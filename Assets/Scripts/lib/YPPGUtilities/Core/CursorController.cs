namespace Core
{
    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// The CursorController class is a global controller that handles how the cursor is 
    /// represented in the game. It provides open methods to change the cursor to a different 
    /// type. Elements should only communicate with the controller via its instance.
    /// A simplified mono behaviour to do so automatically is the <see cref="CursorView"> class.
    /// 
    /// To use this controller, add it to a GameObject in a scene and set the textures to the
    /// ones you want to use for the different types of cursors. If you want this class to persist
    /// when changing scenes, make sure to set DontDestroyOnLoad for this controller.
    /// </summary>
    public class CursorController : MonoBehaviour
    {
        // Make sure to follow accessibility guidelines for cursors.
        // It is also recommended to supply 32x32 textures.
        [Header("Cursor Textures")]
        [SerializeField] private Texture2D _hoverCursor;
        [SerializeField] private Texture2D _textCursor;
        // ... copy the above for any other type of cursor

        /// <summary>
        /// The global instance of the cursor controller making use of the singleton design pattern
        /// </summary>
        public static CursorController Instance { private set; get; }

        /// <summary>
        /// Sets up the singleton instance on Awake
        /// </summary>
        private void Awake()
        {
            Assert.IsNull(Instance, "A singleton instance must be null. Is there another " +
                $"CursorController in the scene? Type: {GetType()}");

            Instance = this;
        }

        /// <summary>
        /// Changes the cursor to the default cursor of the Unity project.
        /// </summary>
        public void SetDefaultCursor() => SetCursor(null);

        /// <summary>
        /// Changes the cursor to the specified hover cursor.
        /// This can be used for elements that should receive a different cursor when hovering
        /// over them.
        /// 
        /// Note that if the _hoverCursor texture is null, Unity will fall back to the
        /// operating system's default cursor, instead.
        /// </summary>
        /// 
        public void SetHoverCursor() => SetCursor(_hoverCursor);

        /// <summary>
        /// Changes the cursor to the specified text cursor.
        /// This can be used for input fields that should receive a different cursor when hovering
        /// over them.
        /// 
        /// Note that if the _textCursor texture is null, Unity will fall back to the
        /// operating system's default cursor, instead.
        /// </summary>
        public void SetTextCursor() => SetCursor(_textCursor);

        // ... copy the method above for setting another type of cursor

        // Setting the cursor with no offset and auto cursor mode
        private void SetCursor(Texture2D texture)
        {
            Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
        }

    }
}
