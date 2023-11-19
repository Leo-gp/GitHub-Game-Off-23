using System.Diagnostics.CodeAnalysis;
using main.entity.Card_Management.Card_Data;
using main.view.Canvas;
using UnityEngine;
using UnityEngine.EventSystems;

namespace main.view
{
    public class CardInHandContainer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private const float CLAMP_WIDTH = 20f, CLAMP_HEIGHT = 10f, PLAY_HEIGHT_LIMIT = -6.5f;
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private Animator _animator;

        private CardView _child;
        private RectTransform _childRectTransform;
        private CardPlayState _playState;

        public void OnBeginDrag(PointerEventData eventData)
        {
            PlayerHandCanvas.Instance.SetAsDirectChild(_child.transform);
            _playState = CardPlayState.UNPLAYABLE;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var pos = PlayerHandCanvas
                .Instance
                .PooledMainCamera
                .ScreenToWorldPoint(eventData.position);

            _childRectTransform.position = new Vector3(Mathf.Clamp(pos.x, -CLAMP_WIDTH, CLAMP_WIDTH),
                Mathf.Clamp(pos.y, -CLAMP_HEIGHT, CLAMP_HEIGHT));

            var lastPlayState = _playState;

            _playState = pos.y >= PLAY_HEIGHT_LIMIT ? CardPlayState.PLAYABLE : CardPlayState.UNPLAYABLE;

            if (lastPlayState == _playState) return;

            _animator.Play(_playState is CardPlayState.PLAYABLE
                ? "CardInHandContainer_Shrink"
                : "CardInHandContainer_Expand");
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
            _playState = CardPlayState.IDLE;
        }

        private enum CardPlayState
        {
            PLAYABLE,
            UNPLAYABLE,
            IDLE
        }
    }
}