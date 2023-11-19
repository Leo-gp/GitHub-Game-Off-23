using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace main.view
{
    public class CardView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private TMP_Text _cardNameText;
        [SerializeField] private TMP_Text _cardRarityText;
        [SerializeField] private TMP_Text _cardTimeCostText;

        public void OnBeginDrag(PointerEventData eventData)
        {
        }


        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // throw new NotImplementedException();
        }

        public void Render([NotNull] Card cardEntity)
        {
            _cardNameText.text = cardEntity.Name;
            _cardRarityText.text = $"(r): {cardEntity.Rarity}";
            _cardTimeCostText.text = $"(t): {cardEntity.TimeCost}";
        }
    }
}