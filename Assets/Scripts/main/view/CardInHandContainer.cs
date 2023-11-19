using System.Diagnostics.CodeAnalysis;
using main.entity.Card_Management.Card_Data;
using UnityEngine;

namespace main.view
{
    public class CardInHandContainer : MonoBehaviour
    {
        [SerializeField] private CardView _cardViewPrefab;

        private CardView _child;

        public void CreateChild([NotNull] Card cardToContain)
        {
            var newCardView = Instantiate(_cardViewPrefab, transform);
            newCardView.Render(cardToContain);
            _child = newCardView;
        }
    }
}