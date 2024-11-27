using System.Collections;
using Core;
using main.service.Turn_System;
using UnityEngine;
using Zenject;

namespace main.view.Buttons
{
    public class SkipAnimationsButton : MonoBehaviour
    {
        [SerializeField] private CanvasGroup buttonViewCanvasGroup;
        [SerializeField] private float fadeTime;

        private TurnService turnService;
        
        [Inject]
        public void Construct(TurnService turnService)
        {
            this.turnService = turnService;
        }

        private void OnEnable()
        {
            buttonViewCanvasGroup.interactable = false;
            buttonViewCanvasGroup.alpha = 0;
            turnService.OnBeforeEndOfTurn.AddListener(Display);
            turnService.OnNewTurnStart.AddListener(Hide);
            GameSettingsManager.onSettingsChanged += OnGameSettingsChanged;
            if (GameSettingsManager.skipScaleAnimations)
            {
                DisableButtonView();
            }
        }

        private void OnDisable()
        {
            turnService.OnBeforeEndOfTurn.RemoveListener(Display);
            turnService.OnNewTurnStart.RemoveListener(Hide);
            GameSettingsManager.onSettingsChanged -= OnGameSettingsChanged;
        }
        
        private void Display()
        {
            buttonViewCanvasGroup.interactable = true;
            StartCoroutine(Fade(0, 1));
        }

        private void Hide()
        {
            if (turnService.CurrentTurnNumber <= 1)
            {
                return;
            }
            buttonViewCanvasGroup.interactable = false;
            StartCoroutine(Fade(1, 0));
        }

        private IEnumerator Fade(float initialValue, float endValue)
        {
            var elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                var interpolationValue = elapsedTime / fadeTime;
                buttonViewCanvasGroup.alpha = Mathf.Lerp(initialValue, endValue, interpolationValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            buttonViewCanvasGroup.alpha = endValue;
        }
        
        private void OnGameSettingsChanged()
        {
            if (GameSettingsManager.skipScaleAnimations)
            {
                DisableButtonView();
            }
            else
            {
                EnableButtonView();
            }
        }
        
        private void EnableButtonView()
        {
            buttonViewCanvasGroup.gameObject.SetActive(true);
        }
        
        private void DisableButtonView()
        {
            buttonViewCanvasGroup.gameObject.SetActive(false);
        }
    }
}