using System.Linq;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using main.service.Turn_System;
using ModestTree;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class PlayerHandViewPagination : MonoBehaviour
    {
        [SerializeField] private PlayerHandView playerHandView;
        [SerializeField] private GameObject nextCardArrowView;
        [SerializeField] private GameObject previousCardArrowView;
        [SerializeField] private int maxAmountOfCardsToDisplay;
        
        private PlayerHandService _playerHandService;
        private TurnService _turnService;

        [Inject]
        public void Construct(PlayerHandService playerHandService, TurnService turnService)
        {
            _playerHandService = playerHandService;
            _turnService = turnService;
        }

        private void OnEnable()
        {
            _playerHandService.OnCardPlayed += OnCardPlayed;
            _turnService.OnNewTurnStart.AddListener(DisableArrowsViews);
        }

        private void OnDisable()
        {
            _playerHandService.OnCardPlayed -= OnCardPlayed;
            _turnService.OnNewTurnStart.RemoveListener(DisableArrowsViews);
        }
        
        private void DisableArrowsViews()
        {
            nextCardArrowView.SetActive(false);
            previousCardArrowView.SetActive(false);
        }

        private void OnCardPlayed(Card card)
        {
            if (_playerHandService.AmountOfCardsInHand - 1 < maxAmountOfCardsToDisplay)
            {
                return;
            }
            if (_playerHandService.AmountOfCardsInHand - 1 == maxAmountOfCardsToDisplay)
            {
                playerHandView.CardInHandContainers
                    .Find(cardInHandContainer => !cardInHandContainer.isActiveAndEnabled)
                    ?.gameObject
                    .SetActive(true);
                DisableArrowsViews();
                return;
            }
            if (nextCardArrowView.activeInHierarchy)
            {
                EnableRight();
            }
            else
            {
                EnableLeft();
            }
        }

        public void ShiftRightmost()
        {
            if (playerHandView.CardInHandContainers.Count <= maxAmountOfCardsToDisplay)
            {
                return;
            }
            for (var i = playerHandView.CardInHandContainers.Count - 1; i >= 0; i--)
            {
                playerHandView.CardInHandContainers[i].gameObject
                    .SetActive(playerHandView.CardInHandContainers.Count - 1 - i < maxAmountOfCardsToDisplay);
            }
            nextCardArrowView.SetActive(false);
            previousCardArrowView.SetActive(true);
        }
        
        public void ShiftRight()
        {
            var hasDisabledLeftCard = false;
            
            for (var i = 0; i < playerHandView.CardInHandContainers.Count; i++)
            {
                if (!hasDisabledLeftCard && playerHandView.CardInHandContainers[i].isActiveAndEnabled)
                {
                    playerHandView.CardInHandContainers[i].gameObject.SetActive(false);
                    hasDisabledLeftCard = true;
                }
                else if (hasDisabledLeftCard && !playerHandView.CardInHandContainers[i].isActiveAndEnabled)
                {
                    playerHandView.CardInHandContainers[i].gameObject.SetActive(true);
                    if (i == playerHandView.CardInHandContainers.Count - 1)
                    {
                        nextCardArrowView.SetActive(false);
                    }
                    break;
                }
            }
            previousCardArrowView.SetActive(true);
        }
        
        public void ShiftLeft()
        {
            var hasDisabledRightCard = false;
            
            for (var i = playerHandView.CardInHandContainers.Count - 1; i >= 0; i--)
            {
                if (!hasDisabledRightCard && playerHandView.CardInHandContainers[i].isActiveAndEnabled)
                {
                    playerHandView.CardInHandContainers[i].gameObject.SetActive(false);
                    hasDisabledRightCard = true;
                }
                else if (hasDisabledRightCard && !playerHandView.CardInHandContainers[i].isActiveAndEnabled)
                {
                    playerHandView.CardInHandContainers[i].gameObject.SetActive(true);
                    if (i == 0)
                    {
                        previousCardArrowView.SetActive(false);
                    }
                    break;
                }
            }
            nextCardArrowView.SetActive(true);
        }

        private void EnableRight()
        {
            var disabledCardInHandContainersOnTheRight = Enumerable.Reverse(playerHandView.CardInHandContainers)
                .TakeWhile(cardInHandContainer => !cardInHandContainer.isActiveAndEnabled)
                .ToList();
            if (disabledCardInHandContainersOnTheRight.IsEmpty())
            {
                return;
            }
            if (disabledCardInHandContainersOnTheRight.Count is 1)
            {
                nextCardArrowView.SetActive(false);
            }
            disabledCardInHandContainersOnTheRight.Last().gameObject.SetActive(true);
        }
        
        private void EnableLeft()
        {
            var disabledCardInHandContainersOnTheLeft = playerHandView.CardInHandContainers
                .TakeWhile(cardInHandContainer => !cardInHandContainer.isActiveAndEnabled)
                .ToList();
            if (disabledCardInHandContainersOnTheLeft.IsEmpty())
            {
                return;
            }
            if (disabledCardInHandContainersOnTheLeft.Count is 1)
            {
                previousCardArrowView.SetActive(false);
            }
            disabledCardInHandContainersOnTheLeft.Last().gameObject.SetActive(true);
        }
    }
}