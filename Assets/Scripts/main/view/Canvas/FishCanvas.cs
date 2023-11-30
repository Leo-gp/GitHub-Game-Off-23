using System;
using System.Collections;
using System.Collections.Generic;
using main.service.Fish_Management;
using main.view.Panels;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace main.view.Canvas
{
    public class FishCanvas : MonoBehaviour
    {
        [SerializeField] private SlashPanel _slashPrefab;
        [SerializeField] private Image _rawFishImage;

        private readonly Queue<EndOfTurnSegment> _endOfTurnQueue = new();
        private float _currentAlphaDamage;
        private FishService _fishService;
        private bool _isHandlingQueue;

        private void OnEnable()
        {
            _fishService.OnFishHasReceivedDamage.AddListener(EnqueueFishScale);
            _fishService.OnFishHasBeenScaled.AddListener(EnqueueFishKill);
        }

        private void OnDisable()
        {
            _fishService.OnFishHasReceivedDamage.RemoveListener(EnqueueFishScale);
            _fishService.OnFishHasBeenScaled.RemoveListener(EnqueueFishKill);
        }

        [Inject]
        public void Construct(FishService fishService)
        {
            _fishService = fishService;
        }

        private void EnqueueFishKill()
        {
            _endOfTurnQueue.Enqueue(new FishKillSegment());
            HandleQueueIfNotAlready();
        }

        private void EnqueueFishScale(int amountThatHasBeenScaled)
        {
            _endOfTurnQueue.Enqueue(new ScaleDamageSegment
            {
                DamageAmount = amountThatHasBeenScaled
            });

            HandleQueueIfNotAlready();
        }

        private void HandleQueueIfNotAlready()
        {
            Assert.AreNotEqual(_endOfTurnQueue.Count, 0);
            if (_isHandlingQueue) return;

            _isHandlingQueue = true;
            HandleNextSegment();
        }

        private void HandleNextSegment()
        {
            if (_endOfTurnQueue.Count is 0)
            {
                Debug.Log("Should now proceed with end of turn");
            }
            else
            {
                var nextSegment = _endOfTurnQueue.Dequeue();
                switch (nextSegment)
                {
                    case ScaleDamageSegment scaleDamageSegment:
                        var damage = scaleDamageSegment.DamageAmount;
                        _rawFishImage.color = new Color(1f, 1f, 1f,
                            TranslateDamageToRawFishAlphaColour(damage));
                        StartCoroutine(CreateSlash(TranslateDamageToSlashAmount(damage)));
                        break;
                    case FishKillSegment:
                        _currentAlphaDamage = 0f;
                        _rawFishImage.color = new Color(255f, 255f, 255, 0);
                        break;
                    default:
                        throw new NotImplementedException("Segment is not implemented");
                }
            }
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
                yield return new WaitForSeconds(0.2f);
                Instantiate(_slashPrefab, transform).Render();
                amountOfSlashes--;
            }

            HandleNextSegment();
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