using System.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FMODUnity;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace main.view
{
    public class PlayerHandView : MonoBehaviour
    {
        private const float BASE_SPACING_AMOUNT = 25f;
        private const float CARD_SPACING_FACTOR = 15f;

        [SerializeField] private CardInHandContainer _cardViewContainerPrefab;
        [SerializeField] private HorizontalLayoutGroup _playerHandLayout;
        [SerializeField] private StudioEventEmitter _cardDrawEvent;

        private int _drawOffset;
        private PlayerHandService playerHandService;
        private DiscardPileService discardPileService;

        private List<CardInHandContainer> cardInHandContainers = new();
        
        [Inject]
        public void Construct(PlayerHandService playerHandService, DiscardPileService discardPileService)
        {
            this.playerHandService = playerHandService;
            this.discardPileService = discardPileService;
        }

        private void OnEnable()
        {
            playerHandService.OnCardDrawn.AddListener(RenderNewCard);
            discardPileService.OnDiscard += RemoveCard;

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }

        private void OnDisable()
        {
            playerHandService.OnCardDrawn.RemoveListener(RenderNewCard);
            discardPileService.OnDiscard -= RemoveCard;
        }

        public void IncreaseSpacing()
        {
            _playerHandLayout.spacing += CARD_SPACING_FACTOR;
        }

        public void DecreaseSpacing()
        {
            _playerHandLayout.spacing -= CARD_SPACING_FACTOR;
        }
        
        public void PlayCard(Card card)
        {
            playerHandService.PlayCard(card);
        }

        private void RenderNewCard([NotNull] Card cardEntity)
        {
            var newCardViewContainer = Instantiate(_cardViewContainerPrefab, transform);
            cardInHandContainers.Add(newCardViewContainer);
            _drawOffset++;
            StartCoroutine(CreateCardAfterTime(newCardViewContainer, cardEntity, _drawOffset));
        }
        
        private IEnumerator CreateCardAfterTime(
            [NotNull] CardInHandContainer container,
            [NotNull] Card cardEntity,
            int offset)
        {
            // Guarantee to wait one frame
            yield return new WaitForEndOfFrame();

            // Now create a slight draw offset
            yield return new WaitForSeconds(0.1f * offset);

            _cardDrawEvent.Play();
            container.CreateChild(cardEntity, this);
        }

        private void RemoveCard(Card card)
        {
            var cardInHandContainer = cardInHandContainers.Find(container => container.CardView.Card == card);
            cardInHandContainer.CardView.Discard();
            cardInHandContainers.Remove(cardInHandContainer);
            Destroy(cardInHandContainer.gameObject);
        }
    }
}