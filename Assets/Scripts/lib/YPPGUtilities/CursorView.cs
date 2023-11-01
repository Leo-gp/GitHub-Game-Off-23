namespace Core
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using YPPGUtilities.Core;

    /// <summary>
    /// The CursorView allows changing the cursor when hovering over the UI element
    /// </summary>
    public class CursorView : View, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// If set to true, the cursor will be set to the text cursor instead of the hover cursor 
        /// </summary>
        [SerializeField] private bool _displayTextCursorOnHoverInstead;

        /// <summary>
        /// Set the cursor to the hover or text cursor when entering
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_displayTextCursorOnHoverInstead)
            {
                CursorController.Instance.SetTextCursor();
            }
            else
            {
                CursorController.Instance.SetHoverCursor();
            }
        }

        /// <summary>
        /// Set the cursor to the default cursor when exiting
        /// </summary>
        public void OnPointerExit(PointerEventData eventData) =>
            CursorController.Instance.SetDefaultCursor();
    }
}
