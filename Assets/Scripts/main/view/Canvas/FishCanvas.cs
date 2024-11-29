using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using main.service.Fish_Management;
using main.service.Turn_System;
using main.view.Panels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace main.view.Canvas
{
    public class FishCanvas : MonoBehaviour
    {
        [SerializeField] private SlashPanel _slashPrefab;
        [SerializeField] private Image _rawFishImage;
        [SerializeField] private RemainingScalesView _remainingScalesView;
        [SerializeField] private RemainingFishView _remainingFishView;
        [SerializeField] private Animator _newFishAnimator;
        [SerializeField] private RawFishReset _rawFishReset;
        private readonly Queue<EndOfTurnSegment> _endOfTurnQueue = new();
        private float _currentAlphaDamage;
        private EffectAssemblyService _effectAssemblyService;
        private FishService _fishService;
        private TurnService _turnService;
        private int damage;
        private bool _shouldSkipCurrentAnimations;

        private bool ShouldSkipCurrentAnimations
        {
            get => GameSettingsManager.skipScaleAnimations || _shouldSkipCurrentAnimations;
            set => _shouldSkipCurrentAnimations = value;
        }

        private void OnEnable()
        {
            _fishService.OnFishHasReceivedDamage.AddListener(EnqueueFishScale);
            _fishService.OnFishHasBeenScaled.AddListener(EnqueueFishKill);
            _effectAssemblyService.OnEffectsWereExecuted.AddListener(HandleEndOfTurn);

            UpdateScalesView();
        }

        private void OnDisable()
        {
            _fishService.OnFishHasReceivedDamage.RemoveListener(EnqueueFishScale);
            _fishService.OnFishHasBeenScaled.RemoveListener(EnqueueFishKill);
            _effectAssemblyService.OnEffectsWereExecuted.RemoveListener(HandleEndOfTurn);
        }

        [Inject]
        public void Construct(FishService fishService, EffectAssemblyService effectAssemblyService,
            TurnService turnService)
        {
            _fishService = fishService;
            _effectAssemblyService = effectAssemblyService;
            _turnService = turnService;
        }

        public void SkipCurrentAnimations()
        {
            ShouldSkipCurrentAnimations = true;
            _newFishAnimator.Play("New_Fish", 0, 1);
            _rawFishReset.StopNewFishSound();
        }

        private void EnqueueFishKill()
        {
            _endOfTurnQueue.Enqueue(new FishKillSegment());
        }

        private void EnqueueFishScale(int amountThatHasBeenScaled)
        {
            _endOfTurnQueue.Enqueue(new ScaleDamageSegment
            {
                DamageAmount = amountThatHasBeenScaled
            });
        }

        private void HandleEndOfTurn()
        {
            StartCoroutine(HandleEndOfTurnSegments());
        }
        
        private IEnumerator HandleEndOfTurnSegments()
        {
            while (_endOfTurnQueue.Count > 0)
            {
                var nextSegment = _endOfTurnQueue.Dequeue();
                switch (nextSegment)
                {
                    case ScaleDamageSegment scaleDamageSegment:
                        damage = scaleDamageSegment.DamageAmount;
                        _rawFishImage.color = new Color(1f, 1f, 1f,
                            TranslateDamageToRawFishAlphaColour(damage));
                        UpdateScalesView();
                        if (!ShouldSkipCurrentAnimations)
                        {
                            yield return StartCoroutine(CreateSlash(TranslateDamageToSlashAmount(damage)));
                        }
                        break;
                    case FishKillSegment:
                        HandleFishKill();
                        if (!ShouldSkipCurrentAnimations)
                        {
                            yield return StartCoroutine(RunNewFishAnimation());
                        }
                        break;
                    default:
                        throw new NotImplementedException("Segment is not implemented");
                }
            }
            ShouldSkipCurrentAnimations = false;
            _turnService.ProceedWithEndOfTurn();
        }

        private void HandleFishKill()
        {
            _currentAlphaDamage = 0f;
            UpdateScalesView();
            _remainingFishView.IncrementAndRender();
        }
        
        private IEnumerator RunNewFishAnimation()
        {
            _newFishAnimator.Play("New_Fish");
            var elapsedTime = 0f;
            const float waitTime = 2.3f;
            while (elapsedTime < waitTime)
            {
                if (ShouldSkipCurrentAnimations)
                {
                    yield break;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private void UpdateScalesView()
        {
            _remainingScalesView.Render(_fishService.GetBaseScalesOfCurrentFish() - (int)_currentAlphaDamage);
        }

        private float TranslateDamageToRawFishAlphaColour(int damage)
        {
            _currentAlphaDamage += damage;
            return _currentAlphaDamage / _fishService.GetBaseScalesOfCurrentFish();
        }

        private static int TranslateDamageToSlashAmount(int damage)
        {
            return damage switch
            {
                >= 100 => 5,
                >= 50 => 4,
                >= 20 => 3,
                >= 10 => 2,
                _ => 1
            };
        }

        private IEnumerator CreateSlash(int amountOfSlashes)
        {
            while (amountOfSlashes > 0)
            {
                if (ShouldSkipCurrentAnimations)
                {
                    yield break;
                }
                Instantiate(_slashPrefab, transform).Render();
                amountOfSlashes--;
                var elapsedTime = 0f;
                const float waitTime = 0.2f;
                while (elapsedTime < waitTime)
                {
                    if (ShouldSkipCurrentAnimations)
                    {
                        yield break;
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
        }

        private abstract class EndOfTurnSegment
        {
        }

        private class ScaleDamageSegment : EndOfTurnSegment
        {
            public int DamageAmount { set; get; }
        }

        private class FishKillSegment : EndOfTurnSegment
        {
        }
    }
}