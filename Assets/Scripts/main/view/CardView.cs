using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using TMPro;
using UnityEngine;

namespace main.view
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cardNameText;
        [SerializeField] private TMP_Text _cardRarityText;
        [SerializeField] private TMP_Text _cardTimeCostText;

        public void Render([NotNull] Card cardEntity)
        {
            _cardNameText.text = cardEntity.Name;
            _cardRarityText.text = $"(r): {cardEntity.Rarity}";
            _cardTimeCostText.text = $"(t): {cardEntity.TimeCost}";
        }
    }
}