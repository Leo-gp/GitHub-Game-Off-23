using System.Diagnostics.CodeAnalysis;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using UnityEngine;
using UnityEngine.UI;

namespace main.view
{
    public class PlayerHandView : MonoBehaviour
    {
        private const float BASE_SPACING_AMOUNT = 100f;
        private const float CARD_SPACING_FACTOR = 15f;
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private HorizontalLayoutGroup _playerHandLayout;

        private void Start()
        {
            PlayerHandService.Instance.OnCardDrawn.AddListener(RenderNewCard);
            PlayerHandService.Instance.OnCardDiscarded.AddListener(RemoveCardAtIndex);
            PlayerHandService.Instance.OnHandDiscarded.AddListener(RemoveAll);

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }

        private void RenderNewCard([NotNull] Card cardEntity)
        {
            var newCardView = Instantiate(_cardViewPrefab, transform);
            newCardView.Render(cardEntity);
            _playerHandLayout.spacing -= CARD_SPACING_FACTOR;
        }

        private void RemoveCardAtIndex(int index)
        {
            var cardViewToRemove = _playerHandLayout.transform.GetChild(index);
            Destroy(cardViewToRemove.gameObject);
            _playerHandLayout.spacing += CARD_SPACING_FACTOR;
        }

        private void RemoveAll()
        {
            foreach (Transform child in _playerHandLayout.transform) Destroy(child.gameObject);

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }
    }
}