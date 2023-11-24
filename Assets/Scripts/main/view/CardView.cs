using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using main.view.Canvas;
using TMPro;
using UnityEngine;

namespace main.view
{
    public class CardView : MonoBehaviour
    {
        private const float DISCARD_SPEED = 3.5f, CURVE_HEIGHT = 4f;
        [SerializeField] private TMP_Text _cardNameText;
        [SerializeField] private TMP_Text _cardRarityText;
        [SerializeField] private TMP_Text _cardTimeCostText;
        private Vector3[] _bezierNodes;

        private float _bezierTargetCount;
        private bool _isBeingDiscarded;

        private void Update()
        {
            HandlePotentialDiscard();
        }

        private void HandlePotentialDiscard()
        {
            if (!_isBeingDiscarded) return;

            if (_bezierTargetCount >= 1f) Destroy(gameObject);

            _bezierTargetCount += DISCARD_SPEED * Time.deltaTime;

            var m1 = Vector3.Lerp(_bezierNodes[0], _bezierNodes[1], _bezierTargetCount);
            var m2 = Vector3.Lerp(_bezierNodes[1], _bezierNodes[2], _bezierTargetCount);
            transform.position = Vector3.Lerp(m1, m2, _bezierTargetCount);
        }

        public void Discard()
        {
            _isBeingDiscarded = true;
            _bezierTargetCount = 0f;

            _bezierNodes = new Vector3[3];
            _bezierNodes[0] = transform.position;
            _bezierNodes[2] = PlayerHandCanvas.Instance.DiscardTargetPosition;
            _bezierNodes[1] = _bezierNodes[0] + (_bezierNodes[2] - _bezierNodes[0]) / 2 + Vector3.up * CURVE_HEIGHT;
        }

        public void Render([NotNull] Card cardEntity)
        {
            _cardNameText.text = cardEntity.Name;
            _cardRarityText.text = $"(r): {cardEntity.Rarity}";
            _cardTimeCostText.text = $"(t): {cardEntity.TimeCost}";
        }
    }
}