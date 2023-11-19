using System.Diagnostics.CodeAnalysis;
using main.entity.Card_Management.Card_Data;
using main.view.Canvas;
using UnityEngine;
using UnityEngine.EventSystems;

namespace main.view
{
    public class CardInHandContainer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private CardView _cardViewPrefab;

        private CardView _child;

        public void OnBeginDrag(PointerEventData eventData)
        {
            PlayerHandCanvas.Instance.SetAsDirectChild(_child.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void CreateChild([NotNull] Card cardToContain)
        {
            var newCardView = Instantiate(_cardViewPrefab, transform);
            newCardView.Render(cardToContain);
            _child = newCardView;
        }
    }
}