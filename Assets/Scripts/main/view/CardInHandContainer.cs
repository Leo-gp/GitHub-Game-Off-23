using System.Diagnostics.CodeAnalysis;
using main.entity.Card_Management.Card_Data;
using main.view.Canvas;
using UnityEngine;
using UnityEngine.EventSystems;

namespace main.view
{
    public class CardInHandContainer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private const float CLAMP_WIDTH = 20f, CLAMP_HEIGHT = 10f;
        [SerializeField] private CardView _cardViewPrefab;

        private CardView _child;
        private RectTransform _childRectTransform;

        public void OnBeginDrag(PointerEventData eventData)
        {
            PlayerHandCanvas.Instance.SetAsDirectChild(_child.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var pos = PlayerHandCanvas
                .Instance
                .PooledMainCamera
                .ScreenToWorldPoint(eventData.position);

            _childRectTransform.position = new Vector3(Mathf.Clamp(pos.x, -CLAMP_WIDTH, CLAMP_WIDTH),
                Mathf.Clamp(pos.y, -CLAMP_HEIGHT, CLAMP_HEIGHT));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void CreateChild([NotNull] Card cardToContain)
        {
            var newCardView = Instantiate(_cardViewPrefab, transform);
            newCardView.Render(cardToContain);

            _child = newCardView;
            _childRectTransform = _child.GetComponent<RectTransform>();
        }
    }
}